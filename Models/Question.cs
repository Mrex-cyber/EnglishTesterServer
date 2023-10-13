namespace EnglishTesterServer.Models
{
    public class Question
    {
        public Question(int id, string text, string firstVariant, string secondVariant, string thirdVariant,  string fourthVariant, string rightAnswer, int testId) {
            Id = id;
            Text = text;
            FirstVariant = firstVariant;
            SecondVariant = secondVariant;
            ThirdVariant = thirdVariant;
            FourthVariant = fourthVariant;
            RightAnswer = rightAnswer;
            TestId = testId;
        }
        public int Id { get; set; }
        public string Text { get; set; }
        public string FirstVariant { get; set; }
        public string SecondVariant { get; set; }
        public string ThirdVariant { get; set; }
        public string FourthVariant { get; set; }
        public string RightAnswer { get; set; } 
        public int TestId { get; set; }
        public Test? Test { get; set; }
    }
}
