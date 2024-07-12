using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;
using WebApplication1.Table;
using Microsoft.Extensions.Logging;

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
        Task<List<Proposal>> GetAcceptedProposalsByUserId(int userId);
        Task<List<Proposal>> GetUnacceptedProposalsByCommittee(string committee);
        Task<bool> AddComment(int proposalId, string comment);
        Task<bool> AddVote(int proposalId, string option, int userId);
    }

    public class ProposalService : IProposalService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ProposalService> _logger;

        public ProposalService(AppDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<ProposalService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
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
                CommentText = m.CommentText,
                VotedUsers = "",
                name=m.name
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

        public async Task<bool> AddComment(int proposalId, string comment)
        {
            try
            {
                var proposal = await _context.Proposals.FindAsync(proposalId);
                if (proposal != null)
                {
                    if (proposal.CommentText == null)
                    {
                        proposal.CommentText = comment;
                    }
                    else
                    {
                        proposal.CommentText += $";{comment}";
                    }
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment to proposal {ProposalID}", proposalId);
                return false;
            }
        }

        public async Task<bool> AddVote(int proposalId, string option, int userId)
        {
            try
            {
                var proposal = await _context.Proposals.FindAsync(proposalId);
                if (proposal != null)
                {
                    var votedUsers = proposal.VotedUsers?.Split(';') ?? new string[0];
                    if (votedUsers.Contains(userId.ToString()))
                    {
                        return false; // المستخدم قام بالتصويت بالفعل
                    }

                    var votes = new Dictionary<string, int>();
                    if (!string.IsNullOrEmpty(proposal.OptionText))
                    {
                        votes = proposal.OptionText.Split(',')
                            .Where(v => v.Contains(':'))
                            .Select(v => v.Split(':'))
                            .ToDictionary(v => v[0], v => int.Parse(v[1]));
                    }

                    if (votes.ContainsKey(option))
                    {
                        votes[option]++;
                    }
                    else
                    {
                        votes[option] = 1;
                    }

                    proposal.OptionText = string.Join(",", votes.Select(v => $"{v.Key}:{v.Value}"));
                    proposal.VotedUsers = string.Join(";", votedUsers.Append(userId.ToString()));

                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding vote to proposal {ProposalID}", proposalId);
                return false;
            }
        }







    }
}
