using System;

namespace WebApplication1.Table
{
    public class Message
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; } // Add this property
        public DateTime SentAt { get; set; }
    }
}
