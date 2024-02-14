using EnglishTesterServer.DAL.Models.DTO.Main_Page;

namespace EnglishTesterServer.DAL.Repositories.Main_Page
{
    public interface IMainPageRepository : IDisposable
    {
        IEnumerable<AchievementDto> GetAchievements();
        IEnumerable<FeedbackDto> GetFeedbacks();
    }
}