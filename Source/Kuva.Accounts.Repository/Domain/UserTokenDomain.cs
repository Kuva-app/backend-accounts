using System;

namespace Kuva.Accounts.Repository.Domain
{
    public sealed class UserTokenDomain
    {
        public string Id { get; set; }
        public long UserId { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTime CreateAt { get; set; }
        
        public UserDomain User { get; set; } 
    }
}