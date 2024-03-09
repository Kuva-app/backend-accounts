using System.Linq;
using System.Threading.Tasks;
using Kuva.Accounts.Business;
using Kuva.Accounts.Business.Exceptions;
using Kuva.Accounts.Business.Interfaces;
using Kuva.Accounts.Service.Controllers;
using Kuva.Accounts.Tests.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Kuva.Accounts.Tests.Controllers
{
    public class PasswordControllerTest
    {
        public PasswordControllerTest()
        {
            _controller = new PasswordController(_requestChangePasswordBusinessMock.Object, _changePasswordBusinessMock.Object);
        }

        private readonly Mock<IRequestChangePasswordBusiness> _requestChangePasswordBusinessMock = new();
        private readonly Mock<IChangePasswordBusiness> _changePasswordBusinessMock = new();
        private readonly PasswordController _controller;

        [Fact]
        public async Task RequestPasswordOkTest()
        {
            _requestChangePasswordBusinessMock.Reset();

            _requestChangePasswordBusinessMock
                .Setup(s => s.RequestByUserEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<string>());

            var action = await _controller.RequestChangePassword(UserFixture.GetRegisteredUsers().First().Email);
 
            Assert.IsType<OkResult>(action);
            var result = (OkResult) action;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
        
        [Fact]
        public async Task RequestPasswordBadRequestTest()
        {
            _requestChangePasswordBusinessMock.Reset();

            _requestChangePasswordBusinessMock
                .Setup(s => s.RequestByUserEmailAsync(It.IsAny<string>()))
                .ThrowsAsync(new RequestChangePasswordException(AccountsErrors.UserNotFound));

            var action = await _controller.RequestChangePassword(UserFixture.GetRegisteredUsers().First().Email);
 
            Assert.IsType<NotFoundResult>(action);
            var result = (NotFoundResult) action;
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }
        
        [Fact]
        public async Task RequestPasswordConflictTest()
        {
            _requestChangePasswordBusinessMock.Reset();

            _requestChangePasswordBusinessMock
                .Setup(s => s.RequestByUserEmailAsync(It.IsAny<string>()))
                .ThrowsAsync(new RequestChangePasswordException(AccountsErrors.RequestChangePasswordTokenAlreadyRegistered));

            var action = await _controller.RequestChangePassword(UserFixture.GetRegisteredUsers().First().Email);
 
            Assert.IsType<ConflictObjectResult>(action);
            var result = (ConflictObjectResult) action;
            Assert.Equal(StatusCodes.Status409Conflict, result.StatusCode);
        }

        [Fact]
        public async Task ChangePasswordOkTest()
        {
            var currentUser = UserFixture.GetRegisteredUsers().First();
            
            _changePasswordBusinessMock.Setup(s => 
                s.ChangePasswordByUserIdAsync(It.IsAny<long>(), 
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(true);
            
            var action = await _controller.ChangePassword(currentUser.Id, ChangePasswordFixture.GetDefaultValues(),
                GeneralFixture.ApiToken);

            Assert.IsType<OkResult>(action);
            var result = (OkResult) action;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
        
        [Fact]
        public async Task ChangePasswordNotFoundTest()
        {
            var currentUser = UserFixture.GetRegisteredUsers().First();

            _changePasswordBusinessMock.Setup(s =>
                    s.ChangePasswordByUserIdAsync(It.IsAny<long>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .ThrowsAsync(new ChangePasswordException(It.IsAny<long>(), AccountsErrors.UserNotFound));
            
            var action = await _controller.ChangePassword(currentUser.Id,
                ChangePasswordFixture.GetDefaultValues(),
                GeneralFixture.ApiToken);

            Assert.IsType<NotFoundResult>(action);
            var result = (NotFoundResult) action;
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }
          
        [Fact]
        public async Task ChangePasswordBadRequestTest()
        {
            var currentUser = UserFixture.GetRegisteredUsers().First();

            _changePasswordBusinessMock.Setup(s =>
                    s.ChangePasswordByUserIdAsync(It.IsAny<long>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .ThrowsAsync(new ChangePasswordException(It.IsAny<long>(), AccountsErrors.PasswordToBig));
            
            var action = await _controller.ChangePassword(currentUser.Id,
                ChangePasswordFixture.GetDefaultValues(),
                GeneralFixture.ApiToken);

            Assert.IsType<BadRequestObjectResult>(action);
            var result = (BadRequestObjectResult) action;
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }
    }
}