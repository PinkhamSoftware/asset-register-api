namespace HomesEngland.Domain
{
    public interface IAuthenticationToken
    {
        string Email { get; }
        string Token { get; }
    }
}
