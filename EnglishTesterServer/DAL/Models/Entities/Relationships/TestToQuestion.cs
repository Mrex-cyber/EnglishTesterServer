using EnglishTesterServer.DAL.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishTesterServer.DAL.Models.Entities.Relationships
{
    public class TestToQuestion
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("questionId")]
        public int QuestionId { get; set; }
        public QuestionEntity Question { get; set; }
        [Column("testId")]
        public int TestId { get; set; }
        public TestEntity Test { get; set; }
    }
}
