using PopularGameEngines.Data;
using PopularGameEngines.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PopularGameEngines.Controllers
{
    public class BlogController : Controller
    {
        //readonly AppDbContext context;
        readonly IBlogRepository repository;

        readonly UserManager<AppUser> userManager;

        public BlogController(IBlogRepository r, UserManager<AppUser> u)
        {
            repository = r;
            userManager = u;
        }

        // Message(s)

        public IActionResult Index()
        {
            var messages = repository.GetMessages();

            return View(messages);
        }

        [HttpPost]
        public IActionResult Index(string author, string date)
        {
            List<Message> messages = (from m in repository.GetMessages() select m).ToList();

            // Author + Date
            if (author != null & date != null)
            {
                var matchFound = false;

                foreach (var m in repository.GetMessages()) if (m.From != null && m.From.Name == author) matchFound = true;

                try
                {
                    if (date != null)
                    {
                        DateOnly convertedDate = DateOnly.Parse(date);

                        if (matchFound)
                        {
                            messages = (from m in repository.GetMessages()
                                        where m.From != null && m.From.Name == author
                                        & m.Date == convertedDate
                                        select m).ToList();
                        }
                        else
                        {
                            messages = (from m in repository.GetMessages()
                                        where m.Date == convertedDate
                                        select m).ToList();
                        }
                    }
                }
                catch
                {
                    if (matchFound)
                    {
                        messages = (from m in repository.GetMessages()
                                    where m.From != null && m.From.Name == author
                                    select m).ToList();
                    }
                }
            }

            // Author
            if (author != null & date == null)
            {
                var matchFound = false;

                foreach (var m in repository.GetMessages()) if (m.From != null && m.From.Name == author) matchFound = true;

                if (matchFound)
                {
                    messages = (from m in repository.GetMessages()
                                where m.From != null && m.From.Name == author
                                select m).ToList();
                }
            }

            // Date
            if (author == null & date != null)
            {
                try
                {
                    if (date != null)
                    {
                        DateOnly convertedDate = DateOnly.Parse(date);

                        messages = (from m in repository.GetMessages()
                                    where m.Date == convertedDate
                                    select m).ToList();
                    }
                }
                catch { }
            }

            return View("Index", messages);
        }

        // Message

        [Authorize]
        public IActionResult Post() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Post(Message model)
        {
            Random rnd = new();

            // Fallbacks
            model.Title ??= "Random title";
            model.Body ??= "Lorem ipsum.";
            if (model.From != null) model.From.Name ??= "John Smith";

            // Originals
            model.Date = DateOnly.FromDateTime(DateTime.Now);
            model.Rating = rnd.Next(0, 10);

            if (userManager != null) model.From = userManager.GetUserAsync(User).Result;

            //int result =
            repository.StoreMessage(model);

            return RedirectToAction("Index", new { model.MessageId });
        }
    }
}
