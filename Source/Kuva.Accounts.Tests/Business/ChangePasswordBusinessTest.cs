using System.Linq;
using System.Threading.Tasks;
using Kuva.Accounts.Business;
using Kuva.Accounts.Business.Exceptions;
using Kuva.Accounts.Business.UseCases.Interfaces;
using Kuva.Accounts.Repository.Data.Interfaces;
using Kuva.Accounts.Tests.Fixtures;
using Moq;
using Xunit;

namespace Kuva.Accounts.Tests.Business
{
    public class ChangePasswordBusinessTest
    {
        public ChangePasswordBusinessTest(IChangePasswordUseCase changePasswordUseCase)
        {
            _changePasswordBusiness = new(_userDataMock.Object, _userTokenData.Object, changePasswordUseCase);
        }
        
        private readonly Mock<IUserData> _userDataMock = new();
        private readonly Mock<IUserTokenData> _userTokenData = new();
        private readonly ChangePasswordBusiness _changePasswordBusiness;

        [Theory]
        [InlineData(1, "XPTO123456", "123456")]
        public async Task ChangeNewPasswordSuccessfullyTest(int userId, string confirmationCode, string newPassword)
        {
            _userDataMock.Setup(s => s
                    .GetDataByPrimaryKeyAsync(It.IsAny<long>()))
                .ReturnsAsync(UserFixture.GetRegisteredUsers().First());
            
            _userDataMock.Setup(s => s
                    .ChangePasswordByIdAsync(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _userTokenData.Setup(s => s.GetUserTokenByUserIdAsync(It.IsAny<long>()))
                .ReturnsAsync(UserTokenFixture.GetRegisteredTokenFor(userId));
            
            var passwordChanged = await _changePasswordBusiness.ChangePasswordByUserIdAsync(userId,
                confirmationCode, newPassword);
            Assert.True(passwordChanged);
        }
        
        [Theory]
        [InlineData(0, "XPTO123455", "123456")]
        [InlineData(1, "XPTO123457", "")]
        [InlineData(1, "XPTO123458", null)]
        [InlineData(1, "XPTO123459", "  ")]
        public async Task ChangePasswordFailTest(int userId, string confirmationCode, string newPassword)
        {
            _userDataMock.Reset();
            
            _userDataMock.Setup(s => s
                    .GetDataByPrimaryKeyAsync(It.IsAny<long>()))
                .ReturnsAsync(UserFixture.GetRegisteredUsers().First());
            
            _userDataMock.Setup(s => s
                    .ChangePasswordByIdAsync(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var passwordChanged = false;
            await Assert.ThrowsAsync<ChangePasswordException>(async () =>
            {
                passwordChanged = await _changePasswordBusiness.ChangePasswordByUserIdAsync(userId, 
                    confirmationCode, newPassword);
            });
            Assert.False(passwordChanged);
        }
    }
}
