using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository.Implementations
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly EVMDealerSystemContext _context;
        public FeedbackRepository(EVMDealerSystemContext context) { _context = context; }

        public async Task<Feedback> AddAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<Feedback?> GetByIdAsync(Guid id) => await _context.Feedbacks.FindAsync(id);

        public async Task<IEnumerable<Feedback>> GetAllAsync() => await _context.Feedbacks.OrderByDescending(f => f.CreatedAt).ToListAsync();

        public async Task<Feedback> UpdateAsync(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var f = await _context.Feedbacks.FindAsync(id);
            if (f == null) return false;
            _context.Feedbacks.Remove(f);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Feedback>> GetByOrderIdAsync(Guid orderId)
            => await _context.Feedbacks.Where(f => f.OrderId == orderId).ToListAsync();
    }
}
