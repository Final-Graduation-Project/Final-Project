using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Table;
using System.Collections.Generic;



namespace WebApplication1.Services.Event;

    public interface IEventServer
    {
        Task<Table.EventEntity> AddEvent(EventAddEntitycs m);

    Task<List<EventEntity>> GitAllEvent();

    Task<Table.EventEntity> GitEvent(string EntityResponsibleActivity);

    Task<EventEntity> UpdateEvent(EventAddEntitycs m,int evintid);
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

        public async Task<Table.EventEntity> AddEvent(EventAddEntitycs m)
        {
            var Event = new Table.EventEntity(m.ActivityID, m.ActivityName, m.LocationOfActivity, m.ActivityExecutionTime,
                m.time, m.EntityResponsibleActivity, m.concilMemberID, m.imagePath);
            _context.Events.Add(Event);
            await _context.SaveChangesAsync();
            return Event;

        }

        public async Task<List<EventEntity>> GitAllEvent()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Table.EventEntity> GitEvent(string name_of_the_committee)

        {
            var Event = await _context.Events.FirstOrDefaultAsync(x =>
                x.EntityResponsibleActivity == name_of_the_committee);
            return Event;
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
            existingEvent.time = m.time;
            existingEvent.EntityResponsibleActivity = m.EntityResponsibleActivity;
            existingEvent.ConcilMemberID = m.concilMemberID;

            await _context.SaveChangesAsync();
            return existingEvent;
        }
    


    }

