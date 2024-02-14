using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;

namespace EnglishTesterServer.DAL.Models.Entities
{
    public class RightAnswerVariantEntity
    {
        [JsonPropertyName("id")]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [JsonPropertyName("text")]
        [Column("text")]
        public string? Text { get; set; }

        [JsonPropertyName("questionId")]
        [Column("questionId")]
        [NotMapped]
        public int QuestionId { get; set; }
        [JsonPropertyName("questions")]
        [NotMapped]
        public ICollection<QuestionEntity> Questions { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public bool IsRight { get; set; }

        public static implicit operator RightAnswerVariantEntity(AnswerVariantEntity answerVariant)
        {
            RightAnswerVariantEntity rightAnswerVariant = new RightAnswerVariantEntity();

            rightAnswerVariant.Id = answerVariant.Id;
            rightAnswerVariant.Text = answerVariant.Text;
            rightAnswerVariant.Questions = answerVariant.Questions;
            rightAnswerVariant.QuestionId = answerVariant.QuestionId;
            rightAnswerVariant.IsRight = true;

            return rightAnswerVariant;
        }

        public static async Task<AnswerVariantEntity> FillFromRow(DataRow row)
        {
            AnswerVariantEntity filledAnswer = new AnswerVariantEntity();
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
            return filledAnswer;
        }
    }
}
