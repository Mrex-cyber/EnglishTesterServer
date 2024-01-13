using Newtonsoft.Json;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace EnglishTesterServer.Application.Models
{
    public class Test
    {
        public Test() {}
        public Test(int id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
        public Test(Test test)
        {
            Id = test.Id;
            Title = test.Title;
            Description = test.Description;
            isFree = test.isFree;
        }
        [JsonProperty(PropertyName = "id")]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        [Column("title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        [Column("description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "isFree")]
        [Column("isFree")]
        public bool isFree {  get; set; }

        [JsonProperty(PropertyName = "questions")]
        public ICollection<Question> Questions { get; set; }

        public static async Task<Test> FillFromRow(DataRow row)
        {
            Test filledTest = new Test();
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
