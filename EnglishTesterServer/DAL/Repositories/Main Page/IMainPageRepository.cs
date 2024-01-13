using EnglishTesterServer.Application.Models;
using EnglishTesterServer.DAL.Models.DTO.Main_Page;
using EnglishTesterServer.DAL.Models.Relationships;
using static EnglishTesterServer.Controllers.TestController;

namespace EnglishTesterServer.DAL.Repositories.Main_Page
{
    public interface IMainPageRepository : IDisposable
    {
        IEnumerable<AchievementDto> GetAchievements();
        IEnumerable<FeedbackDto> GetFeedbacks();
    }
}