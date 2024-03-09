using System;
using Kuva.Accounts.Entities;
using RandomTestValues;

namespace Kuva.Accounts.Tests.Fixtures
{
    public static class UserTokenFixture
    {
        public static UserTokenEntity GetDefaultValuesFor(long userId)
        {
            return new()
            {
                Id = "14f75ee3-efed-42b8-898b-3676911c69e7",
                UserId = userId,
                ConfirmationCode = RandomValue.String(40),
                CreateAt = RandomValue.DateTime(),
            };
        }

        public static UserTokenEntity GetRegisteredTokenFor(long userId)
        {
            return new()
            {
                Id = "f7e4e1ff-6727-45a2-a9d5-4c0dce034879",
                UserId = userId,
                ConfirmationCode = "XPTO123456",
                CreateAt = DateTime.Now.AddMinutes(-20),
            };
        }
        
        public static UserTokenEntity GetRegisteredExpiredTokenFor(long userId)
        {
            return new()
            {
                Id = "f7e4e1ff-6727-45a2-a9d5-4c0dce034879",
                UserId = userId,
                ConfirmationCode = "XPTO123456",
                CreateAt = DateTime.Now.AddHours(-2),
            };
        }
    }
}