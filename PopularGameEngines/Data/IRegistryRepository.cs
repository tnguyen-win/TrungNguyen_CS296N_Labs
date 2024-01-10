using PopularGameEngines.Models;

namespace PopularGameEngines.Data {
    public interface IRegistryRepository {
        public List<Message> GetMessages();

        public Message GetMessageById(int id);

        public int StoreMessage(Message message);
    }
}
