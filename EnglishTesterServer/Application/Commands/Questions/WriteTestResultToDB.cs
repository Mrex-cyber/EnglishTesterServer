using EnglishTesterServer.Application.Models;
using MediatR;
using static EnglishTesterServer.Controllers.TestController;

namespace EnglishTesterServer.Application.Commands.Questions
{
    public record WriteTestResultToDB (string userEmail, int testId, int result) : IRequest<IResult>
    {
        public const string command = "exec rel_user_test_WriteResult @userEmail, @testId, @result";
    }
}
