using ChatApp.Entity;

namespace ChatApp.Data.Interfaces
{
    public interface IMessageRepository
    {
        
        Task AddAsync(Message message);

        Task<IEnumerable<Message>> GetConversationAsync(int user1Id, int user2Id);

        Task<IEnumerable<Message>> GetLatestMessagesForUserAsync(int userId);

       
    }
}
