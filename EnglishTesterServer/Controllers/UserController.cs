using EnglishTesterServer.Application.Commands.Users;
using EnglishTesterServer.Application.Handlers;
using EnglishTesterServer.Application.Models;
using EnglishTesterServer.Application.Queries.User;
using EnglishTesterServer.Application.Validators;
using EnglishTesterServer.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
namespace EnglishTesterServer.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/api/user/signin")]
        public async Task<IResult> SignIn([FromBody]UserCredentials credentials)
        {
            GetUserQuery query = new GetUserQuery(credentials);
            
            return await _mediator.Send(query);
        }
        [HttpPost("/api/user/signup")]
        public async Task<IResult> SignUp([FromBody] UserCredentials credentials)
        {
            InsertUserCommand command = new InsertUserCommand(credentials);

            return await _mediator.Send(command);
        }

    }
}
