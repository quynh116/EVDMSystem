using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.Promotion;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<Result<IEnumerable<PromotionResponse>>> GetAllPromotionsAsync();
        Task<Result<PromotionResponse>> GetPromotionByIdAsync(Guid id);
        Task<Result<PromotionResponse>> CreatePromotionAsync(PromotionCreateRequest request);
        Task<Result<PromotionResponse>> UpdatePromotionAsync(Guid id, PromotionUpdateRequest request);
        Task<Result<bool>> DeletePromotionAsync(Guid id);
    }
}
