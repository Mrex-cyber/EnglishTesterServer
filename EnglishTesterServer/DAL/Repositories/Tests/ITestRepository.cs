using EnglishTesterServer.DAL.Models.Entities;
using EnglishTesterServer.DAL.Models.Entities.Relationships;
using static EnglishTesterServer.Controllers.TestController;

namespace EnglishTesterServer.DAL.Repositories.Tests
{
    public interface ITestRepository : ICrud<TestEntity>, IDisposable
    {

        IEnumerable<TestEntity> GetUserTests(string userEmail);
        UserToTest? CheckTestById(string userEmail, int testId, AnswerVariantEntity[] answers);
    }
}
