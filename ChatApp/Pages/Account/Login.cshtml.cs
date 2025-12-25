using ChatApp.Models.ViewModels;
using ChatApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims; // Kimlik bilgileri için gerekli
using Microsoft.AspNetCore.Authentication; // Oturum açma (SignInAsync) için gerekli

namespace ChatApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        // Formdan gelen veriyi yakalamak için
        [BindProperty]
        public LoginViewModel Input { get; set; } = new LoginViewModel();

        // IAuthService'i Dependency Injection (DI) ile alıyoruz
        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
            // Kullanıcı zaten giriş yapmışsa ana sayfaya yönlendirilebilir.
            if (User.Identity!.IsAuthenticated)
            {
                RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // ... (Model doğrulama ve AuthService ile kullanıcıyı çekme kodları burada kalacak) ...

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _authService.LoginAsync(Input.Username, Input.Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }

            // ... (Claims oluşturma ve HttpContext.SignInAsync kodları burada kalacak) ...
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            var authProperties = new AuthenticationProperties { IsPersistent = false };
            await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);


            // YÖNLENDİRME MANTIĞI BURASI
            if (user.Role == "Admin")
            {
                // Admin rolündeyse /Admin/Page sayfasına yönlendir
                return RedirectToPage("/Admin/AdminPageModel");
            }
            else if (user.Role == "User")
            {
                // User rolündeyse /User/Page sayfasına yönlendir
                return RedirectToPage("/User/UserPageModel");
            }
            else
            {
                // Tanımlanmamış roller için varsayılan bir sayfaya yönlendir
                return RedirectToPage("/Index");
            }
        }
    }
}