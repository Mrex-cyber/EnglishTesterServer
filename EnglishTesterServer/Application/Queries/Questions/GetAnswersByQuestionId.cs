using EnglishTesterServer.Application.Models;
using MediatR;

namespace EnglishTesterServer.Application.Queries.Questions
{
    public record GetAnswersByQuestionId (int questionId) : IRequest<List<AnswerVariant>>
    {
        public const string query = "exec obj_questions_GetAnswersByQuestionId @questionId";
    }
}
