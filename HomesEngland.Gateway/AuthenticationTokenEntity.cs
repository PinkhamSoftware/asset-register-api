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

        [Column("email")]
        public string Email { get; set; }
        [Column("token")]
        public string Token { get; set; }
        [Column("expiry")]
        public DateTime Expiry { get; set; }

        [Column("modifieddatetime")]
        public DateTime ModifiedDateTime { get; set; }

        public AuthenticationTokenEntity() { }

        public AuthenticationTokenEntity(IAuthenticationToken authenticationToken)
        {
            if (authenticationToken == null)
                return;
            Id = authenticationToken.Id;
            Email = authenticationToken.Email;
            Expiry = authenticationToken.Expiry;
            Token = authenticationToken.Token;
            ModifiedDateTime = authenticationToken.Expiry;
        }
    }
}