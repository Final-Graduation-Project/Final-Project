using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Resorces;
using WebApplication1.Services.Event;
using WebApplication1.Table;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventControllercs : Controller
    {
        private readonly IEventServer _eventServer;

        public EventControllercs(IEventServer eventServer)
        {
            _eventServer = eventServer;
        }

        [HttpPost("AddEvent")]
        public async Task<IActionResult> AddEvent(EventAddEntitycs m)
        {
            var res = await _eventServer.AddEvent(m);
            var resource = new EventResource
            {
                Name = res.ActivityName,
                EventId = res.ActivityID,
                Location = res.LocationOfActivity,
                ExecutionTime = res.ActivityExecutionTime,
                Time = res.Time, // Ensure this is DateTime
                ResponsibleActivity = res.EntityResponsibleActivity,
                StudentID = res.ConcilMemberID,
            };
            return Ok(resource);
        }

        [HttpGet("GetAllEvent")]
        public async Task<IActionResult> GetAllEvent()
        {
            var res = await _eventServer.GetAllEvent();
            List<Event> model = new List<Event>();
            foreach (var item in res)
            {
                var detale = new Event
                {
                    ActivityID = item.ActivityID,
                    ActivityName = item.ActivityName,
                    LocationOfActivity = item.LocationOfActivity,
                    ActivityExecutionTime = item.ActivityExecutionTime,
                    Time = item.Time,
                    EntityResponsibleActivity = item.EntityResponsibleActivity,
                    ImagePath = item.ImagePath,
                    ConcilMemberID = item.ConcilMemberID,
                    
                };
                model.Add(detale);

            }
            return Ok(model);
        }

        [HttpGet("GetEvent")]
        public async Task<IActionResult> GetEvent(string EntityResponsibleActivity)
        {

            var res = await _eventServer.GetEvent(EntityResponsibleActivity);
           
                        return Ok(res);

        }

        [HttpDelete("DeleteEvent/{eventId}")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var isDeleted = await _eventServer.DeleteEvent(eventId);

            if (isDeleted)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent([FromBody] EventAddEntitycs updateEvent, int id)
        {
            var updatedEvent = await _eventServer.UpdateEvent(updateEvent, id);
            if (updatedEvent != null)
            {
                var res = new EventResource
                {
                    Name = updatedEvent.ActivityName,
                    EventId = updatedEvent.ActivityID,
                    Location = updatedEvent.LocationOfActivity,
                    ExecutionTime = updatedEvent.ActivityExecutionTime,
                    Time = updatedEvent.Time, // Ensure this is DateTime
                    ResponsibleActivity = updatedEvent.EntityResponsibleActivity,
                    StudentID = updatedEvent.ConcilMemberID,
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
