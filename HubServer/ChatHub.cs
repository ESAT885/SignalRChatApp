using Microsoft.AspNetCore.SignalR;

namespace HubServer
{
    public class ChatHub:Hub
    {
       
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"];
            var userName = Context.GetHttpContext()?.Request.Query["userName"];
           
            await Clients.Caller.SendAsync("ReceiveSystemMessage", "Welcome to the chat room!");
           
        }
    }
}
