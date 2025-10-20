using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request;
using EVMDealerSystem.BusinessLogic.Models.Request.Order;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Result<IEnumerable<OrderResponse>>> GetAllOrdersAsync();
        Task<Result<OrderResponse>> GetOrderByIdAsync(Guid id);
        Task<Result<OrderResponse>> CreateOrderAsync(OrderCreateRequest request, Guid dealerStaffId);
        Task<Result<OrderResponse>> UpdateOrderAsync(Guid id, OrderUpdateRequest request);
        Task<Result<bool>> DeleteOrderAsync(Guid id);
        Task<Result<IEnumerable<OrderResponse>>> GetOrdersByStaffAsync(Guid staffId);
        Task<Result<IEnumerable<OrderResponse>>> GetOrdersByDealerAsync(Guid dealerId);
    }
}
