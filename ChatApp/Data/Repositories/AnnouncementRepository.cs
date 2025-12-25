using ChatApp.Data.Interfaces;
using ChatApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AnnouncementRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }


        public async Task AddAsync(Announcement entity)
        {
            await _dbContext.Announcements.AddAsync(entity);
        }

        public void Remove(Announcement entity)
        {
            _dbContext.Announcements.Remove(entity);
        }

        public async Task<Announcement?> GetByIdAsync(int id)
        {
            return await _dbContext.Announcements.FindAsync(id);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }


        public async Task<IEnumerable<Announcement>> GetLatestAnnouncementsAsync(int count = 5)
        {
            return await _dbContext.Announcements
                                   .Include(a => a.CreatedByUser) 
                                   .OrderByDescending(a => a.Date)
                                   .Take(count)
                                   .ToListAsync();
        }

        public async Task<Announcement?> GetAnnouncementWithUserAsync(int id)
        {
            return await _dbContext.Announcements
                                   .Include(a => a.CreatedByUser) 
                                   .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
