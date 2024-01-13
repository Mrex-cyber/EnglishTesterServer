using EnglishTesterServer.Application.Models;
using EnglishTesterServer.DAL.Models.DTO.Main_Page;

namespace EnglishTesterServer.DAL.Repositories.Main_Page
{
    public class MainPageRepository : IMainPageRepository, IDisposable
    {
        private PlatformContext _context;
        public MainPageRepository(PlatformContext context)
        {
            _context = context;
        }
        public IEnumerable<AchievementDto> GetAchievements()
        {
            return new AchievementDto[2] { 
                new AchievementDto()
                {
                    Title = "SomeAchTitle1"
                },
                new AchievementDto()
                {
                    Title = "SomeAchTitle2"
                }
            };
        }
        public IEnumerable<FeedbackDto> GetFeedbacks()
        {
            return new FeedbackDto[3] {
                new FeedbackDto()
                {
                    Text = "Text1"
                },
                new FeedbackDto()
                {
                    Text = "Text2"
                },
                new FeedbackDto()
                {
                    Text = "Text3"
                }
            };
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
