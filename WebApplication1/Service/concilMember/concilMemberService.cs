using System.Threading.Tasks;
using WebApplication1.Model;
using WebApplication1.Models;
using WebApplication1.Table;

namespace WebApplication1.Service.concilMember
{
    public interface IconcilMemberService
    {
        Task<studentConcilMember> AddConcilmember(concilMemberEntity m);
        Task<studentConcilMember> UpdateConcilmember(concilMemberEntity m);
        void setsessionvalue(studentConcilMember concilMember);
    }

    public class concilMemberService : IconcilMemberService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public concilMemberService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<studentConcilMember> AddConcilmember(concilMemberEntity m)
        {
            var concilMember = new studentConcilMember(m.concilMemberID, m.concilMemberName, m.email, m.EntityResponsibleActivity);

            _context.studentConcilMembers.Add(concilMember);
            await _context.SaveChangesAsync();
            return concilMember;
        }

        public async Task<studentConcilMember> UpdateConcilmember(concilMemberEntity m)
        {
            var existingConcilMember = await _context.studentConcilMembers.FindAsync();
            if (existingConcilMember == null)
            {
                return null;
            }
            existingConcilMember.ConcilMemberID = m.concilMemberID;
            existingConcilMember.ConcilMemberName = m.concilMemberName;
            existingConcilMember.Email = m.email;
            existingConcilMember.EntityResponsibleActivity = m.EntityResponsibleActivity;

            await _context.SaveChangesAsync();
            return existingConcilMember;
        }

        public void setsessionvalue(studentConcilMember concilMember)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32("ActivityID", concilMember.ConcilMemberID);
            _httpContextAccessor.HttpContext.Session.SetString("ActivityName", concilMember.ConcilMemberName);
            _httpContextAccessor.HttpContext.Session.SetString("LocationOfActivity", concilMember.Email);
            _httpContextAccessor.HttpContext.Session.SetString("ActivityExecutionTime", concilMember.EntityResponsibleActivity);
        }
    }
}
