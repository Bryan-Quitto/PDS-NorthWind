using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthWind.DomainLogs.Entities.Interfaces;
using NorthWind.DomainLogs.Entities.Services;
using NorthWind.Entities;
using NorthWind.Events.Entities.Interfaces;
using NorthWind.Events.Entities.Services;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Ports;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Backend.Controllers;
using NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;
using NorthWind.Sales.Backend.DataContexts.EFCore.Options;
using NorthWind.Sales.Backend.Presenters;
using NorthWind.Sales.Backend.Repositories.Interfaces;
using NorthWind.Sales.Backend.Repositories.Repositories;
using NorthWind.Sales.Backend.SmtpGateways.Options;
using NorthWind.Sales.Backend.SmtpGateways.Services;
using NorthWind.Sales.Backend.UseCases.CreateOrder;
using NorthWind.Sales.Backend.UseCases.EventHandlers;
using NorthWind.Sales.Backend.UseCases.Events;
using NorthWind.Sales.Backend.UseCases.Validators;
using NorthWind.Sales.Entities.Dtos;
using NorthWind.Sales.Validators.Entities;
using NorthWind.Validation.Entities.Interfaces;
using NorthWind.Validation.Entities.Services;
using NorthWind.ValidationService.FluentValidation.Services;

namespace NorthWind.Sales.Backend.IoC;

public static class DependencyContainer
{
    public static IServiceCollection AddNorthWindSalesServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Opciones (Options pattern)
        services.Configure<DBOptions>(configuration.GetSection(DBOptions.SectionKey));
        services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.SectionKey));

        // Servicios transversales de arquitectura
        NorthWind.Transactions.Entities.DependencyInjection.TransactionServiceCollectionExtensions.AddTransactionServices(services);
        NorthWind.UserServices.DependencyInjection.UserServiceCollectionExtensions.AddUserServices(services);

        // Subsistema de Membresía
        NorthWind.Membership.Backend.Core.DependencyInjection.MembershipCoreServiceCollectionExtensions.AddMembershipCoreServices(services, configuration);
        NorthWind.Membership.Backend.AspNetIdentity.DependencyInjection.MembershipAspNetIdentityServiceCollectionExtensions.AddMembershipAspNetIdentityServices(services, configuration);

        // 2. DataContexts de EF Core
        services.AddScoped<NorthWindSalesContext>();
        services.AddScoped<INorthWindSalesCommandsDataContext>(sp => sp.GetRequiredService<NorthWindSalesContext>());
        services.AddScoped<INorthWindSalesQueriesDataContext>(sp => sp.GetRequiredService<NorthWindSalesContext>());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<NorthWindSalesContext>());

        services.AddScoped<NorthWindDomainLogsContext>();
        services.AddScoped<INorthWindDomainLogsDataContext>(sp => sp.GetRequiredService<NorthWindDomainLogsContext>());

        // 3. Repositorios de adaptadores
        services.AddScoped<ICommandsRepository, CommandsRepository>();
        services.AddScoped<IQueriesRepository, QueriesRepository>();
        services.AddScoped<IDomainLogsRepository, DomainLogsRepository>();

        // 4. Auditoría y SMTP
        services.AddScoped<IDomainLogger, DomainLogger>();
        services.AddScoped<IMailService, MailService>();

        // 5. Validación genérica y adaptadores de FluentValidation
        services.AddTransient(typeof(IValidationService<>), typeof(ValidationService<>));
        services.AddTransient<IModelValidator<CreateOrderDetailDto>, CreateOrderDetailDtoValidator>();
        services.AddTransient<IModelValidator<CreateOrderDto>, CreateOrderDtoValidator>();
        services.AddTransient<IModelValidator<CreateOrderDto>, CreateOrderCustomerValidator>();
        services.AddTransient<IModelValidator<CreateOrderDto>, CreateOrderProductValidator>();
        services.AddScoped(typeof(IModelValidatorHub<>), typeof(ModelValidatorHub<>));

        // 6. Eventos de dominio
        services.AddScoped<IDomainEventHandler<SpecialOrderCreatedEvent>, SendEMailWhenSpecialOrderCreatedEventHandler>();
        services.AddScoped(typeof(IDomainEventHub<>), typeof(DomainEventHub<>));

        // 7. Puertos, Interactor y Presentador
        services.AddScoped<CreateOrderPresenter>();
        services.AddScoped<ICreateOrderOutputPort>(sp => sp.GetRequiredService<CreateOrderPresenter>());
        services.AddScoped<ICreateOrderInputPort, CreateOrderInteractor>();

        return services;
    }

    public static WebApplication MapNorthWindSalesEndpoints(this WebApplication app)
    {
        NorthWind.Membership.Backend.Core.DependencyInjection.MembershipCoreServiceCollectionExtensions.MapMembershipEndpoints(app);
        app.UseCreateOrderController();
        return app;
    }
}
