using ChatApp.Data.Interfaces; 
using ChatApp.Data.Repositories;
using ChatApp.Entity; 
using ChatApp.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;


namespace ChatApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminPageModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMessageRepository _messageRepository; 
        private readonly IHubContext<ChatHub> _hubContext;

        [BindProperty]
        public AnnouncementInputModel AnnouncementInput { get; set; } = new AnnouncementInputModel();

        public IEnumerable<Announcement> LatestAnnouncements { get; set; } = Enumerable.Empty<Announcement>();

        public IEnumerable<Entity.User> Users { get; set; } = Enumerable.Empty<Entity.User>();

        public string WelcomeMessage { get; set; } = string.Empty;

        public AdminPageModel(
            IUserRepository userRepository,
            IAnnouncementRepository announcementRepository,
            IMessageRepository messageRepository,
            IHubContext<ChatHub> hubContext) 

        {
            _userRepository = userRepository;
            _announcementRepository = announcementRepository;
            _messageRepository = messageRepository; 
            _hubContext = hubContext;
        }

        public async Task OnGetAsync()
        {
            Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Append("Pragma", "no-cache");
            Response.Headers.Append("Expires", "0");

            await LoadCommonData();
        }


        public async Task<JsonResult> OnGetChatHistoryAsync(int targetId)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (!int.TryParse(currentUserIdClaim?.Value, out int adminId))
            {
                return new JsonResult(new { success = false, message = "Yetkisiz erişim." }) { StatusCode = 401 };
            }

            var messages = await _messageRepository.GetConversationAsync(adminId, targetId);

            var chatHistory = messages.Select(m => new
            {
                isSender = m.FromId == adminId,
                content = m.Context,
                time = m.Date.ToString("HH:mm")
            }).ToList();

            return new JsonResult(new { success = true, messages = chatHistory });
        }


        public async Task<IActionResult> OnPostDeleteUserAsync(int id)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (currentUserIdClaim != null && int.TryParse(currentUserIdClaim.Value, out int currentUserId))
            {
                if (id == currentUserId)
                {
                    return StatusCode(403, new { message = "You cannot delete your own admin account." }); // 403 Forbidden
                }
            }
            else
            {
                return Forbid();
            }

            try
            {
                await _userRepository.DeleteAsync(id);

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user {id}: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while deleting the user and associated messages." });
            }
        }

        public async Task<IActionResult> OnPostPublishAnnouncementAsync()
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (!int.TryParse(currentUserIdClaim?.Value, out int currentUserId))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                await LoadCommonData();
                return Page();
            }

            var newAnnouncement = new Announcement
            {
                Title = AnnouncementInput.Title,
                Content = AnnouncementInput.Content,
                CreatedByUserId = currentUserId,
                Date = DateTime.Now
            };

            await _announcementRepository.AddAsync(newAnnouncement);
            await _announcementRepository.SaveAsync();

            await _hubContext.Clients.All.SendAsync(
                "ReceiveAnnouncement",
                newAnnouncement.Title,
                newAnnouncement.Content,
                newAnnouncement.Date.ToString("dd MMM, HH:mm"),
                newAnnouncement.Id
            );

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAnnouncementAsync(int id)
        {
            var announcementToDelete = await _announcementRepository.GetByIdAsync(id);

            if (announcementToDelete == null)
            {
                return NotFound();
            }

            _announcementRepository.Remove(announcementToDelete);
            await _announcementRepository.SaveAsync();

            await _hubContext.Clients.All.SendAsync(
                "RemoveAnnouncement",
                id 
            );

            return RedirectToPage();
        }


        private async Task LoadCommonData()
        {
            int currentUserId = 0;
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (currentUserIdClaim != null && int.TryParse(currentUserIdClaim.Value, out currentUserId))
            {
                WelcomeMessage = $"Welcome, {User.Identity!.Name}!";
            }
            else
            {
                WelcomeMessage = "Welcome!";
            }

            var allUsers = await _userRepository.GetAllAsync();
            Users = allUsers
                .Where(u => u.Id != currentUserId)
                .OrderBy(u => u.Username)
                .ToList();

            LatestAnnouncements = await _announcementRepository.GetLatestAnnouncementsAsync(10); 

            
        }
    }

    public class AnnouncementInputModel
    {
        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(100, ErrorMessage = "Başlık 100 karakterden uzun olamaz.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik zorunludur.")]
        public string Content { get; set; } = string.Empty;
    }
}