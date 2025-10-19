using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.Inventory;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<Result<IEnumerable<InventoryResponse>>> GetAllInventoriesAsync();
        Task<Result<InventoryResponse>> GetInventoryByIdAsync(Guid id);
        Task<Result<InventoryResponse>> CreateInventoryAsync(InventoryCreateRequest request);
        Task<Result<InventoryResponse>> UpdateInventoryAsync(Guid id, InventoryUpdateRequest request);
        Task<Result<bool>> DeleteInventoryAsync(Guid id);
    }
}
