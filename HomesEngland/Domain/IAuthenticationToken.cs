using System;

namespace HomesEngland.Domain
{
    public interface IAuthenticationToken:IDatabaseEntity<int>
    {
        string Email { get; }
        string Token { get; }
        DateTime Expiry { get; set; }
    }
}
