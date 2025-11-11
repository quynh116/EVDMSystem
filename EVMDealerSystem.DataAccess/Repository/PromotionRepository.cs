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

        private IQueryable<Promotion> GetQueryWithIncludes()
        {
            return _context.Promotions
                .Include(p => p.CreatedByNavigation)
                    .ThenInclude(u => u.Dealer)
                .AsNoTracking();
        }

        public async Task<Promotion?> GetByIdAsync(Guid id)
        {
            return await _context.Promotions
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        //public async Task<Promotion?> GetActiveByVehicleIdAsync(Guid vehicleId)
        //{
        //    var now = DateTime.Now;
        //    return await _context.Promotions
        //        .Where(p => p.VehicleId == vehicleId &&
        //                   p.IsActive &&
        //                   p.StartDate <= now &&
        //                   p.EndDate >= now)
        //        .FirstOrDefaultAsync();
        //}

        public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
        {
            return await GetQueryWithIncludes().ToListAsync();
        }

        public async Task<Promotion> AddPromotionAsync(Promotion promotion)
        {
            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();
            return promotion;
        }

        public async Task<Promotion> UpdatePromotionAsync(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();
            return promotion;
        }

        public async Task DeletePromotionAsync(Guid id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion != null)
            {
                _context.Promotions.Remove(promotion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Promotion?> GetPromotionByIdAsync(Guid id)
        {
            return await GetQueryWithIncludes().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Promotion>> GetPromotionsByDealerIdAsync(Guid dealerId)
        {
            return await _context.Promotions
                .Include(p => p.CreatedByNavigation)
                    .ThenInclude(u => u.Dealer)
                .Where(p => p.CreatedByNavigation.DealerId == dealerId) 
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task ApplyPromotionToVehicleAsync(Guid promotionId, Guid vehicleId)
        {
            var promotion = await _context.Promotions
                .Include(p => p.Vehicles)
                .FirstOrDefaultAsync(p => p.Id == promotionId);

            var vehicle = await _context.Vehicles.FindAsync(vehicleId);

            if (promotion != null && vehicle != null)
            {
                if (!promotion.Vehicles.Any(v => v.Id == vehicleId))
                {
                    promotion.Vehicles.Add(vehicle);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RemovePromotionFromVehicleAsync(Guid promotionId, Guid vehicleId)
        {
            var promotion = await _context.Promotions
                .Include(p => p.Vehicles)
                .FirstOrDefaultAsync(p => p.Id == promotionId);

            var vehicleToRemove = promotion?.Vehicles.FirstOrDefault(v => v.Id == vehicleId);

            if (promotion != null && vehicleToRemove != null)
            {
                promotion.Vehicles.Remove(vehicleToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Promotion?> GetPromotionWithVehiclesByIdAsync(Guid id)
        {
            return await _context.Promotions
                .Include(p => p.Vehicles)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Promotion>> GetActivePromotionsByVehicleIdAsync(Guid vehicleId, Guid dealerId)
        {
            var now = DateTime.UtcNow;
            return await _context.Promotions
            .Include(p => p.CreatedByNavigation)
        .Where(p =>
            p.Vehicles.Any(v => v.Id == vehicleId) &&

            p.CreatedByNavigation.DealerId == dealerId &&

            p.IsActive == true &&
            p.StartDate <= now &&
            p.EndDate >= now
        )
        .AsNoTracking()
        .ToListAsync();
        }
    }
}