using Kuva.Accounts.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace Kuva.Accounts.Tests.Repository
{
    public class AccountsContextFactoryMock
    {
        public static Mock<IDbContextFactory<AccountsContext>> Create()
        {
            var mockFactory = new Mock<IDbContextFactory<AccountsContext>>();
            mockFactory
                .Setup(_ => _.CreateDbContext())
                .Returns(() =>
                {
                    var options = new DbContextOptionsBuilder<AccountsContext>()
                        .UseInMemoryDatabase("TestDatabase")
                        .ConfigureWarnings(_ => _.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                        .Options;

                    return new AccountsContext(options);
                });
            return mockFactory;
        }
    }
}
