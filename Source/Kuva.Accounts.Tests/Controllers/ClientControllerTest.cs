using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kuva.Accounts.Business;
using Kuva.Accounts.Business.Exceptions;
using Kuva.Accounts.Business.Interfaces;
using Kuva.Accounts.Entities;
using Kuva.Accounts.Service.Controllers;
using Kuva.Accounts.Tests.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

#nullable enable
namespace Kuva.Accounts.Tests.Controllers
{
    public class ClientControllerTest : BaseControllerTest
    {
        public ClientControllerTest([FromServices] IMapper mapper)
        {
            _controller = new ClientController(mapper, _clientBusinessMock.Object, _searchUserBusinessMock.Object);
        }

        private readonly Mock<IClientBusiness> _clientBusinessMock = new();
        private readonly Mock<ISearchUserBusiness> _searchUserBusinessMock = new();
        private readonly ClientController _controller;
        

        [Fact]
        public async Task GetUserByIdSuccessfullyTest()
        {
            _searchUserBusinessMock.Setup(s => 
                    s.SearchUserByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(UserFixture.GetRegisteredUsers().First());
            
            var action = await _controller.GetById(1);

            Assert.IsType<OkObjectResult>(action.Result);
            Assert.NotNull(action);
            Assert.NotNull(action.Result);
            var result = action?.Result as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result?.StatusCode);
            Assert.NotNull(result?.Value);
        }
        
        [Fact]
        public async Task GetUserByEmailSuccessfullyTest()
        {
            _searchUserBusinessMock.Reset();
            
            _searchUserBusinessMock.Setup(s => 
                    s.SearchUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(UserFixture.GetUnregisteredUsers().First());

            var action = await _controller.GetByEmail("user@email.com");

            Assert.IsType<OkObjectResult>(action.Result);
            var result = action.Result as OkObjectResult; 

            Assert.Equal(StatusCodes.Status200OK, result?.StatusCode);
            Assert.NotNull(result?.Value);
        }

        [Fact]
        public async Task GetUserByIdNotFoundTest()
        {
            _searchUserBusinessMock.Reset();

            _searchUserBusinessMock.Setup(s => 
                    s.SearchUserByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(default(UserEntity));

            var action = await _controller.GetById(1);

            Assert.IsType<NotFoundResult>(action.Result);
            var result = action.Result as NotFoundResult;
            
            Assert.Equal(StatusCodes.Status404NotFound, result?.StatusCode);
        }

        [Fact]
        public async Task GetUserByEmailBadRequestTest()
        {
            _searchUserBusinessMock.Reset();

            _searchUserBusinessMock.Setup(s =>
                    s.SearchUserByEmailAsync(It.IsAny<string>()))
                .ThrowsAsync(new SearchUserException<string>(AccountsErrors.InvalidUserEmail,
                    It.IsAny<string>()));

            var action = await _controller.GetByEmail(RandomTestValues.RandomValue.String());

            Assert.IsType<BadRequestResult>(action.Result);
            var result = action.Result as BadRequestResult;
            
            Assert.Equal(StatusCodes.Status400BadRequest, result?.StatusCode);
        }

        [Fact]
        public async Task PostRegisterClientTest()
        {
            _clientBusinessMock.Reset();

            _clientBusinessMock.Setup(s => s.Register(It.IsAny<UserEntity>(), 
                    It.IsAny<string>()))
                .ReturnsAsync(UserFixture.GetRegisteredUsers().First());

            var action = await _controller.Register(RegisterClientModelFixture.GetDefaultValues());
            
            Assert.IsType<CreatedResult>(action.Result);
            var result = action.Result as CreatedResult;
            Assert.Equal(StatusCodes.Status201Created, result?.StatusCode);
            Assert.NotNull(result?.Value);
        }

        [Fact]
        public async Task PostRegisterBadRequestTest()
        {
            _clientBusinessMock.Reset();

            _clientBusinessMock.Setup(s => s.Register(It.IsAny<UserEntity>(),
                    It.IsAny<string>()))
                .ThrowsAsync(new NullReferenceException());

            var action = await _controller.Register(RegisterClientModelFixture.GetNullValues());
            
            Assert.IsType<BadRequestObjectResult>(action.Result);
            var result = action.Result as BadRequestObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, result?.StatusCode);
        }

        [Fact]
        public async Task PostRegisterInvalidEmailTest()
        {
            _clientBusinessMock.Reset();

            _clientBusinessMock.Setup(s => s.Register(It.IsAny<UserEntity>(),
                    It.IsAny<string>()))
                .ThrowsAsync(new RegisterException(AccountsErrors.InvalidUserEmail));

            var action = await _controller.Register(RegisterClientModelFixture.GetNullValues());
            
            Assert.IsType<BadRequestObjectResult>(action.Result);
            var result = action.Result as BadRequestObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, result?.StatusCode);
        }

        [Fact]
        public async Task DeleteRegisterAcceptTest()
        {
            _clientBusinessMock.Reset();

            _clientBusinessMock.Setup(s => s.Unregister(It.IsAny<long>()))
                .ReturnsAsync(true);

            var action = await _controller.Unregister(UserFixture.GetRegisteredUsers().First().Id);
            
            Assert.IsType<AcceptedResult>(action);
            var result = (AcceptedResult) action;
            Assert.Equal(StatusCodes.Status202Accepted, result.StatusCode);
        }
        
        [Fact]
        public async Task DeleteRegisterNotFoundTest()
        {
            _clientBusinessMock.Reset();

            _clientBusinessMock.Setup(s => s.Unregister(It.IsAny<long>()))
                .ThrowsAsync(new UnregisterException(AccountsErrors.UserNotFound));

            var action = await _controller.Unregister(UserFixture.GetRegisteredUsers().First().Id);
            
            Assert.IsType<NotFoundResult>(action);
            var result = (NotFoundResult) action;
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }
        
        [Fact]
        public async Task DeleteRegisterConflitTest()
        {
            _clientBusinessMock.Reset();

            _clientBusinessMock.Setup(s => s.Unregister(It.IsAny<long>()))
                .ReturnsAsync(false);

            var action = await _controller.Unregister(UserFixture.GetRegisteredUsers().First().Id);
            
            Assert.IsType<ConflictResult>(action);
            var result = (ConflictResult) action;
            Assert.Equal(StatusCodes.Status409Conflict, result.StatusCode);
        }
    }
}