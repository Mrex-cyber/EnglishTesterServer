using EnglishTesterServer.Application.Models;
using EnglishTesterServer.DAL.Models.Relationships;
using static EnglishTesterServer.Controllers.TestController;

namespace EnglishTesterServer.DAL.Repositories.Tests
{
    public interface ITestRepository : IDisposable
    {
        IEnumerable<Test> GetCommonTests();
        IEnumerable<Test> GetUserTests(string userEmail);
        Test? GetTestById(int testId);
        UserToTest CheckTestById(AnswersForTheTestCheck answers);
        void Save();
    }
}
