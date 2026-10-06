using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NorthWind.Entities;

namespace NorthWind.UserServices.DependencyInjection;

public static class UserServiceCollectionExtensions
{
    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.TryAddScoped<IUserService, UserService>();
        return services;
    }
}
