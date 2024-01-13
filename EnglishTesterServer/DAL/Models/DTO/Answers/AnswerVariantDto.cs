using EnglishTesterServer.Application.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace EnglishTesterServer.DAL.Models.DTO.Answers
{
    public class AnswerVariantDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }


        [JsonProperty(PropertyName = "text")]
        public string? Text { get; set; }


        [JsonProperty(PropertyName = "questionId")]
        [NotMapped]
        public int QuestionId { get; set; }
        public AnswerVariantDto() { }
        public static implicit operator AnswerVariantDto(AnswerVariant answer)
        {
            return new AnswerVariantDto
            {
                Id = answer.Id,
                Text = answer.Text ?? "Error text!"
            };
        }
    }
}
