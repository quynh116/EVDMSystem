using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly EVMDealerSystemContext _context;

        public PromotionRepository(EVMDealerSystemContext context)
        {
            _context = context;
        }

        public async Task<Promotion?> GetByIdAsync(Guid id)
        {
            return await _context.Promotions
                .Include(p => p.Vehicle)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Promotion?> GetActiveByVehicleIdAsync(Guid vehicleId)
        {
            var now = DateTime.Now;
            return await _context.Promotions
                .Where(p => p.VehicleId == vehicleId &&
                           p.IsActive &&
                           p.StartDate <= now &&
                           p.EndDate >= now)
                .FirstOrDefaultAsync();
        }
    }
}