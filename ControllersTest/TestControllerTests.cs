using EnglishTesterServer.Application.Models;
using EnglishTesterServer.Controllers;
using EnglishTesterServer.DAL.Repositories.Tests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ControllersTest
{
    public class TestControllerTests
    {
        [Fact]
        public async Task GetById_ReturnsHttpNotFound_ForInvalidId()
        {
            // Arrange
            int testId = 123;

            var mockRepo = new Mock<ITestRepository>();

            mockRepo.Setup(repo => repo.GetTestById(testId))
                .Returns((Test)null);

            var controller = new TestController(mockRepo.Object);

            // Act
            var result = controller.OnGetTestById(123);

            // Assert
            Assert.IsType<NotFound>(result);
        }

        [Fact]
        public async Task GetUserTests_Unauthorized()
        {
            // Arrange
            string userEmail = "some.email@gmail.com";

            var mockRepo = new Mock<ITestRepository>();

            mockRepo.Setup(repo => repo.GetUserTests(userEmail))
                .Returns(new Test[]
                {
                    new Test(),
                    new Test()
                });

            var controller = new TestController(mockRepo.Object);

            // Act
            var result = controller.OnGetUserTests(userEmail);

            // Assert
            Assert.IsType<JsonHttpResult<IEnumerable<Test>>>(result);
        }
    }
}