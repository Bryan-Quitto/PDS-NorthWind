namespace NorthWind.Membership.Backend.Core.Dtos;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;

    public UserDto() { }

    public UserDto(string id, string userName, string email, string fullName)
    {
        Id = id;
        UserName = userName;
        Email = email;
        FullName = fullName;
    }
}
