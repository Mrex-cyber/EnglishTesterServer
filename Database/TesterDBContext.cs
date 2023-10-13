using EnglishTesterServer.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishTesterServer.Database
{
    public class TesterDBContext : DbContext
    {
        public DbSet<Test> Tests { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public TesterDBContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TesterDatabase;Trusted_Connection=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Test[] Tests = {
                new Test(1, "English test", "Test your English knowledge"),
                new Test(2, "C# Basic", "Do you know C# language on Basic level?"),
                new Test(3, "Another test", "What do You think about this Application?")
            };


            Question[] QuestionsOne = {
                new Question(1, "What is correct?", "Engand", "Egland", "Anglend", "England", "England", 1),
                new Question(2, "Check _ next computer.", "a", "the", "_", "-1", "_", 1),
                new Question(3, "Be interested in _", "you", "yourself", "myself", "your", "yourself", 1),
                new Question(4, "Choose correct sentence", "What is that like?", "What is like that?", "What like that is?", "It is like what?", "What is that like?", 1),
                new Question(5, "'There was an empty room' | Do you see _ there?", "anyone", "someone", "no one", "none", "anyone", 1),
                new Question(6, "This bag is _", "Johns", "John's", "Johns's", "Johned", "John's", 1),
            };

            Question[] QuestionsTwo = {
                new Question(7, "What type of programming languages is C#?", "Object-oriented", "Functional", "Declarative programming", "Imperative programming", "Object-oriented", 2),
                new Question(8, "Correct is: ", "string person = \"Tom\";", "person = \"Tom\";", "string \"Tom\";", "let person = \"Tom\";", "Boring", 2),
            };

            Question[] QuestionsThree = {
                new Question(9, "Is it amazing?", "Yes", "Of course", "No", "Maybe", "Of course", 3),
                new Question(10, "What about you? How are You?", "Fine", "Bad", "Worried", "Best of the best", "Best of the best", 3),
            };
            
            List<Question> questionsList = new List<Question>();
            questionsList.AddRange(QuestionsOne);
            questionsList.AddRange(QuestionsTwo);
            questionsList.AddRange(QuestionsThree);
            modelBuilder.Entity<Question>().HasData(questionsList);
            modelBuilder.Entity<Test>().HasData(Tests);

            modelBuilder.Entity<Question>().HasOne<Test>().WithMany().HasForeignKey(q => q.TestId);
        }
    }
}
