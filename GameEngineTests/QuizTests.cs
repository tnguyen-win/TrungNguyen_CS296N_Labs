using PopularGameEngines.Controllers;
using PopularGameEngines.Models;

namespace GameEngineTests {
    public class QuizTests {
        [Fact]
        public void TestLoadQuestions() {
            var controller = new QuizController();
            var model = new QuizQuestions();
            var loadedModel = controller.LoadQuestions(model);

            Assert.NotNull(loadedModel.Questions);
            Assert.NotNull(loadedModel.Answers);
            Assert.NotEmpty(loadedModel.Questions);
            Assert.NotEmpty(loadedModel.Answers); Assert.Equal(controller.Questions, loadedModel.Questions);
            Assert.Equal(controller.Answers, loadedModel.Answers);
            Assert.Equal(loadedModel.Questions.Count, loadedModel.Answers.Count);
        }

        [Fact]
        public void TestCheckQuizAnswers() {
            var model = new QuizQuestions();
            var controller = new QuizController();
            var loadedModel = controller.LoadQuestions(model);

            // Check for - right answers
            loadedModel.UserAnswers[1] = "No";
            loadedModel.UserAnswers[2] = "Some";
            loadedModel.UserAnswers[3] = "C++";
            loadedModel.UserAnswers[4] = "Unity";

            var result1 = controller.CheckQuizAnswers(model);

            Assert.True(result1.Results[1]);
            Assert.True(result1.Results[2]);
            Assert.True(result1.Results[3]);
            Assert.True(result1.Results[4]);

            // Check for - wrong answers
            loadedModel.UserAnswers[1] = "Yes";
            loadedModel.UserAnswers[2] = "No";
            loadedModel.UserAnswers[3] = "GDScript";
            loadedModel.UserAnswers[4] = "Godot";

            var result2 = controller.CheckQuizAnswers(model);

            Assert.False(result2.Results[1]);
            Assert.False(result2.Results[2]);
            Assert.False(result2.Results[3]);
            Assert.False(result2.Results[4]);
        }
    }
}
