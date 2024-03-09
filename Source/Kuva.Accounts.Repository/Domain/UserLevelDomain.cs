using System;
using System.Collections.Generic;

namespace Kuva.Accounts.Repository.Domain
{
    public sealed class UserLevelDomain
    {
        public short Id { get; set; } 
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime CreateAt { get; set; }

        public List<UserDomain> Users { get; } = [];
    }
}