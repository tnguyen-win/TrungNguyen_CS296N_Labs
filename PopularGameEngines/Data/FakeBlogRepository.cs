using PopularGameEngines.Models;

namespace PopularGameEngines.Data
{
    public class FakeBlogRepository : IBlogRepository
    {
        readonly List<Message> _messages = new();

        public async Task<Message> GetMessageByIdAsync(int id) => throw new NotImplementedException();

        public List<Message> GetMessages() => throw new NotImplementedException();

        public async Task<int> StoreMessageAsync(Message message)
        {
            message.MessageId = _messages.Count + 1;

            _messages.Add(message);

            return message.MessageId;
        }

        public int UpdateMessage(Message message) => throw new NotImplementedException();

        public int DeleteMessage(int messageId) => throw new NotImplementedException();
    }
}
