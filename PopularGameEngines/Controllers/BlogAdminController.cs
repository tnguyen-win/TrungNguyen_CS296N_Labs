using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopularGameEngines.Data;
using PopularGameEngines.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace PopularGameEngines.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BlogAdminController : Controller
    {
        private readonly AppDbContext _context;

        private readonly IBlogRepository _repository;

        private readonly UserManager<AppUser> _userManager;

        public BlogAdminController(AppDbContext context, IBlogRepository repository, UserManager<AppUser> u)
        {
            _context = context;
            _repository = repository;
            _userManager = u;
        }

        // GET: BlogAdmin
        public async Task<IActionResult> Index() => _context.Messages != null ? View(await _context.Messages.ToListAsync()) : Problem("Entity set 'AppDbContext.Messages' is null.");

        // GET: BlogAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Messages == null) NotFound();

            var message = await _context.Messages.FirstOrDefaultAsync(m => m.MessageId == id);

            if (message == null) return NotFound();

            return View(message);
        }

        // GET: BlogAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Messages == null) return NotFound();

            var message = await _repository.GetMessageByIdAsync(id.Value);

            if (message == null) return NotFound();

            return View(message);
        }

        // POST: BlogAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string fromId, string date, [Bind("MessageId,Title,Body,Date,Rating,OriginalMessageId")] Message message)
        {
            if (id != message.MessageId) return NotFound();

            var from = await _userManager.FindByIdAsync(fromId);

            message.From = from;
            message.Date = DateOnly.Parse(date);

            ModelState.ClearValidationState(nameof(Message.From));

            TryValidateModel(message);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.MessageId)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(message);
        }

        // GET: BlogAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Messages == null) return NotFound();

            var message = await _context.Messages.FirstOrDefaultAsync(m => m.MessageId == id);

            if (message == null) return NotFound();

            return View(message);
        }

        // POST: BlogAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Messages == null) return Problem("Entity set 'AppDbContext.Messages' is null.");

            var message = await _context.Messages.FindAsync(id);

            if (message != null) _context.Messages.Remove(message);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id) => (_context.Messages?.Any(e => e.MessageId == id)).GetValueOrDefault();
    }
}
