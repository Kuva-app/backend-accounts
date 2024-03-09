using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using Kuva.Accounts.Business;
using Kuva.Accounts.Business.Exceptions;
using Kuva.Accounts.Business.Interfaces;
using Kuva.Accounts.Service.Controllers.Filters;
using Kuva.Accounts.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

#nullable enable
namespace Kuva.Accounts.Service.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/v1/accounts/[Controller]")]
    public class PasswordController : BaseController
    {
        public PasswordController(IRequestChangePasswordBusiness requestChangePassword,
            IChangePasswordBusiness changePasswordBusiness)
        {
            _requestChangePassword = requestChangePassword;
            _changePasswordBusiness = changePasswordBusiness;
        }

        private readonly IRequestChangePasswordBusiness _requestChangePassword;
        private readonly IChangePasswordBusiness _changePasswordBusiness;

        [HttpPost("request"), Validate, AuthorizeApiToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RequestChangePassword([FromQuery(Name = "mail"),
                                                                Required(ErrorMessage = "Necessário informar o e-mail"),
                                                                EmailAddress(ErrorMessage =
                                                                    "Necessário informar um e-mail válido")]
            string email,
            [FromHeader(Name = Constants.HeaderTokenKey), Required] string? xApiToken = null)
        {
            try
            {
                await _requestChangePassword.RequestByUserEmailAsync(email);
                return Ok();
            }
            catch (RequestChangePasswordException e)
            {
                var result = new ProblemDetails
                {
                    Detail = e.Message,
                    Title = e.ErrorCode
                };

                return e.AccountsError switch
                {
                    AccountsErrors.RequestChangePasswordTokenAlreadyRegistered => Conflict(result),
                    AccountsErrors.UserNotFound => NotFound(),
                    _ => Problem()
                };
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpPut("{user_id}/change"), Validate, AuthorizeApiToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromRoute(Name = "user_id"), Required]
            long userId,
            [FromBody] ChangePassowordModel changePassword,
            [FromHeader(Name = Constants.HeaderTokenKey), Required] string? xApiToken = null)
        {
            try
            {
                var changed = await _changePasswordBusiness.ChangePasswordByUserIdAsync(userId,
                    changePassword.ConfirmationCode, changePassword.NewPassword);

                return !changed ? StatusCode(StatusCodes.Status304NotModified) : Ok();
            }
            catch (ChangePasswordException e)
            {
                var result = new ProblemDetails
                {
                    Title = e.ErrorCode,
                    Detail = e.Message
                };

                return e.AccountsError switch
                {
                    AccountsErrors.UserNotFound => NotFound(),
                    AccountsErrors.PasswordToBig => BadRequest(result),
                    AccountsErrors.RequestChangePasswordTokenNotRegistered => NotFound(),
                    _ => Problem()
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Problem();
            }
        }

        [HttpGet, Validate, AuthorizeApiToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByToken([FromQuery(Name = "t"), Required] string hash)
        {
            try
            {
                var userToken = await _requestChangePassword.GetUserByTokenHashAsync(hash);
                return Ok(userToken);
            }
            catch (RequestChangePasswordException e)
            {
                var result = new ProblemDetails
                {
                    Detail = e.Message,
                    Title = e.ErrorCode
                };
                return e.AccountsError switch
                {
                    AccountsErrors.UserNotFound => NotFound(),
                    AccountsErrors.RequestChangePasswordTokenExpired => Conflict(result),
                    AccountsErrors.RequestChangePasswordTokenNotRegistered => NotFound(),
                    _ => Problem()
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Problem();
            }
        }
    }
}