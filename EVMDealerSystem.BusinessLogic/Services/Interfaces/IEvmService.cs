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
    public interface IEvmService
    {
        Task<Result<IEnumerable<EvmResponse>>> GetAllEvmsAsync();
        Task<Result<EvmResponse>> GetEvmByIdAsync(Guid id);
        Task<Result<EvmResponse>> CreateEvmAsync(EvmRequest request);
        Task<Result<EvmResponse>> UpdateEvmAsync(Guid id, EvmRequest request);
        Task<Result<bool>> DeleteEvmAsync(Guid id);
    }
}
