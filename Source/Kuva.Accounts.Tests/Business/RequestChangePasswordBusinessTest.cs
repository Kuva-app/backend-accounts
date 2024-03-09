using System.Linq;
using System.Threading.Tasks;
using Kuva.Accounts.Business;
using Kuva.Accounts.Business.Exceptions;
using Kuva.Accounts.Business.Interfaces;
using Kuva.Accounts.Business.QueueConnector;
using Kuva.Accounts.Business.UseCases.Interfaces;
using Kuva.Accounts.Tests.Fixtures;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Repository.Data.Interfaces;
using Moq;

#nullable enable
namespace Kuva.Accounts.Tests.Business
{
    public class RequestChangePasswordBusinessTest
    {
        public RequestChangePasswordBusinessTest(IRequestChangePasswordUseCase requestChangePasswordUseCase)
        {
            _searchUser = new SearchUserBusiness(_userDataMock.Object);
            _requestChangePasswordUseCase = requestChangePasswordUseCase;

            _serviceQueueConnector.Setup(s => s.SendMessageToQueueAsync(It.IsAny<string>(), It.IsAny<UserTokenEntity>()))
                .Returns(Task.CompletedTask);

            _sut = new RequestChangePasswordBusiness(_userTokenDataMock.Object, 
                requestChangePasswordUseCase, _serviceQueueConnector.Object, _searchUser);
        }

        private readonly Mock<IUserData> _userDataMock = new();
        private readonly Mock<IUserTokenData> _userTokenDataMock = new();
        private readonly Mock<IServiceQueueConnector> _serviceQueueConnector = new();
        private readonly IRequestChangePasswordUseCase _requestChangePasswordUseCase;
        private readonly RequestChangePasswordBusiness _sut;
        private readonly ISearchUserBusiness _searchUser;
        
        [Fact]
        public async Task RequestChangeByUserIdSuccessfullyTest()
        {
            const long currentUserId = 1;

            _userDataMock.Setup(s => s.GetDataByPrimaryKeyAsync(It.IsAny<long>()))
                .ReturnsAsync(UserFixture.GetRegisteredUsers().First());

            _userTokenDataMock.Setup(s => s.GetUserTokenByUserIdAsync(It.IsAny<long>()))
                          .ReturnsAsync(default(UserTokenEntity));
            
            _userTokenDataMock.Setup(s => s.AddDataAsync(It.IsAny<UserTokenEntity>()))
                .ReturnsAsync(UserTokenFixture.GetDefaultValuesFor(currentUserId));

            var hashToken = await _sut.RequestByUserIdAsync(currentUserId);

            Assert.NotNull(hashToken);
        }
        
        [Fact]
        public async Task RequestChangeByEmailSuccessfullyTest()
        {
            var selectedUser = UserFixture.GetRegisteredUsers().First();

            _userDataMock.Setup(s => s.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(selectedUser);

            _userTokenDataMock.Setup(s => s.GetUserTokenByUserIdAsync(It.IsAny<long>()))
                .ReturnsAsync(default(UserTokenEntity));
            
            _userTokenDataMock.Setup(s => s.AddDataAsync(It.IsAny<UserTokenEntity>()))
                .ReturnsAsync(UserTokenFixture.GetDefaultValuesFor(selectedUser.Id));

            var hashToken = await _sut.RequestByUserEmailAsync(selectedUser.Email);

            Assert.NotNull(hashToken);
        }

        [Fact]
        public async Task RequestChangePasswordUserNotFoundErrorTest()
        {
            var expected = AccountsErrors.UserNotFound.GetAccountsErrors().Value;

            _userDataMock.Setup(s => s.GetDataByPrimaryKeyAsync(It.IsAny<long>()))
                .ReturnsAsync(default(UserEntity));

            var e = await Assert.ThrowsAsync<RequestChangePasswordException>(async () =>
            {
                const long currentUserId = 1;
                await _sut.RequestByUserIdAsync(currentUserId);
            });

            Assert.Equal(expected, e?.Message);
        }

        [Fact]
        public async Task RequestChangePasswordUserAlreadyExistsTokenTest()
        {
            const long currentUserId = 1;

            var expected = AccountsErrors.RequestChangePasswordTokenAlreadyRegistered.GetAccountsErrors().Value;
            
            _userDataMock.Setup(s => s.GetDataByPrimaryKeyAsync(It.IsAny<long>()))
                .ReturnsAsync(UserFixture.GetRegisteredUsers().First());

            _userTokenDataMock.Setup(s => s.GetUserTokenByUserIdAsync(It.IsAny<long>()))
                          .ReturnsAsync(UserTokenFixture.GetDefaultValuesFor(It.IsAny<long>()));
            
            var e = await Assert.ThrowsAsync<RequestChangePasswordException>(async () =>
            {
                await _sut.RequestByUserIdAsync(currentUserId);
            });

            Assert.Equal(expected, e?.Message);
        }

        [Fact]
        public async Task RequestChangePasswordGetBbyHasHSuccessfully()
        {
            var expected = UserFixture.GetRegisteredUsers().First();
            var userToken = UserTokenFixture.GetRegisteredTokenFor(expected.Id);
            var hash = _requestChangePasswordUseCase.GetHashByToken(userToken);

            _userTokenDataMock.Setup(s => s.GetDataByPrimaryKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(userToken);

            _userDataMock.Setup(s => s.GetDataByPrimaryKeyAsync(It.IsAny<long>()))
                .ReturnsAsync(expected);

            var actual = await _sut.GetUserByTokenHashAsync(hash);
            
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Passcode, actual.Passcode);
        }
        
        [Fact]
        public async Task RequestChangePasswordGetBbyHashExpiredTest()
        {
            var expected = UserFixture.GetUserRegisterThrowException().First();
            var userToken = UserTokenFixture.GetRegisteredExpiredTokenFor(expected.Id);
            var hash = _requestChangePasswordUseCase.GetHashByToken(userToken);

            _userTokenDataMock.Setup(s => s.GetDataByPrimaryKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(userToken);

            _userDataMock.Setup(s => s.GetDataByPrimaryKeyAsync(It.IsAny<long>())).ReturnsAsync(expected);

            var e = await Assert.ThrowsAsync<RequestChangePasswordException>(async () =>
            {
                await _sut.GetUserByTokenHashAsync(hash);
            });

            Assert.IsType<RequestChangePasswordException>(e);
            Assert.Equal(AccountsErrors.RequestChangePasswordTokenExpired, e.AccountsError);
        }
    }
}