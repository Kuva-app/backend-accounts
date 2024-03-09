using Kuva.Accounts.Business;
using Kuva.Accounts.Business.Interfaces;
using Kuva.Accounts.Business.Operations;
using Kuva.Accounts.Business.UseCases.Interfaces;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Tests.Fixtures;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Kuva.Accounts.Repository.Data.Interfaces;
using Xunit;

namespace Kuva.Accounts.Tests.Business
{
    public class ClientBusinessTest
    {
        public ClientBusinessTest(IClientUseCase clientUseCase)
        {
            _clientUseCase = clientUseCase;
            var userDataMock = new Mock<IUserData>();
            _clientOperationsMock = new Mock<ClientOperations>(userDataMock.Object);
        }

        private readonly IClientUseCase _clientUseCase;
        private readonly Mock<ClientOperations> _clientOperationsMock;
        private readonly Mock<ISearchUserBusiness> _searchUserBusinessMock = new();

        [Fact]
        public async Task RegisterClientSuccessTest()
        {
            _clientOperationsMock.Reset();
            
            _clientOperationsMock.Setup(s => s.Register(It.IsAny<UserEntity>()))
                             .ReturnsAsync(UserFixture.GetUnregisteredUsers().First());

            var clientBusiness = new ClientBusiness(_clientUseCase,
                                                    _clientOperationsMock.Object,
                                                    _searchUserBusinessMock.Object);

            var newUser = UserFixture.GetUnregisteredUsers().First();

            var actual = await clientBusiness.Register(newUser, "test");

            Assert.NotNull(actual);
        }

        [Fact]
        public async Task UnregisterClientSuccessTest()
        {
            _clientOperationsMock.Reset();
            _searchUserBusinessMock.Reset();
            
            _clientOperationsMock.Setup(s => s.Unregister(It.IsAny<UserEntity>()))
                .ReturnsAsync(true);

            _searchUserBusinessMock.Setup(s => s.SearchUserByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(UserFixture.GetRegisteredUsers().First());

            ClientBusiness clientBusiness = new(_clientUseCase,
                _clientOperationsMock.Object,
                _searchUserBusinessMock.Object);

            var actual = await clientBusiness.Unregister(UserFixture.GetRegisteredUsers().First().Id);
            
            Assert.True(actual);


        }
    }
}
