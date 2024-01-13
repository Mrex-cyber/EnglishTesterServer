using EnglishTesterServer.Application.Models;
using MediatR;

namespace EnglishTesterServer.Application.Queries.User
{
    public class GetUserQuery : IRequest<IResult>
    {
        public UserCredentials Credentials { get; set; }
        public GetUserQuery(UserCredentials credentials)
        {
            Credentials = credentials;
        }
        public static string Query { 
            get 
            { 
                return "exec obj_users_GetCredentials @email, @password";
            }
        }
    }
}
