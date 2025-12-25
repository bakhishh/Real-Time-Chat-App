using ChatApp.Entity;

namespace ChatApp.Data.Interfaces
{
    public interface IAnnouncementRepository
    {
        
        Task AddAsync(Announcement entity);             
        void Remove(Announcement entity);               
        Task<Announcement?> GetByIdAsync(int id);      
        Task SaveAsync();                              

        Task<IEnumerable<Announcement>> GetLatestAnnouncementsAsync(int count = 5);

        Task<Announcement?> GetAnnouncementWithUserAsync(int id);
    }
}
