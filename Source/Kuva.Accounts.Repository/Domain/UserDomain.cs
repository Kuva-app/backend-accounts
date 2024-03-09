using System;

namespace Kuva.Accounts.Repository.Domain
{
    public sealed class UserDomain
    {
        public long Id { get; set;  }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Passcode { get; set; }
        public bool Active { get; set; }
        public DateTime CreateAt { get; set; }
        
        public short UserLevelId { get; set; }
        public UserLevelDomain UserLevel { get; set; }
        
        public UserTokenDomain UserToken { get; set; }
    }
}