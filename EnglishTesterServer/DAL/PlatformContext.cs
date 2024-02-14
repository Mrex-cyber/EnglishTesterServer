using EnglishTesterServer.DAL.Models.Entities;
using EnglishTesterServer.DAL.Models.Entities.Relationships;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace EnglishTesterServer.DAL
{
#pragma warning disable CS1591
    public class PlatformContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<TestEntity> Tests { get; set; } = null!;
        public DbSet<QuestionEntity> Questions { get; set; } = null!;
        public DbSet<AnswerVariantEntity> Answers { get; set; } = null!;
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
            builder.Entity<UserEntity>()
                .ToTable("obj_users");
            builder.Entity<TestEntity>()
                .ToTable("obj_tests");
            builder.Entity<QuestionEntity>()
                .ToTable("obj_questions");
            builder.Entity<AnswerVariantEntity>()
                .ToTable("obj_answers");

            builder.Entity<QuestionToAnswer>()
               .ToTable("rel_question_answer");
            builder.Entity<TestToQuestion>()
                .ToTable("rel_test_question");
            builder.Entity<UserToTest>()
                .ToTable("rel_user_test");

            builder.Entity<TestEntity>()
                .HasMany(t => t.Questions)
                .WithMany(q => q.Tests)
                .UsingEntity<TestToQuestion>();

            builder.Entity<QuestionEntity>()
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
