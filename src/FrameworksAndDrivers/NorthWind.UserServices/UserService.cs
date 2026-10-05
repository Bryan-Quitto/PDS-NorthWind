using Microsoft.AspNetCore.Http;
using NorthWind.Entities;

namespace NorthWind.UserServices;

public class UserService(IHttpContextAccessor httpContextAccessor) : IUserService
{
    private HttpContext? HttpContext => httpContextAccessor.HttpContext;

    public bool IsAuthenticated =>
        HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public string UserName =>
        HttpContext?.User?.Identity?.Name ?? string.Empty;

    public string FullName =>
        HttpContext?.User?.FindFirst("FullName")?.Value ?? string.Empty;
}
