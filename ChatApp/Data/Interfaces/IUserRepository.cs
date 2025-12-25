using ChatApp.Entity;
using System.Collections.Generic; // IEnumerable için
using System.Threading.Tasks;

namespace ChatApp.Data.Interfaces
{
    public interface IUserRepository
    {
       
        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(int id);

        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByUsernameAsync(string username);

        Task<bool> IsUsernameTakenAsync(string username);

        Task<IEnumerable<User>> GetAllAsync(); 
    }
}