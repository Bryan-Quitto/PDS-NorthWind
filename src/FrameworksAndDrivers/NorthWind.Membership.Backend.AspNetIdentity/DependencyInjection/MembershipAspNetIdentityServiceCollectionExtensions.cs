using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthWind.Membership.Backend.AspNetIdentity.DataContexts;
using NorthWind.Membership.Backend.AspNetIdentity.Entities;
using NorthWind.Membership.Backend.AspNetIdentity.Options;
using NorthWind.Membership.Backend.AspNetIdentity.Services;
using NorthWind.Membership.Backend.Core.Interfaces.Services;

namespace NorthWind.Membership.Backend.AspNetIdentity.DependencyInjection;

public static class MembershipAspNetIdentityServiceCollectionExtensions
{
    public static IServiceCollection AddMembershipAspNetIdentityServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MembershipDBOptions>(configuration.GetSection(MembershipDBOptions.SectionKey));

        var connectionString = configuration.GetSection(MembershipDBOptions.SectionKey)["ConnectionString"]
            ?? configuration.GetConnectionString("MembershipDB");

        services.AddDbContext<NorthWindMembershipContext>(options =>
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                options.UseSqlServer(connectionString);
            }
        });

        services.AddIdentityCore<NorthWindUser>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<NorthWindMembershipContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IMembershipService, MembershipService>();

        return services;
    }
}
