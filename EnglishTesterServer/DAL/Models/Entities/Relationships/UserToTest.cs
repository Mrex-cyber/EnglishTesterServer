using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishTesterServer.DAL.Models.Entities.Relationships
{
    public class UserToTest
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("userId")]
        public int UserId { get; set; }
        [Column("testId")]
        public int TestId { get; set; }
        [Column("finished")]
        public bool Finished { get; set; }
        [Column("accessTime")]
        public DateTime AccessTime { get; set; }
        [Column("finishedTime")]
        public DateTime? FinishedTime { get; set; }
        [Column("result")]
        public int Result { get; set; }
        [Column("status")]
        public bool Status { get; set; }
    }
}
