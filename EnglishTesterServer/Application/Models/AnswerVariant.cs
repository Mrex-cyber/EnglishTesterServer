using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace EnglishTesterServer.Application.Models
{
    public class AnswerVariant
    {

        [JsonProperty(PropertyName = "id")]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        [Column("text")]
        public string? Text { get; set; }

        
        [JsonProperty(PropertyName = "questionId")]
        [Column("questionId")]
        [NotMapped]
        public int QuestionId { get; set; }
        [NotMapped]
        public ICollection<Question> Questions { get; set; }

        [JsonProperty(PropertyName = "isRight")]
        [Column("isRight")]
        public bool IsRight { get; set; }

        public static async Task<AnswerVariant> FillFromRow(DataRow row)
        {
            AnswerVariant filledAnswer = new AnswerVariant();
            if (row.Table.Columns.Contains("id"))
            {
                filledAnswer.Id = Convert.ToInt32(row["id"]);
            }
            if (row.Table.Columns.Contains("text"))
            {
                filledAnswer.Text = row["text"].ToString()!;
            }
            if (row.Table.Columns.Contains("questionId"))
            {
                filledAnswer.QuestionId = Convert.ToInt32(row["questionId"]);
            }
            if (row.Table.Columns.Contains("isRight"))
            {
                filledAnswer.IsRight = Convert.ToBoolean(row["isRight"]);
            }
            return filledAnswer;
        }
    }
}
