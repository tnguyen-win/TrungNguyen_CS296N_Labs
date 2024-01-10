using PopularGameEngines.Models;

namespace PopularGameEngines.Data {
    public class FakeRegistryRepository : IRegistryRepository {
        readonly List<Message> messages = new();

        public Message GetMessageById(int id) => throw new NotImplementedException();

        public List<Message> GetMessages() => throw new NotImplementedException();

        public int StoreMessage(Message message) {
            message.MessageId = messages.Count + 1;
            messages.Add(message);

            return message.MessageId;
        }
    }
}
