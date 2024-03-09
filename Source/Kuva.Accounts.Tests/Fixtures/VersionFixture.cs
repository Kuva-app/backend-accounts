using Kuva.Accounts.Service.Models;

namespace Kuva.Accounts.Tests.Fixtures
{
    public static class VersionFixture
    {
        public static VersionModel GetVersionDefault()
        {
            return new()
            {
                Version = "1.0.0",
                Description = "Kuva.Accounts.Service"
            };
        }
    }
}