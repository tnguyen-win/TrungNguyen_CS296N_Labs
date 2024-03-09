using PopularGameEngines.Models;
using Microsoft.EntityFrameworkCore;

namespace PopularGameEngines.Data
{
    public class BlogRepository : IBlogRepository
    {
        readonly AppDbContext _context;

        public BlogRepository(AppDbContext c) => _context = c;

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            _context.Entry(message).Reference(m => m.From).Load();
            _context.Entry(message).Collection(m => m.Replies).Load();

            return message;
        }

        public List<Message> GetMessages()
        {
            return _context.Messages
            .Include(m => m.From)
            .ToList();
        }

        public async Task<int> StoreMessageAsync(Message message)
        {
            await _context.AddAsync(message);

            return _context.SaveChanges();
        }

        public int UpdateMessage(Message message)
        {
            _context.Update(message);

            return _context.SaveChanges();
        }

        public int DeleteMessage(int messageId)
        {
            Message message = GetMessageByIdAsync(messageId).Result;

            if (message.Replies.Count > 0) foreach (var reply in message.Replies) _context.Messages.Remove(reply);

            _context.Messages.Remove(message);

            return _context.SaveChanges();
        }
    }
}
