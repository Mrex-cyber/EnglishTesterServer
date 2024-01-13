using EnglishTesterServer.Application.Models;
using MediatR;

namespace EnglishTesterServer.Application.Queries.Questions
{
    public record GetRightAnswerByQuestionIdQuery(int questionId) : IRequest<IResult>
    {
        public const string query = "exec obj_questions_GetRightAnswerByQuestionIdQuery @questionId";

    }
}
