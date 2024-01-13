using EnglishTesterServer.Application.Models;
using EnglishTesterServer.Application.Validators;
using EnglishTesterServer.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scrutor;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using Connection;
using EnglishTesterServer.Application.Commands;
using System.Data;
using EnglishTesterServer.Application.Queries.User;

namespace EnglishTesterServer.Application.Handlers.Users
{
    public class AuthorizeUserHandler : IRequestHandler<GetUserQuery, IResult>
    {
        private readonly Connection.MsSql connection;
        public AuthorizeUserHandler()
        {
            connection = new MsSql(Program.Settings.ConnectionString);
        }

        public async Task<IResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            if (request is null) return Results.BadRequest();

            if (UserValidator.EmailIsValid(request.Credentials.email) && UserValidator.PasswordIsValid(request.Credentials.password))
            {
                DataTable dt = await connection.getDataTable(GetUserQuery.Query, new List<SqlQueryParameter>()
                {
                    new SqlQueryParameter()
                    {
                        Parameter = "@email",
                        Value = request.Credentials.email,
                        Type = SqlDbType.NVarChar
                    },
                    new SqlQueryParameter()
                    {
                        Parameter = "@password",
                        Value = request.Credentials.password,
                        Type = SqlDbType.NVarChar
                    }
                });
                List<UserCredentials> users = new List<UserCredentials>();

                foreach (var row in dt.Rows)
                {
                    users.Add(new UserCredentials(null, null, dt.Rows[0]["email"].ToString()!, dt.Rows[0]["password"].ToString()!));
                }
                if (users.Exists(u => u.email == request.Credentials.email && u.password == request.Credentials.password))
                {
                    var userData = new SendUserData(AuthToken.MakeToken(request.Credentials.email), request.Credentials.email);
                    return await Task.FromResult(Results.Json(userData));
                }

                return await Task.FromResult(Results.NotFound());

            }
            return await Task.FromResult(Results.Conflict("Email or password is not valid"));
        }
        public record SendUserData(string token, string email);
    }
}
