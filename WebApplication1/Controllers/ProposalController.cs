using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Model;
using WebApplication1.Service.Probosal;
using WebApplication1.Resources;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalController : ControllerBase
    {
        private readonly IProposalService _proposalService;
        private readonly ILogger<ProposalController> _logger;

        public ProposalController(IProposalService proposalService, ILogger<ProposalController> logger)
        {
            _proposalService = proposalService;
            _logger = logger;
        }

        [HttpPost("AddProposal")]
        public async Task<IActionResult> AddProposal(ProposalEntity proposalEntity)
        {
            var proposal = await _proposalService.AddProposal(proposalEntity);
            var resource = new ProposalResource
            {
                ProposalID = proposal.ProposalID,
                Type = proposal.Type,
                Question = proposal.Question,
                Committee = proposal.Committee,
                UserID = proposal.UserID,
                OptionText = proposal.OptionText,
                CommentText = proposal.CommentText
            };
            return Ok(resource);
        }

        [HttpPut("UpdateProposal/{id}")]
        public async Task<IActionResult> UpdateProposal(ProposalEntity proposalEntity, int id)
        {
            var existingProposal = await _proposalService.UpdateProposal(proposalEntity, id);
            if (existingProposal != null)
            {
                var resource = new ProposalResource
                {
                    ProposalID = existingProposal.ProposalID,
                    Type = existingProposal.Type,
                    Question = existingProposal.Question,
                    Committee = existingProposal.Committee,
                    UserID = existingProposal.UserID,
                    OptionText = existingProposal.OptionText,
                    CommentText = existingProposal.CommentText
                };
                return Ok(resource);
            }
            return NotFound();
        }

        [HttpDelete("DeleteProposal/{id}")]
        public async Task<IActionResult> DeleteProposal(int id)
        {
            var isDeleted = await _proposalService.DeleteProposal(id);
            if (isDeleted)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut("AcceptProposal/{id}")]
        public async Task<IActionResult> AcceptProposal(int id)
        {
            var isAccepted = await _proposalService.AcceptProposal(id);
            if (isAccepted)
            {
                return Ok(new { message = "Proposal accepted successfully" });
            }
            return NotFound(new { message = "Proposal not found" });
        }

        [HttpGet("GetAcceptedProposalsByUserId/{userId}")]
        public async Task<IActionResult> GetAcceptedProposalsByUserId(int userId)
        {
            var proposals = await _proposalService.GetAcceptedProposalsByUserId(userId);
            var resources = proposals.Select(p => new ProposalResource
            {
                ProposalID = p.ProposalID,
                Type = p.Type,
                Question = p.Question,
                Committee = p.Committee,
                UserID = p.UserID,
                OptionText = p.OptionText,
                CommentText = p.CommentText,
                name=p.name
            }).ToList();

            return Ok(resources);
        }

        [HttpGet("GetAcceptedProposals")]
        public async Task<IActionResult> GetAcceptedProposals()
        {
            var proposals = await _proposalService.GetAcceptedProposals();
            var resources = proposals.Select(p => new ProposalResource
            {
                ProposalID = p.ProposalID,
                Type = p.Type,
                Question = p.Question,
                Committee = p.Committee,
                UserID = p.UserID,
                OptionText = p.OptionText,
                CommentText = p.CommentText,
                name=p.name

            }).ToList();

            return Ok(resources);
        }

        [HttpGet("GetUnacceptedProposalsByCommittee/{committee}")]
        public async Task<IActionResult> GetUnacceptedProposalsByCommittee(string committee)
        {
            var proposals = await _proposalService.GetUnacceptedProposalsByCommittee(committee);
            var resources = proposals.Select(p => new ProposalResource
            {
                ProposalID = p.ProposalID,
                Type = p.Type,
                Question = p.Question,
                Committee = p.Committee,
                UserID = p.UserID,
                OptionText = p.OptionText,
                CommentText = p.CommentText,
                name=p.name
            }).ToList();

            return Ok(resources);
        }

        // إضافة أسلوب لإضافة التعليق
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment(CommentResource commentResource)
        {
            _logger.LogInformation("Received AddComment request: ProposalID = {ProposalID}, Comment = {Comment}", commentResource.ProposalID, commentResource.Comment);
            try
            {
                var isAdded = await _proposalService.AddComment(commentResource.ProposalID, commentResource.Comment);
                if (isAdded)
                {
                    return Ok(new { message = "Comment added successfully" });
                }
                return NotFound(new { message = "Proposal not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // إضافة أسلوب لإضافة التصويت
        [HttpPost("AddVote")]
        public async Task<IActionResult> AddVote(VoteResource voteResource)
        {
            _logger.LogInformation("Received AddVote request: ProposalID = {ProposalID}, Option = {Option}, UserID = {UserID}", voteResource.ProposalID, voteResource.Option, voteResource.UserID);
            try
            {
                var isAdded = await _proposalService.AddVote(voteResource.ProposalID, voteResource.Option, voteResource.UserID);
                if (isAdded)
                {
                    return Ok(new { message = "Vote added successfully" });
                }
                return Conflict(new { message = "User has already voted" }); // تعارض إذا كان المستخدم قد صوت بالفعل
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding vote");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
    public class VoteResource
    {
        public int ProposalID { get; set; }
        public string Option { get; set; }
        public int UserID { get; set; } // إضافة userId إلى الموارد
    }



    public class CommentResource
    {
        public int ProposalID { get; set; }
        public string Comment { get; set; }
    }

}
