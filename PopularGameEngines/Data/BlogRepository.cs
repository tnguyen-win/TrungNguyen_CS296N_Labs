using PopularGameEngines.Models;
using Microsoft.EntityFrameworkCore;

namespace PopularGameEngines.Data
{
    public class BlogRepository : IBlogRepository
    {
        readonly AppDbContext _context;

        public BlogRepository(AppDbContext c) => _context = c;

        public async Task<Message> GetMessageByIdAsync(int id) => await _context.Messages.FindAsync(id);

        public List<Message> GetMessages() => _context.Messages != null ? _context.Messages.Include(m => m.From).ToList() : new List<Message>();

        public async Task<int> StoreMessageAsync(Message message)
        {
            await _context.AddAsync(message);

            return _context.SaveChanges();
        }
    }
}
