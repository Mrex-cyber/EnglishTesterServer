using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EnglishTesterServer.DAL.Models.Entities;

namespace EnglishTesterServer.DAL.Models.Entities.Relationships
{
    public class QuestionToAnswer
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("questionId")]
        public int QuestionId { get; set; }
        public QuestionEntity Question { get; set; }
        [Column("answerId")]
        public int AnswerId { get; set; }
        public AnswerVariantEntity Answer { get; set; }
    }
}
