using System;

namespace HomesEngland.Domain
{
    public interface IAuthenticationToken:IDatabaseEntity<int>
    {
        string ReferenceNumber { get; }
        string Token { get; }
        DateTime Expiry { get; set; }
    }
}
