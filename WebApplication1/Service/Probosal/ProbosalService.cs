using WebApplication1.Model;

namespace WebApplication1.Service.Probosal
{
    public interface IProbosalService
    {
        Task<Table.Probosal> AddProbosal(ProbosalEntity m);
        Task<Table.Probosal> UpdateProbosal(ProbosalEntity m,int probosalid);

        Task<bool> DeletePRobosal(int probosalID);
        void setsessionvalue(Table.Probosal probosaltEntity);


    }
    public class ProbosalService : IProbosalService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProbosalService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Table.Probosal> AddProbosal(ProbosalEntity m)
        {
            var probosal=new Table.Probosal(m.probosalID,m.probosalDescription,m.NameOFCommittee,m.imagePath);
            _context.Proposals.Add(probosal);
            await _context.SaveChangesAsync();
            return probosal;
        }
        public async Task<Table.Probosal> UpdateProbosal(ProbosalEntity m, int probosalid)
        {
            var exitingprobosal = await _context.Proposals.FindAsync(probosalid);
            if (exitingprobosal != null)
            {
                exitingprobosal.ProbosalDescribtion = m.probosalDescription;
                exitingprobosal.TargetParty = m.NameOFCommittee;
                exitingprobosal.ImagePath = m.imagePath;
                await _context.SaveChangesAsync();
                return exitingprobosal;
            }
            else
            {
                return null;

            }


        }
        public async Task<Boolean> DeletePRobosal(int probosalid)
        {
            var probosalDelet=await _context.Proposals.FindAsync(probosalid);
            if(probosalDelet != null)
            {
                _context.Proposals.Remove(probosalDelet);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }
        public void setsessionvalue(Table.Probosal probosal)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32("ProbosalID", probosal.ProbosalID);
            _httpContextAccessor.HttpContext.Session.SetString("description", probosal.ProbosalDescribtion);
            _httpContextAccessor.HttpContext.Session.SetString("TargetParty", probosal.TargetParty);
            _httpContextAccessor.HttpContext.Session.SetString("ImagePath", probosal.ImagePath);


        }
    }
}
