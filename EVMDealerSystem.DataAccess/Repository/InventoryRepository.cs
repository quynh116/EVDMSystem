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
    public class InventoryRepository : IInventoryRepository
    {
        private readonly EVMDealerSystemContext _context;

        public InventoryRepository(EVMDealerSystemContext context)
        {
            _context = context;
        }

        private IQueryable<Inventory> GetQueryWithIncludes()
        {
            return _context.Inventories
                .Include(i => i.Dealer)
                .Include(i => i.Vehicle);
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoriesAsync()
        {
            return await GetQueryWithIncludes().ToListAsync();
        }

        public async Task<Inventory?> GetInventoryByIdAsync(Guid id)
        {
            return await GetQueryWithIncludes()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Inventory> AddInventoryAsync(Inventory inventory)
        {
            await _context.Inventories.AddAsync(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInventoryAsync(Guid id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddRangeInventoryAsync(IEnumerable<Inventory> inventories)
        {
            await _context.Inventories.AddRangeAsync(inventories);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesAtManufacturerAsync()
        {
            return await GetQueryWithIncludes()
        .Where(i => i.Status == "At Manufacturer" && i.DealerId == null)
        .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> FindAvailableStockForRequestAsync(Guid vehicleId, int quantity)
        {
            return await GetQueryWithIncludes()
        .Where(i => i.VehicleId == vehicleId &&
                    i.Status == "At Manufacturer" &&
                    i.DealerId == null)
        .Take(quantity) 
        .ToListAsync();
        }

        public async Task UpdateRangeInventoryAsync(IEnumerable<Inventory> inventories)
        {
            _context.Inventories.UpdateRange(inventories);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAvailableStockByVehicleIdAsync(Guid vehicleId)
        {
            return await _context.Inventories
        .CountAsync(i => i.VehicleId == vehicleId &&
                         i.Status == "At Manufacturer" &&
                         i.DealerId == null);
        }

        public async Task<IEnumerable<Inventory>> GetReservedInventoryByRequestIdAsync(Guid requestId)
        {
            return await GetQueryWithIncludes()
            .Where(i => i.VehicleRequestId == requestId && i.Status == "Requested by Dealer")
            .ToListAsync();
        }
    }
}
