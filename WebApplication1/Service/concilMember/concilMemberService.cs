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
        Task<studentConcilMember> GetConcilmember(int id);
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
        public async Task<studentConcilMember> GetConcilmember(int id)
        {
            var concilMember = await _context.studentConcilMembers.FindAsync(id);
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

      
    }
}
