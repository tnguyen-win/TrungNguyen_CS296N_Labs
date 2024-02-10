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
        readonly IBlogRepository _repository;

        readonly UserManager<AppUser> _userManager;

        public BlogController(IBlogRepository r, UserManager<AppUser> u)
        {
            _repository = r;
            _userManager = u;
        }

        // Message(s)

        public IActionResult Index()
        {
            var messages = _repository.GetMessages();

            return View(messages);
        }

        [HttpPost]
        public IActionResult Index(string author, string date)
        {
            List<Message> messages = (from m in _repository.GetMessages() select m).ToList();

            // Author + Date
            if (author != null & date != null)
            {
                var matchFound = false;

                foreach (var m in _repository.GetMessages()) if (m.From != null && m.From.Name == author) matchFound = true;

                try
                {
                    if (date != null)
                    {
                        DateOnly convertedDate = DateOnly.Parse(date);

                        if (matchFound)
                        {
                            messages = (from m in _repository.GetMessages()
                                        where m.From != null && m.From.Name == author
                                        & m.Date == convertedDate
                                        select m).ToList();
                        }
                        else
                        {
                            messages = (from m in _repository.GetMessages()
                                        where m.Date == convertedDate
                                        select m).ToList();
                        }
                    }
                }
                catch
                {
                    if (matchFound)
                    {
                        messages = (from m in _repository.GetMessages()
                                    where m.From != null && m.From.Name == author
                                    select m).ToList();
                    }
                }
            }

            // Author
            if (author != null & date == null)
            {
                var matchFound = false;

                foreach (var m in _repository.GetMessages()) if (m.From != null && m.From.Name == author) matchFound = true;

                if (matchFound)
                {
                    messages = (from m in _repository.GetMessages()
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

                        messages = (from m in _repository.GetMessages()
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
        public async Task<IActionResult> Post(Message model)
        {
            if (_userManager != null)
            {
                model.From = _userManager.GetUserAsync(User).Result;

                AppUser sender = await _userManager.FindByNameAsync(model.From.Name);

                if (sender != null) model.From = sender;
            }

            Random rnd = new();

            // Fallbacks
            model.Title ??= "Random title";
            model.Body ??= "Lorem ipsum.";
            if (model.From != null) model.From.Name ??= "John Smith";

            // Originals
            model.Date = DateOnly.FromDateTime(DateTime.Now);
            model.Rating = rnd.Next(0, 10);

            if (model.From != null && model.From.Name != "")
            {
                await _repository.StoreMessageAsync(model);

                return RedirectToAction("Index", new { model.MessageId });
            }
            else
            {
                ModelState.AddModelError("", "Sender isn't a registered user.");

                return View(model);
            }
        }
    }
}
