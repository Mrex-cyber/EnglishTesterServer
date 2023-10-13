using EnglishTesterServer.Database;
using EnglishTesterServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static EnglishTesterServer.Controllers.UserController;

namespace EnglishTesterServer.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        private readonly TesterDBContext db = new TesterDBContext();

        [HttpGet("/api/tests")]
        public IResult OnGetTests()
        {
            List<Test> tests = db.Tests.ToList();

            foreach(var test in tests)
            {
                test.Questions = db.Questions.Where(q => q.TestId == test.Id).ToArray();
            }
            return Results.Json(tests);
        }
        [HttpPost("/api/tests/{id}")]
        public async Task<IResult> OnPostTest(int id)
        {
            var json = String.Empty;
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                json = await reader.ReadToEndAsync();
            }
            TestCredentials gotTest = JsonSerializer.Deserialize<TestCredentials>(json)!;
            Test? foundTest = db.Tests.Where(test => test.Id == id).FirstOrDefault();
            if (foundTest is null) return Results.NotFound();
            foundTest.Result = gotTest.result;
            foundTest.Finished = true;
            db.SaveChanges();
            return Results.Ok();
        }
        public record TestCredentials(int result);
    }
}
