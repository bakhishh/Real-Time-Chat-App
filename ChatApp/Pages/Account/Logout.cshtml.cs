using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication; // Oturum kapatma için gerekli
using System.Threading.Tasks;

namespace ChatApp.Pages.Account
{
    // Bu sayfa herhangi bir yetkilendirme gerektirmez
    public class LogoutModel : PageModel
    {
        // Kullan?c?y? bilgilendirmek için bir GET metodu b?rak?labilir.
        // Ancak as?l i?lem POST'ta yap?lmal?d?r.
        public void OnGet() {
            Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Append("Pragma", "no-cache");
            Response.Headers.Append("Expires", "0");

        }

        // Güvenlik: Oturumu kapatma i?lemini sadece POST iste?i geldi?inde yap
        public async Task<IActionResult> OnPostAsync()
        {
            // Oturum çerezini siler ve kullan?c?y? sistemden ç?kar?r.
            await HttpContext.SignOutAsync("CookieAuth");

            // Kullan?c?y? Login sayfas?na yönlendir.
            return RedirectToPage("/Account/Login");
        }
    }
}