using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
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
        private readonly IWebHostEnvironment _environment;

        public MessagesController(IMessageService messageService, IWebHostEnvironment environment)
        {
            _messageService = messageService;
            _environment = environment;
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
                        SentAt = messageDto.SentAt,
                        ImageUrl = messageDto.ImageUrl
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

        [HttpGet("GetMessagesBYId/{userId}")]
        public async Task<IActionResult> GetMessagesBYId(int userId)
        {
            try
            {
                var messages = await _messageService.GetMessagesBYId(userId);
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
                TimeSent = m.SentAt,
                ImageUrl = m.ImageUrl
            });

            return Ok(resources);
        }

        [HttpGet("GetSendersByReceiverId/{receiverId}")]
        public async Task<IActionResult> GetSendersByReceiverId(int receiverId)
        {
            try
            {
                var senders = await _messageService.GetSendersByReceiverId(receiverId);
                return Ok(senders);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving senders: {ex.Message}");
                return StatusCode(500, "Failed to retrieve senders. Please try again later.");
            }
        }

        [HttpDelete("DeleteMessage/{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _messageService.GetMessageById(id);
            if (message == null)
            {
                return NotFound("Message not found.");
            }

            var isDelete = await _messageService.DeleteMessage(id);
            if (isDelete)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("UpdateMessage/{id}")]
        public async Task<IActionResult> UpdateMessage(Message message, int id)
        {
            var updatemessage = await _messageService.UpdateMessage(message, id);
            if (updatemessage != null)
            {
                var res = new MessageResource
                {
                    Content = updatemessage.Content,
                };
                return Ok(res);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
