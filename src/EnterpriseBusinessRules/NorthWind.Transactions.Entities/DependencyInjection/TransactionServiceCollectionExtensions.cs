using Microsoft.Extensions.DependencyInjection;
using NorthWind.Transactions.Entities.Interfaces;
using NorthWind.Transactions.Entities.Services;

namespace NorthWind.Transactions.Entities.DependencyInjection;

public static class TransactionServiceCollectionExtensions
{
    public static IServiceCollection AddTransactionServices(this IServiceCollection services)
    {
        services.AddTransient<IDomainTransaction, DomainTransaction>();
        return services;
    }
}
