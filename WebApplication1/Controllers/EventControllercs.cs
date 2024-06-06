using Microsoft.AspNetCore.Mvc;
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
                time = res.time,
                ResponsibleActivity = res.EntityResponsibleActivity,
                StudentID = res.ConcilMemberID,
            };
            return Ok(resource);
        }


        [HttpGet("GitAllEvent")]
        public async Task<IActionResult> GitAllEvent()
        {
            var res = await _eventServer.GitAllEvent();
            return Ok(res);
        }

        [HttpGet("GitEvent")]
        public async Task<IActionResult> GitEvent(string name_of_the_committee)
        {
            var res = await _eventServer.GitEvent(name_of_the_committee);
            return Ok(res);
        }
    
    [HttpDelete("DeleteEvent/{eventId}")]
    public async Task<IActionResult> DeleteEvent(int eventId)
    {
        var isDeleted = await _eventServer.DeleteEvent(eventId);

        if (isDeleted)
        {
            return NoContent(); 
        }
        else
        {
            return NotFound(); 
        }
    }
        [HttpPut("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent( [FromBody] EventAddEntitycs updateEvent,int id)
        {
            var updateevent = await _eventServer.UpdateEvent( updateEvent,id);
            if (updateevent != null)
            {
                var res = new EventResource
                {
                    Name = updateevent.ActivityName,
                    EventId = updateevent.ActivityID,
                    Location = updateevent.LocationOfActivity,
                    ExecutionTime = updateevent.ActivityExecutionTime,
                    time = updateevent.time,
                    ResponsibleActivity = updateevent.EntityResponsibleActivity,
                    StudentID = updateevent.ConcilMemberID,

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


