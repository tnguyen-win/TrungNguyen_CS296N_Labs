namespace PopularGameEngines.Models {
    public class QuizQuestions {
        public Dictionary<int, string> Questions { get; set; }

        public Dictionary<int, string> Answers { get; set; }

        public Dictionary<int, string> UserAnswers { get; set; }

        public Dictionary<int, bool> Results { get; set; }
    }
}
