using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace BlackCaviarBank.Infrastructure.Business
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task Send(string message, string userName)
        {
            await Clients.All.SendAsync("Receive", message, userName);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", $"{Context.User.Identity.Name} entered the chat");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("Notify", $"{Context.User.Identity.Name} left the chat");
            await base.OnDisconnectedAsync(exception);
        }

        [Authorize(Roles = "admin")]
        public async Task Notify(string message, string userName)
        {
            await Clients.All.SendAsync("Receive", message, userName);
        }
    }
}
