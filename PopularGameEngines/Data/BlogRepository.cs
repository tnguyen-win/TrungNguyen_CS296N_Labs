using PopularGameEngines.Models;
using Microsoft.EntityFrameworkCore;

namespace PopularGameEngines.Data
{
    public class BlogRepository : IBlogRepository
    {
        readonly AppDbContext context;
        public BlogRepository(AppDbContext c) => context = c;

        public Message GetMessageById(int id) => throw new NotImplementedException();

        public List<Message> GetMessages() => context.Messages != null ? context.Messages.Include(m => m.From).ToList() : new List<Message>();

        public int StoreMessage(Message message)
        {
            context.Add(message);

            return context.SaveChanges();
        }
    }
}
