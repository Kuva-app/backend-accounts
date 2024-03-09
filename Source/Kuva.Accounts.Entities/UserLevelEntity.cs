using System;

namespace Kuva.Accounts.Entities
{
    public class UserLevelEntity
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime CreateAt { get; set; }
    }
}