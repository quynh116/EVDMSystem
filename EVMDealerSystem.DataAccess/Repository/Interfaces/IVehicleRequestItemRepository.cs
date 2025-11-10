using EVMDealerSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IVehicleRequestItemRepository
    {
        Task<IEnumerable<VehicleRequestItem>> GetItemsByRequestIdAsync(Guid requestId);
        Task<VehicleRequestItem?> GetItemByIdAsync(Guid id);
        Task<IEnumerable<VehicleRequestItem>> GetItemsByVehicleIdAsync(Guid vehicleId);
        Task<VehicleRequestItem> AddItemAsync(VehicleRequestItem item);
        Task<VehicleRequestItem> UpdateItemAsync(VehicleRequestItem item);
        Task DeleteItemAsync(Guid id);
        Task DeleteItemsByRequestIdAsync(Guid requestId);
    }
}
