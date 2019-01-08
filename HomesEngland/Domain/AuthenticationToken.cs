namespace HomesEngland.Domain
{
    public class AuthenticationToken : IAuthenticationToken
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
