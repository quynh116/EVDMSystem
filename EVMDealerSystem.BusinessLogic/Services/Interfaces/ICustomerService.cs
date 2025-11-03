using EVMDealerSystem.BusinessLogic.Models.Request;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Commons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<Result<CustomerResponse>> CreateAsync(CustomerCreateRequest request, Guid dealerStaffId);
        Task<Result<CustomerResponse>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<CustomerResponse>>> GetAllAsync();
        Task<Result<CustomerResponse>> UpdateAsync(Guid id, CustomerUpdateRequest request);
        Task<Result<bool>> DeleteAsync(Guid id);
        Task<Result<CustomerResponse>> GetByPhoneAsync(string phone);
    }
}
