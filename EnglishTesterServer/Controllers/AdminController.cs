using EnglishTesterServer.DAL.Models.Entities;
using EnglishTesterServer.DAL.UnitsOfWork;
using Microsoft.AspNetCore.Mvc;

namespace EnglishTesterServer.Controllers
{
    public class AdminController : Controller
    {
        private UnitOfWorkPlatform unitOfWork = new UnitOfWorkPlatform();

        public AdminController() { }

        /// <summary>
        /// Adding new test
        /// </summary>
        /// <returns>Result of adding</returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     POST /tests/new
        ///     
        ///     {
        ///         text: string,
        ///         description: string
        ///         answers: [
        ///             {
        ///                 text: string
        ///             }
        ///         ]
        ///     }
        ///     
        /// </remarks>
        /// <response code="200" link="">Added test</response>
        /// <response code="204">If the test not found</response>
        [HttpPost("/api/tests/new")]
        public bool OnPostNewTest([FromBody]TestEntity newTest)
        {
            return unitOfWork.TestRepository.AddEntity(newTest);
        }
    }
}
