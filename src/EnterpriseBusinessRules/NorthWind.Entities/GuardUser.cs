namespace NorthWind.Entities;

public static class GuardUser
{
    public static void AgainstUnauthenticated(IUserService userService)
    {
        if (userService == null || !userService.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("Usuario no autenticado.");
        }
    }
}
