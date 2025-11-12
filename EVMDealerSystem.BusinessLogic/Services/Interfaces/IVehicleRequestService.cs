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
        Task<Result<PagedList<VehicleRequestResponse>>> GetAllVehicleRequestsAsync(Guid? userId, VehicleRequestParams pagingParams);
        Task<Result<VehicleRequestResponse>> GetVehicleRequestByIdAsync(Guid id);
        Task<Result<VehicleRequestResponse>> CreateVehicleRequestAsync(VehicleRequestCreateRequest request);
        Task<Result<bool>> DeleteVehicleRequestAsync(Guid id);
        Task<Result<VehicleRequestResponse>> ApproveByDealerManagerAsync(Guid requestId, Guid managerId);
        Task<Result<VehicleRequestResponse>> RejectByDealerManagerAsync(Guid requestId, Guid managerId, string reason);
        Task<Result<VehicleRequestResponse>> ApproveByEVMAsync(Guid requestId, Guid evmStaffId, EVMApproveRequest request);
        Task<Result<VehicleRequestResponse>> RejectByEVMAsync(Guid requestId, Guid evmStaffId, string reason);
        Task<Result<VehicleRequestResponse>> ConfirmReceiptByDealerAsync(Guid requestId, Guid dealerId);
    }
}
