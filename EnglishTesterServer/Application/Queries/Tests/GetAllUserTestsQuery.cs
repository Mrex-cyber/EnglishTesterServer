using MediatR;

namespace EnglishTesterServer.Application.Queries.Tests
{
    public record GetAllUserTestsQuery(string userEmail) : IRequest<IResult>
    {
        public const string query = "exec obj_tests_GetAll_byUserEmail @userEmail";
    }
}
