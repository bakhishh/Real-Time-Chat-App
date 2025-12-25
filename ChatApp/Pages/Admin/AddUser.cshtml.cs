using ChatApp.Data.Interfaces;
using ChatApp.Entity ;
using ChatApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AddUserModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public AddUserModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public UserInputModel Input { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var allUsers = await _userRepository.GetAllAsync();
            if (allUsers.Any(u => u.Username == Input.Username))
            {
                ModelState.AddModelError("Input.Username", "username already exists!");
                return Page();
            }

            var newUser = new Entity.User
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Username = Input.Username,
                PasswordHash = PasswordHelper.HashPassword(Input.Password), 
                Role = Input.Role
            };

            await _userRepository.AddAsync(newUser);

            return RedirectToPage("./AdminPageModel"); 
        }

        public class UserInputModel
        {
            [Required(ErrorMessage = "First Name is required.")]
            public string FirstName { get; set; } = "";

            [Required(ErrorMessage = "Lat Name is required.")]
            public string LastName { get; set; } = "";

            [Required(ErrorMessage = "Username is required.")]
            public string Username { get; set; } = "";

            [Required(ErrorMessage = "Password is required.")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "";

            [Required]
            public string Role { get; set; } = "User";
        }
    }
}