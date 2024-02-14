using EnglishTesterServer.DAL.Models.DTO.Main_Page;
using EnglishTesterServer.DAL.Repositories.Main_Page;
using EnglishTesterServer.DAL.Repositories.Tests;
using EnglishTesterServer.DAL.UnitsOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnglishTesterServer.Controllers
{
    [ApiController]
    public class MainPageController : ControllerBase
    {
        private UnitOfWorkPlatform _unitOfWork = new UnitOfWorkPlatform();
        public MainPageController() { }

        /// <summary>
        /// Getting achievements
        /// </summary>
        /// <returns>Json list of achievements</returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     GET /api/main/achievements
        ///     
        /// </remarks>
        /// <response code="200" link="">Returns achievements</response>
        [HttpGet("api/main/achievements")]
        public IResult OnGetAchievements()
        {
            return Results.Json(_unitOfWork.MainPageRepository.GetAchievements());
        }

        /// <summary>
        /// Getting feedbacks
        /// </summary>
        /// <returns>Json list of feedbacks</returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     GET /api/main/feedbacks
        ///     
        /// </remarks>
        /// <response code="200" link="">Returns feedbacks</response>
        [HttpGet("api/main/feedbacks")]
        public IResult OnGetFeedbacks()
        {
            return Results.Json(_unitOfWork.MainPageRepository.GetFeedbacks());
        }
    }
}
