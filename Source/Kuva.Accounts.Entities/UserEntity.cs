using System;

namespace Kuva.Accounts.Entities
{
    public class UserEntity
    {
        public long Id { get; set;  }
        public UserLevels UserLevelId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Passcode { get; set; }
        public bool Active { get; set; }
        public DateTime CreateAt { get; set; }
    }
}