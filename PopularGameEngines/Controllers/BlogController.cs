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
            Random rnd = new();

            // Fallbacks
            model.Title ??= "Random title";
            model.Body ??= "Lorem ipsum.";

            // Defaults
            if (_userManager != null)
            {
                model.From = await _userManager.GetUserAsync(User);

                // AppUser recipient = await _userManager.FindByNameAsync(model.To.Name);

                // if (recipient != null) model.To = recipient;
            }
            model.Date = DateOnly.FromDateTime(DateTime.Now);
            model.Rating = rnd.Next(0, 10);

            await _repository.StoreMessageAsync(model);

            return RedirectToAction("Index", new { model.MessageId });

            // if (model.To.UserName != null)
            // {
            //     await _repository.StoreMessageAsync(model);

            //     return RedirectToAction("Index", new { model.MessageId });
            // }
            // else
            // {
            //     ModelState.AddModelError("", "Recipient isn't a registered user.");

            //     return View(model);
            // }
        }

        [Authorize]
        public IActionResult Reply(int? OriginalMessageId)
        {
            Message message = new()
            {
                OriginalMessageId = OriginalMessageId
            };

            return View(message);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Reply(Message model)
        {
            // Fallbacks
            model.Body ??= "Lorem ipsum.";

            // Defaults
            if (_userManager != null) model.From = await _userManager.GetUserAsync(User);
            Message originalMessage = await _repository.GetMessageByIdAsync(model.OriginalMessageId.Value);
            model.To = originalMessage.From;
            model.Date = DateOnly.FromDateTime(DateTime.Now);

            await _repository.StoreMessageAsync(model);
            originalMessage.Replies.Add(model);
            _repository.UpdateMessage(originalMessage);

            return RedirectToAction("Index", new { model.MessageId });
        }

        [Authorize]
        public IActionResult DeletePost(int messageId)
        {
            _repository.DeleteMessage(messageId);

            return RedirectToAction("Index");
        }
    }
}
