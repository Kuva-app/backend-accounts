using Kuva.Accounts.Service.Models;

#nullable enable
namespace Kuva.Accounts.Tests.Fixtures
{
    public static class RegisterClientModelFixture
    {
        public static RegisterClientModel GetDefaultValues()
        {
            return new()
            {
                Email = "tiago@email.com",
                Name = "Tiago Oliveira",
                Password = "12345678"
            };
        }

        public static RegisterClientModel GetNullValues()
        {
            return new()
            {
                Email = null,
                Name = null,
                Password = null
            };
        }

        public static RegisterClientModel GetInvalidEmailValue()
        {
            var current = GetDefaultValues();
            current.Email = "tiago.email.com";
            return current;
        }
    }
}