using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HomesEngland.Domain;

namespace HomesEngland.Gateway
{
    [Table("authenticationtokens")]
    public class AuthenticationTokenEntity : IAuthenticationToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("referencenumber")] 
        public string ReferenceNumber { get; set; }
        [Column("token")] 
        public string Token { get; set; }
        [Column("expiry")] 
        public DateTime Expiry { get; set; }
        [Column("emailaddress")] 
        public string EmailAddress { get; set; }
        [Column("modifieddatetime")] 
        public DateTime ModifiedDateTime { get; set; }

        public AuthenticationTokenEntity()
        {
        }

        public AuthenticationTokenEntity(IAuthenticationToken authenticationToken)
        {
            if (authenticationToken == null)
                return;
            Id = authenticationToken.Id;
            ReferenceNumber = authenticationToken.ReferenceNumber;
            Expiry = authenticationToken.Expiry;
            Token = authenticationToken.Token;
            ModifiedDateTime = authenticationToken.Expiry;
            EmailAddress = authenticationToken.EmailAddress;
        }
    }
}
