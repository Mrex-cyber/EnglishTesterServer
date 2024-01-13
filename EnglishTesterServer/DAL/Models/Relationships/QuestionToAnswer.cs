using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EnglishTesterServer.Application.Models;

namespace EnglishTesterServer.DAL.Models.Relationships
{
    public class QuestionToAnswer
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("questionId")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        [Column("answerId")]
        public int AnswerId { get; set; }
        public AnswerVariant Answer { get; set; }
    }
}
