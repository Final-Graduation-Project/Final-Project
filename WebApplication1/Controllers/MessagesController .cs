using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApplication1.Model;
using WebApplication1.Resources;
using WebApplication1.Services;
using WebApplication1.Table;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        

    [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = new Message
                    {
                        SenderId = messageDto.SenderId,
                        ReceiverId = messageDto.ReceiverId,
                        Content = messageDto.Content,
                        SentAt = messageDto.SentAt
                    };

                    var result = await _messageService.SendMessage(message);
                    return Ok(result);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                return StatusCode(500, "Failed to send message. Please try again later.");
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetMessages(int userId)
        {
            try
            {
                var messages = await _messageService.GetMessagesForUser(userId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving messages: {ex.Message}");
                return StatusCode(500, "Failed to retrieve messages. Please try again later.");
            }
        }
        [HttpGet("{userId1}/{userId2}")]
        public async Task<IActionResult> GetMessagesBetweenUsers(int userId1, int userId2)
        {
            var messages = await _messageService.GetMessagesBetweenUsers(userId1, userId2);

            if (messages == null || !messages.Any())
            {
                return NotFound();
            }

            var resources = messages.Select(m => new MessageResource
            {
                MessageId = m.MessageId,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                TimeSent = m.SentAt
            });

            return Ok(resources);
        }
    }
}

