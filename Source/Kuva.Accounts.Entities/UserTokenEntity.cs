using System;

namespace Kuva.Accounts.Entities
{
    public class UserTokenEntity
    {
        public string Id { get; set; }
        public string ConfirmationCode { get; set; }
        public long UserId { get; set; }
        public DateTime CreateAt { get; set; }
    }
}