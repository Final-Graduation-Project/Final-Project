namespace WebApplication1.Resources
{
    public class MessageResource
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime TimeSent { get; set; }
    }
}
