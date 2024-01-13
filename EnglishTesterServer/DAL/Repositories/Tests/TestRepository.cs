using EnglishTesterServer.Application.Models;
using EnglishTesterServer.DAL.Models.Relationships;
using Microsoft.EntityFrameworkCore;
using static EnglishTesterServer.Controllers.TestController;

namespace EnglishTesterServer.DAL.Repositories.Tests
{
    public class TestRepository : ITestRepository, IDisposable
    {
        private PlatformContext _context;
        public TestRepository(PlatformContext context)
        {
            _context = context;
        }
        public IEnumerable<Test> GetCommonTests()
        {
            var tests = _context.Tests
                .Where(t => t.isFree)
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Answers)
                .ToList();

            foreach (var test in tests)
            {
                foreach (var question in test.Questions)
                {
                    foreach (var answer in question.Answers)
                    {
                        answer.Questions.Clear();
                    }
                    question.Tests.Clear();
                }
            }


            return tests;
        }
        public IEnumerable<Test> GetUserTests(string userEmail)
        {
            return from test in _context.Tests
                   join relUserToTest in _context.RelUserToTest
                       on test.Id equals relUserToTest.TestId
                   join user in _context.Users
                       on relUserToTest.UserId equals user.id
                   join relTestToQuestion in _context.RelTestToQuestion
                       on test.Id equals relTestToQuestion.TestId
                   join questions in _context.Questions
                       on relTestToQuestion.QuestionId equals questions.Id
                   where user.Email == userEmail && !test.isFree
                   select test;
        }

        public Test? GetTestById(int testId)
        {
            return _context.Tests.Find(testId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public UserToTest CheckTestById(AnswersForTheTestCheck answers)
        {
            var userToTestList = from relUserToTest in _context.RelUserToTest
                                 join test in _context.Tests
                                      on relUserToTest.TestId equals test.Id
                                 join user in _context.Users
                                      on relUserToTest.UserId equals user.id
                                 where test.Id == answers.testId && user.Email == answers.userEmail
                                 select relUserToTest;
            if (userToTestList is null || userToTestList.Count() == 0)
            {
                throw new ArgumentNullException(nameof(userToTestList));
            }

            UserToTest userTest = userToTestList.First();

            int result = 0;

            List<AnswerVariant> rightAnswers = new List<AnswerVariant>();

            for (int i = 0; i < answers.answers.Length; i++)
            {
                var variant = from answer in _context.Answers
                              where answer.QuestionId == answers.answers[i].QuestionId
                              select answer;

                if (variant is not null)
                {
                    rightAnswers.Add(variant.First());
                }
            }

            foreach (AnswerVariant rightAnswer in rightAnswers)
            {
                if (answers.answers.Where(a => a.QuestionId == rightAnswer.QuestionId && a.Id == rightAnswer.Id).Count() > 0)
                {
                    result++;
                }
            }

            userTest.Result = result;
            Save();

            return userTest;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
