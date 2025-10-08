using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services.Interfaces
{
    public interface IDealerService
    {
        Task<Result<IEnumerable<DealerResponse>>> GetAllDealersAsync();
        Task<Result<DealerResponse>> GetDealerByIdAsync(Guid id);
        Task<Result<DealerResponse>> CreateDealerAsync(DealerRequest request);
        Task<Result<DealerResponse>> UpdateDealerAsync(Guid id, DealerRequest request);
        Task<Result<bool>> DeleteDealerAsync(Guid id);
    }
}
