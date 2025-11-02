using System.Threading.Tasks;
using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;

namespace EVMDealerSystem.DataAccess.Repository.Implementations
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly EVMDealerSystemContext _context;

        public FeedbackRepository(EVMDealerSystemContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
 