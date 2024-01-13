using EnglishTesterServer.DAL.Models.DTO.Main_Page;
using EnglishTesterServer.DAL.Repositories.Main_Page;
using EnglishTesterServer.DAL.Repositories.Tests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishTesterServer.Controllers
{
    [ApiController]
    public class MainPageController : ControllerBase
    {
        private IMainPageRepository _pageRepository;
        public MainPageController(IMainPageRepository pageRepository)
        {
            this._pageRepository = pageRepository;
        }

        [HttpGet("api/main/achievements")]
        public IResult OnGetAchievements()
        {
            return Results.Json(_pageRepository.GetAchievements());
        }

        [HttpGet("api/main/feedbacks")]
        public IResult OnGetFeedbacks()
        {
            return Results.Json(_pageRepository.GetFeedbacks());
        }
    }
}
