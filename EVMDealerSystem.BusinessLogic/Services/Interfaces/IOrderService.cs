using System;
using System.Threading.Tasks;
using EVMDealerSystem.BusinessLogic.Models.Request;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Commons;

namespace EVMDealerSystem.BusinessLogic.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Result<OrderResponse>> CreateOrderAsync(OrderCreateRequest request, Guid dealerStaffId);
        Task<Result<OrderResponse>> GetByIdAsync(Guid orderId);
        Task<Result<IEnumerable<OrderResponse>>> GetAllOrdersAsync();

    }
}