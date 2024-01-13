using EnglishTesterServer.Application.Models;
using MediatR;

namespace EnglishTesterServer.Application.Queries.Tests
{
    public record GetAllTestsQuery(string? userEmail) : IRequest<IResult>
    {
        public const string query = "exec obj_tests_GetAll";
    }
}
