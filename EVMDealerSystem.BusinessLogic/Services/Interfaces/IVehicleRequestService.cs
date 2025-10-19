using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.VehicleRequest;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services.Interfaces
{
    public interface IVehicleRequestService
    {
        Task<Result<IEnumerable<VehicleRequestResponse>>> GetAllVehicleRequestsAsync();
        Task<Result<VehicleRequestResponse>> GetVehicleRequestByIdAsync(Guid id);
        Task<Result<VehicleRequestResponse>> CreateVehicleRequestAsync(VehicleRequestCreateRequest request);
        Task<Result<VehicleRequestResponse>> UpdateVehicleRequestAsync(Guid id, VehicleRequestUpdateRequest request);
        Task<Result<VehicleRequestResponse>> ApproveVehicleRequestAsync(Guid id, Guid approverId); 
        Task<Result<bool>> DeleteVehicleRequestAsync(Guid id);
    }
}
