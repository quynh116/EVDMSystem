using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.Inventory;
using EVMDealerSystem.BusinessLogic.Models.Request.Vehicle;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Models.Responses.VehicleResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<Result<IEnumerable<VehicleResponse>>> GetAllVehiclesAsync();
        Task<Result<VehicleResponse>> GetVehicleByIdAsync(Guid id);
        Task<Result<VehicleResponse>> CreateVehicleAsync(VehicleCreateRequest request);
        Task<Result<VehicleResponse>> UpdateVehicleAsync(Guid id, VehicleUpdateRequest request);
        Task<Result<bool>> DeleteVehicleAsync(Guid id);
        Task<Result<IEnumerable<InventoryResponse>>> AddInventoryBatchAsync(InventoryBatchCreateRequest request);
    }
}
