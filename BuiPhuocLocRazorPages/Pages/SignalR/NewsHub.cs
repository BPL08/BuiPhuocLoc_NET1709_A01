using Microsoft.AspNetCore.SignalR;

namespace BuiPhuocLocRazorPages.Pages.SignalR
{
    public class NewsHub : Hub
    {
        public async Task SendNewsUpdate(string message)
        {
            // Notify all connected clients about the news update
            await Clients.All.SendAsync("ReceiveNewsUpdate", message);
        }

        // Optional: Method to refresh the news list for all connected clients
        public async Task LoadNews()
        {
            // Notify all clients to reload the news list or perform other actions
            await Clients.All.SendAsync("LoadNews");
        }
    }
}
