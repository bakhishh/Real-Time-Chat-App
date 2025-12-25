using ChatApp.Entity;

namespace ChatApp.Services.Interfaces
{
    public interface IAuthService
    {
        
        Task<User?> LoginAsync(string username, string password);

        Task<bool> RegisterAsync(string username, string password, string firstName, string lastName);
    }
}
