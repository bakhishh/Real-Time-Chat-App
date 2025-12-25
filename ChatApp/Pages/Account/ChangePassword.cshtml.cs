using ChatApp.Data.Interfaces;
using ChatApp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ChatApp.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public ChangePasswordModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public ChangePasswordInput Input { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out int userId)) return Forbid();

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return NotFound();

            if (!PasswordHelper.VerifyPassword(Input.CurrentPassword, user.PasswordHash))
            {
                ModelState.AddModelError("Input.CurrentPassword", "Current Password is incorrect.");
                return Page();
            }

            user.PasswordHash = PasswordHelper.HashPassword(Input.NewPassword);

            await _userRepository.UpdateAsync(user);

            TempData["SuccessMessage"] = "Your Password is changed.";
            return RedirectToPage(user.Role == "Admin" ? "/Admin/AdminPageModel" : "/User/UserPageModel");
        }

        public class ChangePasswordInput
        {
            [Required(ErrorMessage = "Current Password is required.")]
            [DataType(DataType.Password)]
            public string CurrentPassword { get; set; } = "";

            [Required(ErrorMessage = "New Password is required.")]
            [DataType(DataType.Password)]
            public string NewPassword { get; set; } = "";

            [Required(ErrorMessage = "New Password is required")]
            [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; } = "";
        }
    }
}