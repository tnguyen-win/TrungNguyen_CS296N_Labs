using PopularGameEngines.Data;
using PopularGameEngines.Models;
using Microsoft.AspNetCore.Mvc;
//using PopularGameEngines.Data;
//using System.Numerics;

namespace PopularGameEngines.Controllers {
    public class BlogController : Controller {
        //readonly AppDbContext context;
        readonly IRegistryRepository repository;

        public BlogController(IRegistryRepository r) => repository = r;

        // Message(s)

        public IActionResult Index() {
            var messages = repository.GetMessages();

            return View(messages);
        }

        [HttpPost]
        public IActionResult Index(string author, string date) {
            List<Message> messages = (from m in repository.GetMessages() select m).ToList();

            // Author + Date
            if (author != null & date != null) {
                var matchFound = false;

                foreach (var m in repository.GetMessages()) if (m.From.Name == author) matchFound = true;

                try {
                    DateOnly convertedDate = DateOnly.Parse(date);

                    if (matchFound) {
                        messages = (from m in repository.GetMessages()
                                    where m.From.Name == author
                                    & m.Date == convertedDate
                                    select m).ToList();
                    } else {
                        messages = (from m in repository.GetMessages()
                                    where m.Date == convertedDate
                                    select m).ToList();
                    }
                } catch {
                    if (matchFound) {
                        messages = (from m in repository.GetMessages()
                                    where m.From.Name == author
                                    select m).ToList();
                    }
                }
            }

            // Author
            if (author != null & date == null) {
                var matchFound = false;

                foreach (var m in repository.GetMessages()) if (m.From.Name == author) matchFound = true;

                if (matchFound) {
                    messages = (from m in repository.GetMessages()
                                where m.From.Name == author
                                select m).ToList();
                }
            }

            // Date
            if (author == null & date != null) {
                try {
                    DateOnly convertedDate = DateOnly.Parse(date);

                    messages = (from m in repository.GetMessages()
                                where m.Date == convertedDate
                                select m).ToList();
                } catch { }
            }

            return View("Index", messages);
        }

        // Message

        public IActionResult Post() => View();

        [HttpPost]
        public IActionResult Post(Message model) {
            Random rnd = new();

            // Fallbacks
            model.Title ??= "Random title";
            model.Body ??= "Lorem ipsum.";
            model.From.Name ??= "John Smith";

            // Originals
            model.Date = DateOnly.FromDateTime(DateTime.Now);
            model.Rating = rnd.Next(0, 10);

            //int result =
            repository.StoreMessage(model);

            return RedirectToAction("Index", new { model.MessageId });
        }
    }
}
