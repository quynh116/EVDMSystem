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
    }
}
