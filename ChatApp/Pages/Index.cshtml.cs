using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatApp.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Kullanıcıyı doğrudan Login sayfasına yönlendirir.
            return RedirectToPage("/Account/Login");
        }
    }
}