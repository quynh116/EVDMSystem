using EVMDealerSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IDealerVehiclePriceRepository
    {
        Task<DealerVehiclePrice?> GetPriceByDealerAndVehicleAsync(Guid dealerId, Guid vehicleId);
        Task<IEnumerable<DealerVehiclePrice>> GetAllPricesForVehicleAsync(Guid vehicleId);
        Task<IEnumerable<DealerVehiclePrice>> GetAllPricesByDealerAsync(Guid dealerId);
        Task<DealerVehiclePrice> SetOrUpdatePriceAsync(DealerVehiclePrice price);
        Task<bool> DeletePriceAsync(Guid dealerId, Guid vehicleId);
    }
}
