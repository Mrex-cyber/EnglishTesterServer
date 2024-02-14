using EnglishTesterServer.DAL.Models.DTO.Answers;
using EnglishTesterServer.DAL.Models.Entities;
using Newtonsoft.Json;
using System.Data;

namespace EnglishTesterServer.DAL.Models.DTO.Questions
{
    public class QuestionDto
    {
        public QuestionDto() { }
        public QuestionDto(int id, string text)
        {
            Id = id;
            Text = text;
        }
        public static implicit operator QuestionDto(QuestionEntity question)
        {
            return new QuestionDto
            {
                Id = question.Id,
                Text = question.Text,
                Answers = question.Answers.Select(a => (AnswerVariantDto)a).ToArray(),
            };
        }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "test")]
        public Tests.TestDto? Tests { get; set; }
        [JsonProperty(PropertyName = "answers")]
        public AnswerVariantDto[]? Answers { get; set; }
    }
}
