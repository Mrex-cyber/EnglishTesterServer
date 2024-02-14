using EnglishTesterServer.DAL.Models.Entities;
using EnglishTesterServer.DAL.Models.Entities.Relationships;
using Microsoft.EntityFrameworkCore;

namespace EnglishTesterServer.DAL.Repositories.Tests
{
    public class TestRepository : ITestRepository, IDisposable
    {
        private PlatformContext _context;
        public TestRepository(PlatformContext context)
        {
            _context = context;
        }

        public IEnumerable<TestEntity> GetEntities()
        {

            var tests = _context.Tests
                .Where(t => t.IsFree)
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Answers)
                .ToList();

            foreach (var test in tests)
            {
                foreach (var question in test.Questions)
                {
                    foreach (var answer in question.Answers)
                    {
                        answer.QuestionId = question.Id;
                        answer.IsRight = false;
                        answer.Questions.Clear();
                    }
                    question.Tests.Clear();
                }
            }


            return tests;
        }

        public IEnumerable<TestEntity> GetUserTests(string userEmail)
        {
            List<TestEntity> allTests = GetEntities().ToList();

            var testIds = from rel in _context.RelUserToTest
                                   join user in _context.Users
                                        on rel.UserId equals user.id
                                   where user.Email == userEmail
                                   select rel.TestId;

            var userTests = _context.Tests
                .Where(t => t.IsFree && testIds.Contains(t.Id))
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Answers)
                .ToList();           

            foreach (var test in userTests)
            {
                foreach (var question in test.Questions)
                {
                    foreach (var answer in question.Answers)
                    {
                        answer.QuestionId = question.Id;
                        answer.IsRight = false;
                        answer.Questions.Clear();
                    }
                    question.Tests.Clear();
                }
            }

            allTests.AddRange(userTests);

            return allTests;
        }

        public bool AddEntity(TestEntity newTest)
        {
            try
            {
                _context.Add(newTest);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool UpdatedEntity(TestEntity test)
        {
            return true;
        }
        public bool RemoveEntity(int id)
        {
            return true;
        }
        public TestEntity? GetEntityById(int id)
        {
            return _context.Tests.Find(id);
        }
        public UserToTest? CheckTestById(string userEmail, int testId, AnswerVariantEntity[] userAnswers)
        {
            RightAnswerVariantEntity[] rightAnswers = GetAllRightAnswersForQuestions(userAnswers);

            int result = CalculatePoints(userAnswers, rightAnswers);

            return GetResultAndSave(userEmail, testId, result);
        }

        private RightAnswerVariantEntity[] GetAllRightAnswersForQuestions(AnswerVariantEntity[] userAnswers)
        {
            RightAnswerVariantEntity[] rightAnswers = new RightAnswerVariantEntity[userAnswers.Length];

            for (int i = 0; i < userAnswers.Length; i++)
            {
                RightAnswerVariantEntity currentAnswer = userAnswers[i];

                var variant = from answer in _context.Answers
                              join questionToAnswer in _context.RelQuestionToAnswer
                                on answer.Id equals questionToAnswer.AnswerId
                              join questions in _context.Questions
                                on questionToAnswer.QuestionId equals questions.Id
                              where answer.IsRight == true
                                && questions.Id == currentAnswer.QuestionId
                              select new RightAnswerVariantEntity()
                              {
                                  Id = answer.Id,
                                  Text = answer.Text,
                                  QuestionId = currentAnswer.QuestionId
                              };

                if (variant is not null)
                {
                    rightAnswers[i] = variant.First();
                }
            }

            return rightAnswers;
        }        

        private UserToTest GetResultAndSave(string userEmail, int testId, int result)
        {
            try
            {
                var userToTestField = from userToTest in _context.RelUserToTest
                                      join users in _context.Users
                                         on userToTest.UserId equals users.id
                                      where userToTest.TestId == testId
                                         && users.Email == userEmail
                                      select userToTest;

                UserToTest resultObject = userToTestField.First();

                if (WriteResultToDB(ref resultObject, result))
                {
                    return resultObject;
                }
                else throw new Exception("Writing to DB was canceled");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private int CalculatePoints(AnswerVariantEntity[] userAnswers, RightAnswerVariantEntity[] rightAnswers)
        {
            int result = 0;

            foreach (RightAnswerVariantEntity rightAnswer in rightAnswers)
            {
                if (userAnswers.Where(a => a.QuestionId == rightAnswer.QuestionId && a.Id == rightAnswer.Id).Count() > 0)
                {
                    result++;
                }
            }

            return result;
        }

        private bool WriteResultToDB(ref UserToTest resultObject, int result)
        {
            try
            {
                resultObject.Result = result;
                Save();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
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
