using EnglishTesterServer.Application.Commands.Questions;
using EnglishTesterServer.Application.Handlers.Questions;
using EnglishTesterServer.Application.Models;
using EnglishTesterServer.Application.Queries.Questions;
using EnglishTesterServer.Application.Queries.Tests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static EnglishTesterServer.Controllers.UserController;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using EnglishTesterServer.DAL.Repositories.Tests;

namespace EnglishTesterServer.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        private ITestRepository _testRepository;

        //public TestController()
        //{
        //    this._testRepository = new TestRepository(new PlatformContext());
        //}
        public TestController(ITestRepository testRepository)
        {
            this._testRepository = testRepository;
        }

        /// <summary>
        /// Getting all free tests
        /// </summary>
        /// <returns>Json list of tests with questions and answers (without right answer)</returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     GET /api/tests
        ///     
        /// </remarks>
        /// <response code="200" link="">Returns tests</response>
        /// <response code="204">If the test list is empty</response>
        [HttpGet("/api/tests")]
        [AllowAnonymous]
        public IResult OnGetTests()
        {
            var commonTests = _testRepository.GetCommonTests();

            if (commonTests.Count() == 0)
            {
                return Results.NoContent();
            }

            return Results.Json(commonTests, JsonSerializerOptions.Default, "application/json", 200);
        }
        /// <summary>
        /// Getting tests that are allowed to user with this email
        /// </summary>
        /// <returns>Json list of tests with questions and answers (without right answer)</returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     POST /api/tests
        ///     {
        ///         "someemail@gmail.com"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200" link="">Returns tests for some user</response>
        /// <response code="204">If the test list is empty</response>
        [HttpPost("/api/tests")]
        public IResult OnGetUserTests([FromBody] string userEmail)
        {
            var userTests = _testRepository.GetUserTests(userEmail);

            if (userTests.Count() == 0)
            {
                return Results.NoContent();
            }

            return Results.Json(userTests);
        }

        /// <summary>
        /// Getting test by ID
        /// </summary>
        /// <returns>Test with questions and answers (without right answer)</returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     GET /api/tests/{testId}
        ///     
        /// </remarks>
        /// <response code="200" link="">Returns test</response>
        /// <response code="204">If the test not found</response>
        [HttpGet("/api/tests/{testId}")]
        public IResult OnGetTestById(int testId)
        {
            var test = _testRepository.GetTestById(testId);

            if (test is null)
            {
                return Results.NoContent();
            }

            return Results.Json(test);
        }

        /// <summary>
        /// Checking the test
        /// </summary>
        /// <returns>Result (in points) for test</returns>
        /// <remarks>
        /// Sample request: 
        /// 
        ///     POST /api/tests/check
        ///     {
        ///         "userEmail": "someemail@gmail.com",
        ///         "testId": 1,
        ///         "answers": [
        ///             {
        ///                 "id": 1,
        ///                 "text": "How are you?",
        ///                 "questionId": 2
        ///             }
        ///         ]
        ///     }
        ///     
        /// </remarks>
        /// <response code="200" link="">Returns result (number)</response>
        [AllowAnonymous]
        [HttpPost("/api/tests/check")]
        public IResult OnPostCheckTestByid([FromBody] AnswersForTheTestCheck testData)
        {
            int result = _testRepository.CheckTestById(testData).Result;
            _testRepository.Save();

            return Results.Json(result);
        }
        public record AnswersForTheTestCheck(string userEmail, int testId, AnswerVariant[] answers);
        protected override void Dispose(bool disposing)
        {
            _testRepository.Dispose();
            base.Dispose(disposing);
        }

        //private readonly IMediator _mediator;
        //public TestController(IMediator mediator)
        //{
        //    _mediator = mediator;
        //}

        //[HttpGet("/api/tests")]
        //[AllowAnonymous]
        //public async Task<IResult> OnGetTests()
        //{
        //    GetAllTestsQuery query = new GetAllTestsQuery(null);

        //    return await _mediator.Send(query);
        //}
        //[HttpPost("/api/tests")]
        //public async Task<IResult> OnGetUserTests([FromBody] string userEmail)
        //{
        //    GetAllTestsQuery query = new GetAllTestsQuery(userEmail);

        //    return await _mediator.Send(query);
        //}


        //[HttpGet("/api/tests/{testId}")]
        //public async Task<IResult> OnGetTestById(int testId)
        //{
        //    GetTestById query = new GetTestById(testId);

        //    return await _mediator.Send(query);
        //}
        //[AllowAnonymous]
        //[HttpPost("/api/tests/check")]
        //public async Task<IResult> OnPostCheckTestByid([FromBody] AnswersForTheTestCheck testData)
        //{
        //    GetAllAnswersQuery command = new GetAllAnswersQuery(testData);

        //    return await _mediator.Send(command);
        //}
        //public record AnswersForTheTestCheck(string userEmail, int testId, AnswerVariant[] answers);
    }
}
