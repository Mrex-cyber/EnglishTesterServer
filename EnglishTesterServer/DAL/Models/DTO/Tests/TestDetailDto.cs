using EnglishTesterServer.Application.Models;
using EnglishTesterServer.DAL.Models.DTO.Questions;
using Newtonsoft.Json;
using System.Data;

namespace EnglishTesterServer.DAL.Models.DTO.Tests
{
    public class TestDetailDto
    {
        public TestDetailDto() { }
        public TestDetailDto(int id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
        public static implicit operator TestDetailDto(Test test)
        {
            return new TestDetailDto
            {
                Id = test.Id,
                Title = test.Title,
                Description = test.Description,
                Questions = test.Questions.Select(q => (QuestionDto)q).ToArray(),
            };
        }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "questions")]
        public QuestionDto[] Questions { get; set; }
    }
}
