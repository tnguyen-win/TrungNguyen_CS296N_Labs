using PopularGameEngines.Models;

namespace PopularGameEngines.Data
{
    public interface IBlogRepository
    {
        public List<Message> GetMessages();

        public Task<Message> GetMessageByIdAsync(int id);

        public Task<int> StoreMessageAsync(Message message);

        public int UpdateMessage(Message message);

        public int DeleteMessage(int messageId);
    }
}
