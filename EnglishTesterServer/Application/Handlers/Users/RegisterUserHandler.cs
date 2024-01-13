using Connection;
using EnglishTesterServer.Application.Commands.Users;
using EnglishTesterServer.Application.Models;
using EnglishTesterServer.Application.Queries.Tests;
using EnglishTesterServer.Application.Queries.User;
using EnglishTesterServer.Application.Validators;
using MediatR;
using System.Data;

namespace EnglishTesterServer.Application.Handlers.Users
{
    public class RegisterUserHandler : IRequestHandler<InsertUserCommand, IResult>
    {

        private readonly Connection.MsSql connection;
        public RegisterUserHandler()
        {
            connection = new MsSql(Program.Settings.ConnectionString);
        }

        public async Task<IResult> Handle(InsertUserCommand request, CancellationToken cancellationToken)
        {
            if (UserValidator.EmailIsValid(request.Credentials.email) && UserValidator.PasswordIsValid(request.Credentials.password))
            {
                DataTable dt = await connection.getDataTable(GetUserQuery.Query, new List<SqlQueryParameter>(){
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
                    string email = dt.Rows[0]["email"].ToString() ?? "None";
                    string password = dt.Rows[0]["password"].ToString() ?? "None";

                    users.Add(new UserCredentials(null, null, email, password));
                }
                if (users.Find(u => u.email == request.Credentials.email) is not null)
                {
                    return await Task.FromResult(Results.Conflict("This email has been registered before"));
                }
                else
                {
                    await connection.executeNonQuery(InsertUserCommand.Command, new List<SqlQueryParameter>()
                    {
                        new SqlQueryParameter()
                        {
                            Parameter = "@firstName",
                            Value = request.Credentials.firstName,
                            Type = SqlDbType.NVarChar
                        },
                        new SqlQueryParameter()
                        {
                            Parameter = "@lastName",
                            Value = request.Credentials.lastName,
                            Type = SqlDbType.NVarChar
                        },
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
                    return await Task.FromResult(Results.Ok());
                }
            }
            return await Task.FromResult(Results.Conflict("Email or password is not valid"));
        }
    }
}
