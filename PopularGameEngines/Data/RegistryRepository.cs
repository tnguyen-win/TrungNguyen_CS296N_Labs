using PopularGameEngines.Models;
using Microsoft.EntityFrameworkCore;

namespace PopularGameEngines.Data {
    public class RegistryRepository : IRegistryRepository {
        readonly AppDbContext context;
        public RegistryRepository(AppDbContext c) => context = c;

        public Message GetMessageById(int id) => throw new NotImplementedException();

        public List<Message> GetMessages() {
            return context.Messages

            .Include(m => m.From)
            .ToList();
        }

        public int StoreMessage(Message message) {
            context.Add(message);

            return context.SaveChanges();
        }
    }
}
