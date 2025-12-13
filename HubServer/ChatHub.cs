using ChatContracts;
using Microsoft.AspNetCore.SignalR;

namespace HubServer
{
    public class ChatHub : Hub<IChatClient>
    {
        private static List<ConnectedUser> _conntectedUsers = new List<ConnectedUser>();
        private static readonly object _lockUsers = new object();
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"];
            var userName = Context.GetHttpContext()?.Request.Query["userName"];
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
            {
                await Clients.Caller.ReceiveSystemMessage("Bağlatı Hatası.");
                Context.Abort();
                return;
            }
            lock (_lockUsers)
            {
                _conntectedUsers.Add(new ConnectedUser
                {
                    UserId = userId ?? string.Empty,
                    UserName = userName ?? string.Empty,
                    ConnectionId = Context.ConnectionId
                });
            }

            await Clients.Caller.ReceiveSystemMessage("Sohbet odasına hoşgeldiniz!");
            await Clients.All.UpdateUserList(_conntectedUsers);

        }
        override public async Task OnDisconnectedAsync(Exception? exception)
        {

            ConnectedUser? user;
            lock (_lockUsers)
            {
                user = _conntectedUsers.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
                if (user != null)
                {
                    _conntectedUsers.Remove(user);
                }
            }
            if (user != null)
            {
                await Clients.Others.ReceiveSystemMessage($"{user?.UserName} kullanıcı ayrıldı");
                await Clients.All.UpdateUserList(_conntectedUsers);
            }
            await base.OnDisconnectedAsync(exception);
        }
        public async Task ForwardMessage(string fromUserId, string toConnectionId, string message)
        {
            await Clients.Client(toConnectionId).ReceiveMessage(fromUserId,Context.ConnectionId,message);
        }
    }  
}
