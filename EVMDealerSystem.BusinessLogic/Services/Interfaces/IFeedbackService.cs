using EVMDealerSystem.BusinessLogic.Models.Request;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Commons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<Result<FeedbackResponse>> CreateAsync(FeedbackCreateRequest request, Guid customerId);
        Task<Result<FeedbackResponse>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<FeedbackResponse>>> GetAllAsync();
        Task<Result<FeedbackResponse>> UpdateAsync(Guid id, FeedbackUpdateRequest request);
        Task<Result<bool>> DeleteAsync(Guid id);
        Task<Result<IEnumerable<FeedbackResponse>>> GetByOrderIdAsync(Guid orderId);
    }
}
