using EVMDealerSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllInventoriesAsync();
        Task<Inventory?> GetInventoryByIdAsync(Guid id);
        Task<Inventory> AddInventoryAsync(Inventory inventory);
        Task UpdateInventoryAsync(Inventory inventory);
        Task DeleteInventoryAsync(Guid id);
        Task AddRangeInventoryAsync(IEnumerable<Inventory> inventories);
        Task<IEnumerable<Inventory>> GetInventoriesAtManufacturerAsync();
        Task<IEnumerable<Inventory>> FindAvailableStockForRequestAsync(Guid vehicleId, int quantity);
        Task UpdateRangeInventoryAsync(IEnumerable<Inventory> inventories);
        Task<int> CountAvailableStockByVehicleIdAsync(Guid vehicleId);
        Task<IEnumerable<Inventory>> GetReservedInventoryByRequestIdAsync(Guid requestId);
        Task<IReadOnlyDictionary<Guid, int>> GetStockCountByVehicleAndDealerAsync(Guid? dealerId = null);
        Task<IQueryable<Inventory>> GetInventoryQueryAsync();

        Task<IEnumerable<Inventory>> GetByDealerIdAsync(Guid dealerId);
        Task<IEnumerable<Inventory>> FindAvailableStockForAllocationAsync(Guid vehicleId, int quantity);

        Task<IEnumerable<Inventory>> GetAllocatedInventoryByRequestIdAsync(Guid requestId);

        Task<IEnumerable<Inventory>> GetMonthlyAllocatedInventoryAsync(DateTime startDate);
    }
}
