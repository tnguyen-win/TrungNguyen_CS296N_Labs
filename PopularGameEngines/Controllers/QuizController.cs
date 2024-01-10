using PopularGameEngines.Models;
using Microsoft.AspNetCore.Mvc;

namespace PopularGameEngines.Controllers {
    public class QuizController : Controller {
        public Dictionary<int, string> Questions { get; set; }

        public Dictionary<int, string> Answers { get; set; }

        public QuizController() {
            Questions = new Dictionary<int, string>();
            Answers = new Dictionary<int, string>();
            Questions[1] = "Are game engines required to create a video game?";
            Answers[1] = "No";
            Questions[2] = "Do all game engines have a editor?";
            Answers[2] = "Some";
            Questions[3] = "What programming language is usually used in video games to help maintain high game performance?";
            Answers[3] = "C++";
            Questions[4] = "What game engine recently had a dramatic debacle? (Mass exodus of users, plummeting of company stocks, credible death threats, etc.)";
            Answers[4] = "Unity";
        }

        public IActionResult Index() {
            var model = LoadQuestions(new QuizQuestions());

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string answer1, string answer2, string answer3, string answer4) {
            var model = LoadQuestions(new QuizQuestions());

            model.UserAnswers[1] = answer1;
            model.UserAnswers[2] = answer2;
            model.UserAnswers[3] = answer3;
            model.UserAnswers[4] = answer4;

            var checkedModel = CheckQuizAnswers(model);

            return View(checkedModel);
        }

        public QuizQuestions LoadQuestions(QuizQuestions model) {
            model.Questions = Questions;
            model.Answers = Answers;
            model.UserAnswers = new Dictionary<int, string>();
            model.Results = new Dictionary<int, bool>();

            foreach (var question in Questions) {
                int key = question.Key;

                model.UserAnswers[key] = "";
            }

            return model;
        }

        public QuizQuestions CheckQuizAnswers(QuizQuestions model) {
            foreach (var question in Questions) {
                int key = question.Key;

                model.Results[key] = model.Answers[key] == model.UserAnswers[key];
            }

            return model;
        }
    }
}
