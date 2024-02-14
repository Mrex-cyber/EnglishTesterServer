using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;

namespace EnglishTesterServer.DAL.Models.Entities
{
    public class QuestionEntity
    {
        public QuestionEntity() { }
        public QuestionEntity(int id, string text)
        {
            Id = id;
            Text = text;
        }
        [JsonPropertyName("id")]
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [JsonPropertyName("text")]
        [Column("text")]
        public string Text { get; set; }
        [JsonPropertyName("tests")]
        public ICollection<TestEntity> Tests { get; set; }

        [JsonPropertyName("answers")]
        public ICollection<AnswerVariantEntity> Answers { get; set; }
        public static async Task<QuestionEntity> FillFromRow(DataRow row)
        {
            QuestionEntity filledTQuestion = new QuestionEntity();

            if (row.Table.Columns.Contains("id"))
            {
                filledTQuestion.Id = Convert.ToInt32(row["id"]);
            }
            if (row.Table.Columns.Contains("text"))
            {
                filledTQuestion.Text = row["text"].ToString()!;
            }

            return filledTQuestion;
        }
    }
}
