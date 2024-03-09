using Kuva.Accounts.Service.Models;
using RandomTestValues;

namespace Kuva.Accounts.Tests.Fixtures
{
    public static class ChangePasswordFixture
    {
        public static ChangePassowordModel GetDefaultValues()
        {
            var password = RandomValue.String(12);
            return new ChangePassowordModel
            {
                ConfirmationCode = RandomValue.String(40),
                ConfirmPassword = password,
                NewPassword = password 
            };
        }
    }
}