namespace EnglishTesterServer.Application.Queries.Questions
{
    public record GetQuestionsByTestId(int? testId)
    {
        public const string query = "exec obj_questions_GetByTestId @testId";
    }
}
