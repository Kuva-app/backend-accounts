using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Kuva.Accounts.Business;
using Kuva.Accounts.Business.Exceptions;
using Kuva.Accounts.Business.Interfaces;
using Kuva.Accounts.Entities;
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
    public class ClientController : BaseController
    {

        public ClientController([FromServices] IMapper mapper,
            [FromServices] IClientBusiness clientBusiness,
            [FromServices] ISearchUserBusiness searchUserBusiness)
        {
            _mapper = mapper;
            _clientBusiness = clientBusiness;
            _searchUserBusiness = searchUserBusiness;
        }

        private readonly IMapper _mapper;
        private readonly ISearchUserBusiness _searchUserBusiness;
        private readonly IClientBusiness _clientBusiness;

        [HttpGet("{client_id}"), AuthorizeApiToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientModel?>> GetById([FromRoute(Name = "client_id")] long clientId,
            [FromHeader(Name = Constants.HeaderTokenKey), Required] string? xApiToken = null)
        {
            try
            {
                var user = await _searchUserBusiness.SearchUserByIdAsync(clientId);
                if (user == null) return NotFound();
                var result = _mapper.Map<ClientModel>(user);
                return Ok(result);
            }
            catch (AccountsBusinessException e)
            {
                return Problem(e.Message, nameof(ClientController),
                    StatusCodes.Status500InternalServerError, e.Message,
                    nameof(GetById));
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpGet, Validate, AuthorizeApiToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientModel?>> GetByEmail([FromQuery(Name = "mail"),
                                                                  Required(AllowEmptyStrings = false,
                                                                      ErrorMessage = "É necessário informar o e-mail"),
                                                                  EmailAddress(ErrorMessage =
                                                                      "Formato de e-mail inválido")]
            string email, 
            [FromHeader(Name = Constants.HeaderTokenKey), Required] string? xApiToken = null)
        {
            try
            {
                // _logger.Write(LogEventLevel.Information, "Get client by e-mail");
                var user = await _searchUserBusiness.SearchUserByEmailAsync(email);
                if (user == null) return NotFound();
                var result = _mapper.Map<ClientModel>(user);
                return Ok(result);
            }
            catch (AccountsBusinessException e)
            {
                if (e is SearchUserException<string>)
                    return BadRequest();

                return Problem(e.Message, nameof(ClientController),
                    StatusCodes.Status500InternalServerError, e.Message,
                    nameof(GetById));
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpPost, Validate, AuthorizeApiToken]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ClientModel?>> Register([FromBody] RegisterClientModel registerClientModel,
            [FromHeader(Name = Constants.HeaderTokenKey), Required] string? xApiToken = null)
        {
            try
            {
                var newClient = _mapper.Map<UserEntity>(registerClientModel);
                var registeredClient = await _clientBusiness.Register(newClient, registerClientModel.Password);
                if (registeredClient == null) return Problem();
                var client = _mapper.Map<ClientModel>(registeredClient);
                return Created("", client);
            }
            catch (NullReferenceException e)
            {
                var result = new ProblemDetails()
                {
                    Detail = e.Message
                };
                return BadRequest(result);
            }
            catch (RegisterException e)
            {
                var result = new ProblemDetails()
                {
                    Detail = e.Message,
                    Title = e.ErrorCode
                };

                if (e.AccountsError == AccountsErrors.InvalidUserEmail)
                    return BadRequest(result);

                return e.AccountsError == AccountsErrors.EmailAlreadyExists ? Conflict(result) : Problem();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Problem();
            }
        }

        [HttpDelete("{client_id}"), Validate, AuthorizeApiToken]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Unregister([FromRoute(Name = "client_id")] long clientId, 
            [FromHeader(Name = Constants.HeaderTokenKey), Required] string? xApiToken = null)
        {
            try
            {
                if (await _clientBusiness.Unregister(clientId))
                    return Accepted();
                return Conflict();
            }
            catch (UnregisterException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}