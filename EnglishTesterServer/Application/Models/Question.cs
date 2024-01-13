using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace EnglishTesterServer.Application.Models
{
    public class Question
    {
        public Question() { }   
        public Question(int id, string text)
        {
            Id = id;
            Text = text;
        }
        [JsonProperty(PropertyName = "id")]
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "text")]
        [Column("text")]
        public string Text { get; set; }
        public ICollection<Test> Tests { get; set; }

        [JsonProperty(PropertyName = "answers")]
        public ICollection<AnswerVariant> Answers { get; set; }
        public static async Task<Question> FillFromRow(DataRow row)
        {
            Question filledTQuestion = new Question();            

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
