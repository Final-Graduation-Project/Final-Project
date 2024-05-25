using WebApplication1.Resorces;
using WebApplication1.Service.OfficeHour;
using WebApplication1.Service.StaffMembers;
using WebApplication1.Services.Event;
using WebApplication1.Table;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.Models;
using Microsoft.Identity.Client;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeHourController : Controller
    {
        private readonly IOfficeHour _OfficeHour;
        private readonly IStaffMemberService _staffMember;

        public OfficeHourController(IOfficeHour OfficeHour, IStaffMemberService staffMember)
        {
            _OfficeHour = OfficeHour;
            _staffMember = staffMember;
        }
       

        [HttpPost("AddOfficeHour")]
        public async Task<IActionResult> AddOfficeHour(OfficeHourEntity m)
        {
            var res = await _OfficeHour.AddOfficeHour(m);
            var resource = new OfficeHourResource
            {
                OfficeHourid = res.OfficeHourId,
                teacherid = res.TeacherId,
                tehcherFreeDay = res.TeacherFreeDay,
                tehcerstartFreeTime = res.TeacherFreeStartTime,
                tehcerEndFreeTime = res.TeacherFreeEndTime,
                buildingName = res.BuildingName,
                rommNumber = res.RoomNumber,

            };
            return Ok(resource);
        }
        [HttpPut("UpdateOfficeHour")]
        public async Task<IActionResult> UpdateOfficeHour(OfficeHourEntity m, int OfficeHourID)
        {
            var existingOfficeHour = await _OfficeHour.UpdateOfficeHour(m, OfficeHourID);
            if (existingOfficeHour != null)
            {
                var res = new OfficeHourResource
                {

                    OfficeHourid = existingOfficeHour.OfficeHourId,
                    teacherid = existingOfficeHour.TeacherId,
                    tehcherFreeDay = existingOfficeHour.TeacherFreeDay,
                    tehcerstartFreeTime = existingOfficeHour.TeacherFreeStartTime,
                    tehcerEndFreeTime = existingOfficeHour.TeacherFreeEndTime,
                    buildingName = existingOfficeHour.BuildingName,
                    rommNumber = existingOfficeHour.RoomNumber,
                };
                return Ok(res);
            }
            else
            {
                return NotFound();
            }

        }
        [HttpDelete("DeleteOfficeHour")]
        public async Task<IActionResult> DeleteOfficeHour(int OfficeHourID)
        {
            var isDelete=await _OfficeHour.DeletOfficeHour(OfficeHourID);
            if (!isDelete)
            {
                return NoContent();
            }
            else
            {
                return NotFound();  
            }
        }
        [HttpGet("GetOfficeHour")]
        public async Task<IActionResult> GetOfficeHour(string TeacherName)
        {
            int TeacherId = await _staffMember.GetStaffMemberId(TeacherName);
            var res = await _OfficeHour.GetOfficeHour(TeacherId);
            if (res != null)
            {
                var resource = new OfficeHourResource
                {
                    tehcherFreeDay = res.TeacherFreeDay,
                    tehcerstartFreeTime = res.TeacherFreeStartTime,
                    tehcerEndFreeTime = res.TeacherFreeEndTime,
                    buildingName = res.BuildingName,
                    rommNumber = res.RoomNumber,
                };
                return Ok(resource);
            }
            else
            {
                return NotFound();
            }
        }


    } 
}
