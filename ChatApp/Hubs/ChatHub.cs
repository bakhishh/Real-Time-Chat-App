using ChatApp.Data.Interfaces; 
using ChatApp.Entity; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims; 

namespace ChatApp.Hubs
{
    
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;

        public ChatHub(IMessageRepository messageRepository, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        public async Task SendMessage(int receiverId, string messageContent)
        {
            var senderIdString = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(senderIdString, out int senderId))
            {
                return;
            }

            var message = new Message
            {
                FromId = senderId,
                ToId = receiverId,
                Context = messageContent,
                Date = DateTime.Now
            };

            await _messageRepository.AddAsync(message);

            var senderUsername = Context.User?.Identity?.Name ?? "Admin";

            await Clients.Users(new string[] { senderId.ToString(), receiverId.ToString() })
                         .SendAsync("ReceiveMessage", senderId, senderUsername, messageContent, DateTime.Now.ToString("HH:mm"));
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}