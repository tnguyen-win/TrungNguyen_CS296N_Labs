﻿using PopularGameEngines.Models;
using Microsoft.AspNetCore.Mvc;

namespace PopularGameEngines.Controllers
{
    public class QuizController : Controller
    {
        public Dictionary<int, string> Questions { get; set; }

        public Dictionary<int, string> Answers { get; set; }

        public QuizController()
        {
            Questions = new Dictionary<int, string>();
            Answers = new Dictionary<int, string>();
            Questions[1] = "Are game engines required to create a video game?";
            Answers[1] = "no";
            Questions[2] = "Do all game engines have a editor?";
            Answers[2] = "some";
            Questions[3] = "What programming language is usually used in video games to help maintain high game performance?";
            Answers[3] = "c++";
            Questions[4] = "What game engine recently had a dramatic debacle? (Fired CEO, mass exodus of users, plummeting of company stocks, credible death threats, etc.)";
            Answers[4] = "unity";
        }

        public IActionResult Index()
        {
            var model = LoadQuestions(new QuizVM());

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string answer1, string answer2, string answer3, string answer4)
        {
            var model = LoadQuestions(new QuizVM());

            model.UserAnswers[1] = answer1;
            model.UserAnswers[2] = answer2;
            model.UserAnswers[3] = answer3;
            model.UserAnswers[4] = answer4;

            var checkedModel = CheckQuizAnswers(model);

            return View(checkedModel);
        }

        public QuizVM LoadQuestions(QuizVM model)
        {
            model.Questions = Questions;
            model.Answers = Answers;
            model.UserAnswers = new Dictionary<int, string>();
            model.Results = new Dictionary<int, bool>();

            foreach (var question in Questions)
            {
                int key = question.Key;

                model.UserAnswers[key] = "";
            }

            return model;
        }

        public QuizVM CheckQuizAnswers(QuizVM model)
        {
            foreach (var question in Questions)
            {
                int key = question.Key;

                model.Results[key] = model.Answers[key] == (model.UserAnswers[key] == null ? model.UserAnswers[key] : model.UserAnswers[key].ToLower());
            }

            return model;
        }
    }
}
