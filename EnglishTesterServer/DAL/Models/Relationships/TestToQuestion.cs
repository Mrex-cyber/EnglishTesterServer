using EnglishTesterServer.Application.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishTesterServer.DAL.Models.Relationships
{
    public class TestToQuestion
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("questionId")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        [Column("testId")]
        public int TestId { get; set; }
        public Test Test { get; set; }
    }
}
