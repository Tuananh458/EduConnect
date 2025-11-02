using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace EduConnect.Hubs
{
    public class NotifyHub : Hub
    {
        // Gửi tin đến user cụ thể
        public async Task SendToUser(string userId, string type, string message)
        {
            await Clients.User(userId).SendAsync("Notify", new { type, message });
        }

        // Gửi tin broadcast
        public async Task Broadcast(string type, string message)
        {
            await Clients.All.SendAsync("Notify", new { type, message });
        }
    }
}
