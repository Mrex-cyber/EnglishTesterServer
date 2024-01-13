using EnglishTesterServer.Application.Models;
using MediatR;
using static EnglishTesterServer.Controllers.TestController;

namespace EnglishTesterServer.Application.Queries.Questions
{
    public record GetAllAnswersQuery(AnswersForTheTestCheck answersData) : IRequest<IResult>
    {
    }
}
