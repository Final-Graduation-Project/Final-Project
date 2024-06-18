using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Table;
using WebApplication1.Model;


namespace WebApplication1.Services.Event
{
    public interface IEventServer
    {
        Task<EventEntity> AddEvent(EventAddEntitycs m);
        Task<List<EventEntity>> GetAllEvent();
        Task<EventEntity> GetEvent(string EntityResponsibleActivity);
        Task<EventEntity> UpdateEvent(EventAddEntitycs m, int eventId);
        Task<bool> DeleteEvent(int eventId);
    }

    public class EventServer : IEventServer
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventServer(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<EventEntity> AddEvent(EventAddEntitycs m)
        {
            var eventEntity = new EventEntity(m.ActivityID, m.ActivityName, m.LocationOfActivity, m.ActivityExecutionTime, m.Time, m.EntityResponsibleActivity, m.ConcilMemberID, m.ImagePath);
            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();
            return eventEntity;
        }

        public async Task<List<EventEntity>> GetAllEvent()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<EventEntity> GetEvent(string EntityResponsibleActivity)
        {
            return await _context.Events.FirstOrDefaultAsync(x => x.EntityResponsibleActivity == EntityResponsibleActivity);
        }

        public async Task<bool> DeleteEvent(int eventId)
        {
            var eventToDelete = await _context.Events.FindAsync(eventId);
            if (eventToDelete == null)
                return false;

            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EventEntity> UpdateEvent(EventAddEntitycs m, int id)
        {
            var existingEvent = await _context.Events.FindAsync(id);
            if (existingEvent == null)
            {
                return null;
            }

            existingEvent.ActivityName = m.ActivityName;
            existingEvent.ActivityID = m.ActivityID;
            existingEvent.LocationOfActivity = m.LocationOfActivity;
            existingEvent.ActivityExecutionTime = m.ActivityExecutionTime;
            existingEvent.Time = m.Time;
            existingEvent.EntityResponsibleActivity = m.EntityResponsibleActivity;
            existingEvent.ConcilMemberID = m.ConcilMemberID;
            existingEvent.ImagePath = m.ImagePath;

            await _context.SaveChangesAsync();
            return existingEvent;
        }
    }
}
