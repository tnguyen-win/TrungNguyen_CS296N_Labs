namespace PopularGameEngines.Models
{
    public class QuizQuestions
    {
        public Dictionary<int, string> Questions { get; set; } = null!;

        public Dictionary<int, string> Answers { get; set; } = null!;

        public Dictionary<int, string> UserAnswers { get; set; } = null!;

        public Dictionary<int, bool> Results { get; set; } = null!;
    }
}
