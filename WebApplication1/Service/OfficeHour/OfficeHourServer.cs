using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.Service.OfficeHour
{
    public interface IOfficeHour
    {
        Task<Table.OfficeHour> AddOfficeHour(OfficeHourEntity m);

        Task<Table.OfficeHour> UpdateOfficeHour(OfficeHourEntity m, int OfficeHourid);

        Task<bool> DeletOfficeHour(int OfficeHourid);

        void setsessionvalue(Table.OfficeHour OfficeHourEntity);
        Task<Table.OfficeHour> GetOfficeHour(int OfficeHourid);
    }
    public class OfficeHourServer : IOfficeHour
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OfficeHourServer(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Table.OfficeHour> AddOfficeHour(OfficeHourEntity m)
        {
            var addOfficeHour=new Table.OfficeHour (m.OfficeHourid,m.teacherid,m.tehcherFreeDay,m.tehcerstartFreeTime,m.tehcerEndFreeTime,m.buildingName,m.rommNumber);
            _context.OfficeHours.Add(addOfficeHour);
            await _context.SaveChangesAsync();
            return addOfficeHour;
        }
        public async Task<Table.OfficeHour> UpdateOfficeHour(OfficeHourEntity m, int OfficeHourid)
        {
            var updateOfficeHour=await _context.OfficeHours.FindAsync(OfficeHourid);
            if (updateOfficeHour != null)
            {
                updateOfficeHour.TeacherFreeDay = m.tehcherFreeDay;
                updateOfficeHour.TeacherFreeStartTime = m.tehcerstartFreeTime;
                updateOfficeHour.TeacherFreeEndTime = m.tehcerEndFreeTime;
                updateOfficeHour.BuildingName = m.buildingName;
                updateOfficeHour.RoomNumber = m.rommNumber;
                await _context.SaveChangesAsync();
                return updateOfficeHour;

            }
            else
            {
                return null;
            }
        }
        public async Task<bool> DeletOfficeHour(int OfficeHourid)
        {
            var deletOfficeHour = await _context.OfficeHours.FindAsync(OfficeHourid);
            if (deletOfficeHour != null)
            {

            }
            _context.OfficeHours.Remove(deletOfficeHour);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Table.OfficeHour> GetOfficeHour(int TeacherId)
        {
            var getOfficeHour = await _context.OfficeHours.FirstAsync(x=>x.TeacherId==TeacherId);
            return getOfficeHour;
        }
        public void setsessionvalue(Table.OfficeHour officeHour) 

        {
            _httpContextAccessor.HttpContext.Session.SetInt32("officeHourid", officeHour.OfficeHourId);
            _httpContextAccessor.HttpContext.Session.SetInt32("tehcaer", officeHour.TeacherId);
            _httpContextAccessor.HttpContext.Session.SetString("tehcherFreeDay", officeHour.TeacherFreeDay.ToString());
            _httpContextAccessor.HttpContext.Session.SetString("TeacherFreeStartTime", officeHour.TeacherFreeStartTime.ToString());
            _httpContextAccessor.HttpContext.Session.SetString("TeacherFreeEndTime", officeHour.TeacherFreeEndTime.ToString());
            _httpContextAccessor.HttpContext.Session.SetString("buildingName", officeHour.BuildingName);
            _httpContextAccessor.HttpContext.Session.SetString("rommNumber", officeHour.RoomNumber);




        }

}
}
