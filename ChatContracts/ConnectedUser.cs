namespace ChatContracts
{
    public class ConnectedUser
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string ConnectionId { get; set; } = string.Empty;
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

    }
}
