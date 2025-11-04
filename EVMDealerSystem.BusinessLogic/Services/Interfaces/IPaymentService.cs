using EVMDealerSystem.BusinessLogic.Models.Request;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Commons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Result<PaymentResponse>> CreateAsync(PaymentCreateRequest request);
        Task<Result<PaymentResponse>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<PaymentResponse>>> GetAllAsync();
        Task<Result<PaymentResponse>> UpdateAsync(Guid id, PaymentUpdateRequest request);
        Task<Result<bool>> DeleteAsync(Guid id);
        Task<Result<IEnumerable<PaymentResponse>>> GetByOrderIdAsync(Guid orderId);
    }
}
