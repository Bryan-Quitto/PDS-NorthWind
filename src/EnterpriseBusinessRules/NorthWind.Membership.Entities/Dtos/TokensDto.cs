namespace NorthWind.Membership.Entities.Dtos;

public class TokensDto
{
    public string AccessToken { get; set; } = string.Empty;

    public TokensDto() { }

    public TokensDto(string accessToken)
    {
        AccessToken = accessToken;
    }
}
