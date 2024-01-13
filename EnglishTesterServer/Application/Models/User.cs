using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishTesterServer.Application.Models
{
    public class User
    {
        [Column("id")]
        [Key]
        public int id { get; set; }
        [Required]
        [Column("firstName")]
        public string FirstName { get; set; }
        [Required]
        [Column("lastName")]
        public string LastName { get; set; }
        [Required]
        [Column("email")]
        public string Email { get; set; }
        [Required]
        [Column("password")]
        public string Password { get; set; }
        [NotMapped]
        public List<Test> UserTests { get; set; }
        public User(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }
    }
}
