namespace EnglishTesterServer.Models
{
    public class Test
    {
        public Test(int id, string title, string description) {
            Id = id; 
            Title = title;
            Description = description;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Question[] Questions { get; set; }
        public bool Finished { get; set; }
        public int Result { get; set; }
    }
}
