using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChatApp.Data.Interfaces; 
using ChatApp.Entity;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic; 

namespace ChatApp.Pages.User
{
    [Authorize(Roles = "User")]
    public class UserPageModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMessageRepository _messageRepository;

        public IEnumerable<Entity.User> Contacts { get; set; } = Enumerable.Empty<Entity.User>();

        public IEnumerable<Entity.Announcement> LatestAnnouncements { get; set; } = Enumerable.Empty<Entity.Announcement>(); // YENİ PROPERTY

        public string WelcomeMessage { get; set; } = string.Empty;

        public UserPageModel(IUserRepository userRepository, IAnnouncementRepository announcementRepository, IMessageRepository messageRepository) // YENİ: announcementRepository eklendi
        {
            _userRepository = userRepository;
            _announcementRepository = announcementRepository;
            _messageRepository = messageRepository;
        }



        
        public async Task OnGetAsync()
        {

            Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Append("Pragma", "no-cache");
            Response.Headers.Append("Expires", "0");

            int currentUserId = 0;

            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (currentUserIdClaim != null && int.TryParse(currentUserIdClaim.Value, out currentUserId))
            {
                WelcomeMessage = $"Welcome, {User.Identity!.Name}! This is your chat area.";
            }
            else
            {
                WelcomeMessage = "Welcome to your chat area!";
            }

            var allUsers = await _userRepository.GetAllAsync();

            
            Contacts = allUsers
                .Where(u => u.Id != currentUserId)
                .OrderBy(u => u.Username)
                .ToList();

            LatestAnnouncements = await _announcementRepository.GetLatestAnnouncementsAsync(10);
        }

        
        public async Task<JsonResult> OnGetChatHistoryAsync(int targetId)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (!int.TryParse(currentUserIdClaim?.Value, out int currentUserId))
            {
                return new JsonResult(new { success = false, message = "Yetkilendirme başarısız." }) { StatusCode = 401 };
            }

            if (currentUserId == targetId)
            {
                return new JsonResult(new { success = false, message = "Kendinizle sohbet edemezsiniz." }) { StatusCode = 400 };
            }

            var messages = await _messageRepository.GetConversationAsync(currentUserId, targetId);

            var chatHistory = messages.Select(m => new
            {
                isSender = m.FromId == currentUserId,
                content = m.Context,
                time = m.Date.ToString("HH:mm")
            }).ToList();

            return new JsonResult(new { success = true, messages = chatHistory });
        }
    }
}