using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository
{
    public class VehicleRequestItemRepository : IVehicleRequestItemRepository
    {
        private readonly EVMDealerSystemContext _context;

        public VehicleRequestItemRepository(EVMDealerSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VehicleRequestItem>> GetItemsByRequestIdAsync(Guid requestId)
        {
            return await _context.VehicleRequestItems
                .Where(i => i.VehicleRequestId == requestId)
                .Include(i => i.Vehicle) 
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleRequestItem>> GetItemsByVehicleIdAsync(Guid vehicleId)
        {
            return await _context.VehicleRequestItems
                .Where(i => i.VehicleId == vehicleId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<VehicleRequestItem?> GetItemByIdAsync(Guid id)
        {
            return await _context.VehicleRequestItems
                .Include(i => i.Vehicle)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<VehicleRequestItem> AddItemAsync(VehicleRequestItem item)
        {
            item.CreatedAt = DateTime.UtcNow;
            _context.VehicleRequestItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<VehicleRequestItem> UpdateItemAsync(VehicleRequestItem item)
        {
            item.UpdatedAt = DateTime.UtcNow;
            _context.VehicleRequestItems.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var item = await _context.VehicleRequestItems.FindAsync(id);
            if (item != null)
            {
                _context.VehicleRequestItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteItemsByRequestIdAsync(Guid requestId)
        {
            var items = await _context.VehicleRequestItems
                .Where(i => i.VehicleRequestId == requestId)
                .ToListAsync();

            _context.VehicleRequestItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
