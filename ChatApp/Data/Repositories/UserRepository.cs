using ChatApp.Data; // ApplicationDbContext için
using ChatApp.Data.Interfaces;
using ChatApp.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var userToDelete = await _context.Users.FindAsync(id);

            if (userToDelete != null)
            {
                var relatedMessages = _context.Messages
                    .Where(m => m.FromId == id || m.ToId == id);

                _context.Messages.RemoveRange(relatedMessages);

                _context.Users.Remove(userToDelete);

                await _context.SaveChangesAsync();
            }
        }

        

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                                 .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetAllAsync() 
        {
            return await _context.Users.ToListAsync();
        }
    }
}