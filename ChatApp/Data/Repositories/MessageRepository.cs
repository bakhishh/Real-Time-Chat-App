using ChatApp.Data.Interfaces;
using ChatApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        

        public async Task AddAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync(); 
        }


        public async Task<IEnumerable<Message>> GetConversationAsync(int user1Id, int user2Id)
        {

            return await _context.Messages
                .Where(m =>
                    (m.FromId == user1Id && m.ToId == user2Id) ||
                    (m.FromId == user2Id && m.ToId == user1Id)
                )
                .OrderBy(m => m.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetLatestMessagesForUserAsync(int userId)
        {

            var participantIds = await _context.Messages
                .Where(m => m.FromId == userId || m.ToId == userId)
                .Select(m => m.FromId == userId ? m.ToId : m.FromId)
                .Distinct()
                .ToListAsync();

            var latestMessages = new List<Message>();

            foreach (var participantId in participantIds)
            {
                var latestMessage = await _context.Messages
                    .Where(m =>
                        (m.FromId == userId && m.ToId == participantId) ||
                        (m.FromId == participantId && m.ToId == userId)
                    )
                    .OrderByDescending(m => m.Date) 
                    .FirstOrDefaultAsync();

                if (latestMessage != null)
                {
                    latestMessages.Add(latestMessage);
                }
            }

            return latestMessages.OrderByDescending(m => m.Date);
        }

        
    }
}
