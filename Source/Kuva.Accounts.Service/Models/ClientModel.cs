using System;

namespace Kuva.Accounts.Service.Models
{
    public class ClientModel
    {
        public long Id { get; set;  }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreateAt { get; set; }
    }
}