using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Model;
using WebApplication1.Table;

namespace WebApplication1.Services
{
    public interface IMessageService
    {
        Task<Message> SendMessage(Message message);
        Task<List<Message>> GetMessagesForUser(int userId);
        Task<IEnumerable<Message>> GetMessagesBetweenUsers(int userId1, int userId2);

    }

    public class MessageService : IMessageService
    {
        private readonly AppDbContext _context;

        public MessageService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Message>> GetMessagesBetweenUsers(int userId1, int userId2)
        {
            return await _context.Messages
                .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                            (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<Message> SendMessage(Message message)
        {
            try
            {
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
                return message;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"DbUpdateException: {dbEx.Message}");
                if (dbEx.InnerException != null)
                {
                    Console.WriteLine($"InnerException: {dbEx.InnerException.Message}");
                }
                throw new Exception("Failed to send message due to a database update issue. Please try again later.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw new Exception("Failed to send message due to an unexpected error. Please try again later.");
            }
        }


        public async Task<List<Message>> GetMessagesForUser(int userId)
        {
            try
            {
                var messages = await _context.Messages
                    .Where(m => m.ReceiverId == userId)
                    .ToListAsync();
                return messages;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw new Exception("Failed to retrieve messages. Please try again later.");
            }
        }
    }
}
