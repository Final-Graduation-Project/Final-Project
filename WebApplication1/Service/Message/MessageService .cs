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
        Task<List<Message>> GetMessagesBYId(int userId);
        Task<IEnumerable<Message>> GetMessagesBetweenUsers(int userId1, int userId2);
        Task<List<int>> GetSendersByReceiverId(int receiverId);

        Task<bool> DeleteMessage(int id);

        Task<Message> UpdateMessage(Message message,int id);
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
                // تحقق من إذا كانت الرسالة تحتوي على صورة
                if (string.IsNullOrEmpty(message.ImageUrl))
                {
                    message.ImageUrl = ""; // قم بتعيين قيمة فارغة لرابط الصورة
                }

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



        public async Task<List<Message>> GetMessagesBYId(int userId)
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
    
    public async Task<List<int>> GetSendersByReceiverId(int receiverId)
    {
        try
        {
            var senders = await _context.Messages
                .Where(m => m.ReceiverId == receiverId)
                .Select(m => m.SenderId)
                .Distinct()
                .ToListAsync();
            return senders;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            throw new Exception("Failed to retrieve senders. Please try again later.");
        }
    }
        public async Task<bool> DeleteMessage(int id)
        {
            var messagedel= await _context.Messages.FindAsync(id);
            if (messagedel == null)
            {
                return false;
            }
            _context.Messages.Remove(messagedel);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<Message> UpdateMessage(Message message,int id)
        {
            var update = await _context.Messages.FindAsync(id);
            if (update == null)
            {
                return null;
            }
            update.Content=message.Content;
            await _context.SaveChangesAsync();
            return update;
        }
        }

}
