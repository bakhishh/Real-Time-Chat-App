using ChatApp.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using UserEntity = ChatApp.Entity.User; 

namespace ChatApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UpdateUserModel : PageModel
    {

        private readonly IUserRepository _userRepository;

        public UpdateUserModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public UserUpdateInput Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            Input = new UserUpdateInput
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Role = user.Role
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var userToUpdate = await _userRepository.GetByIdAsync(Input.Id);
            if (userToUpdate == null) return NotFound();

            if (userToUpdate.Username != Input.Username)
            {
                var allUsers = await _userRepository.GetAllAsync();
                if (allUsers.Any(u => u.Username == Input.Username))
                {
                    ModelState.AddModelError("Input.Username", "username already exists!");
                    return Page();
                }
            }

            userToUpdate.FirstName = Input.FirstName;
            userToUpdate.LastName = Input.LastName;
            userToUpdate.Username = Input.Username;
            userToUpdate.Role = Input.Role;

            await _userRepository.UpdateAsync(userToUpdate);

            return RedirectToPage("./AdminPageModel");
        }

        public class UserUpdateInput
        {

            public int Id { get; set; } 
            [Required] public string FirstName { get; set; } = "";
            [Required] public string LastName { get; set; } = "";
            [Required] public string Username { get; set; } = "";
            [Required] public string Role { get; set; } = "";
        }
    }
}