using Newtonsoft.Json;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;

namespace EnglishTesterServer.DAL.Models.Entities
{
    public class TestEntity
    {
        public TestEntity() { }
        public TestEntity(int id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
        public TestEntity(TestEntity test)
        {
            Id = test.Id;
            Title = test.Title;
            Description = test.Description;
            IsFree = test.IsFree;
            Questions = test.Questions;
            Result = test.Result;
        }
        [JsonPropertyName("id")]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        [Column("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        [Column("description")]
        public string Description { get; set; }

        [JsonPropertyName("isFree")]
        [Column("isFree")]
        public bool IsFree { get; set; }

        [JsonPropertyName("questions")]
        public ICollection<QuestionEntity> Questions { get; set; }
        [JsonPropertyName("result")]
        [NotMapped]
        public int Result { get; set; }

        public static async Task<TestEntity> FillFromRow(DataRow row)
        {
            TestEntity filledTest = new TestEntity();
            if (row.Table.Columns.Contains("id"))
            {
                filledTest.Id = Convert.ToInt32(row["id"]);
            }
            if (row.Table.Columns.Contains("title"))
            {
                filledTest.Title = row["title"].ToString()!;
            }
            if (row.Table.Columns.Contains("description"))
            {
                filledTest.Description = row["description"].ToString()!;
            }
            return filledTest;
        }
    }
}
