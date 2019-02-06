using System;

namespace HomesEngland.Domain
{
    public class AuthenticationToken : IAuthenticationToken
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; }
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
        public string EmailAddress { get; set; }
        public DateTime ModifiedDateTime { get; set; }
    }
}
