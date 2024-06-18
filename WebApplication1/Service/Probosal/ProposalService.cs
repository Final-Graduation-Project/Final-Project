using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;
using WebApplication1.Table;

namespace WebApplication1.Service.Probosal
{
    public interface IProposalService
    {
        Task<Proposal> AddProposal(ProposalEntity proposalEntity);
        Task<Proposal> UpdateProposal(ProposalEntity proposalEntity, int proposalId);
        Task<bool> DeleteProposal(int proposalId);
        Task<bool> AcceptProposal(int proposalId);
        void SetSessionValue(Proposal proposal);
        Task<List<Proposal>> GetAcceptedProposals();
        Task<List<Proposal>> GetAcceptedProposalsByUserId(int userId); // تعديل الاسم
        Task<List<Proposal>> GetUnacceptedProposalsByCommittee(string committee); // تعريف الميثود الجديدة




    }

    public class ProposalService : IProposalService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProposalService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Proposal> AddProposal(ProposalEntity m)
        {
            var proposal = new Proposal
            {
                Type = m.Type,
                Question = m.Question,
                Committee = m.Committee,
                Votes = 0,
                Accepted = false,
                UserID = m.UserID,
                OptionText = m.OptionText,
                CommentText = m.CommentText
            };

            _context.Proposals.Add(proposal);
            await _context.SaveChangesAsync();
            return proposal;
        }

        public async Task<Proposal> UpdateProposal(ProposalEntity proposalEntity, int proposalId)
        {
            var existingProposal = await _context.Proposals.FindAsync(proposalId);
            if (existingProposal != null)
            {
                existingProposal.Type = proposalEntity.Type;
                existingProposal.Question = proposalEntity.Question;
                existingProposal.Committee = proposalEntity.Committee;
                existingProposal.OptionText = proposalEntity.OptionText;
                existingProposal.CommentText = proposalEntity.CommentText;
                await _context.SaveChangesAsync();
                return existingProposal;
            }
            return null;
        }

        public async Task<bool> DeleteProposal(int proposalId)
        {
            var proposalToDelete = await _context.Proposals.FindAsync(proposalId);
            if (proposalToDelete != null)
            {
                _context.Proposals.Remove(proposalToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> AcceptProposal(int proposalId)
        {
            var proposal = await _context.Proposals.FindAsync(proposalId);
            if (proposal != null)
            {
                proposal.Accepted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public void SetSessionValue(Proposal proposal)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32("ProposalID", proposal.ProposalID);
            _httpContextAccessor.HttpContext.Session.SetString("Description", proposal.Question);
            _httpContextAccessor.HttpContext.Session.SetString("TargetParty", proposal.Committee);
        }
        

        public async Task<List<Proposal>> GetAcceptedProposals()
        {
            return await _context.Proposals.Where(p => p.Accepted).ToListAsync();
        }
        public async Task<List<Proposal>> GetAcceptedProposalsByUserId(int userId)
        {
            return await _context.Proposals
                .Where(p => !p.Accepted && p.UserID == userId)
                .ToListAsync();
        }
        public async Task<List<Proposal>> GetUnacceptedProposalsByCommittee(string committee)
        {
            return await _context.Proposals
                .Where(p => p.Committee == committee && !p.Accepted)
                .ToListAsync();
        }
    }
}
