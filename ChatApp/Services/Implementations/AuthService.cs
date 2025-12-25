using ChatApp.Data.Interfaces;
using ChatApp.Entity;
using ChatApp.Helpers;
using ChatApp.Services.Interfaces;

namespace ChatApp.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                return null;
            }

            bool isPasswordValid = PasswordHelper.VerifyPassword(password, user.PasswordHash);
            return isPasswordValid ? user : null;
        }

        public async Task<bool> RegisterAsync(string username, string password, string firstName, string lastName)
        {
            if (await _userRepository.IsUsernameTakenAsync(username))
            {
                return false;
            }

            string passwordHash = PasswordHelper.HashPassword(password);

            var newUser = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                Role = "User"
            };

            await _userRepository.AddAsync(newUser);

            return true;
        }
    }
}
