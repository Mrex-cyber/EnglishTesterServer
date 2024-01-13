using EnglishTesterServer.Application.Models;
using EnglishTesterServer.DAL.Models.Relationships;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace EnglishTesterServer.DAL
{
#pragma warning disable CS1591
    public class PlatformContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Test> Tests { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<AnswerVariant> Answers { get; set; } = null!;
        public DbSet<QuestionToAnswer> RelQuestionToAnswer { get; set; } = null!;
        public DbSet<TestToQuestion> RelTestToQuestion { get; set; } = null!;
        public DbSet<UserToTest> RelUserToTest { get; set; } = null!;


        public PlatformContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TesterDatabase;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .ToTable("obj_users");
            builder.Entity<Test>()
                .ToTable("obj_tests");
            builder.Entity<Question>()
                .ToTable("obj_questions");
            builder.Entity<AnswerVariant>()
                .ToTable("obj_answers");

            builder.Entity<QuestionToAnswer>()
               .ToTable("rel_question_answer");
            builder.Entity<TestToQuestion>()
                .ToTable("rel_test_question");
            builder.Entity<UserToTest>()
                .ToTable("rel_user_test");

            builder.Entity<Test>()
                .HasMany(t => t.Questions)
                .WithMany(q => q.Tests)
                .UsingEntity<TestToQuestion>();

            builder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithMany(a => a.Questions)
                .UsingEntity<QuestionToAnswer>();

            builder.Entity<TestToQuestion>()
                .HasKey(key => new { key.TestId, key.QuestionId });

            builder.Entity<QuestionToAnswer>()
                .HasKey(key => new { key.QuestionId, key.AnswerId });
        }
    }
#pragma warning restore CS1591

}
