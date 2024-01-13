using EnglishTesterServer.Application.Models;
using MediatR;

namespace EnglishTesterServer.Application.Commands.Users
{
    public class InsertUserCommand : IRequest<IResult>
    {
        public UserCredentials Credentials { get; set; }
        public InsertUserCommand(UserCredentials credentials)
        {
            Credentials = credentials;
        }
        public static string Command
        {
            get
            {
                return "exec obj_users_InsertCredentials @firstName, @lastName, @email, @password";
            }
        }
    }
}
