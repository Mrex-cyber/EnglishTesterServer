using MediatR;

namespace EnglishTesterServer.Application.Queries.Tests
{
    public record GetTestById(int testId) : IRequest<IResult>
    {
        public const string query = "exec obj_tests_GetTestById_with_Questions @id";
    }
}
