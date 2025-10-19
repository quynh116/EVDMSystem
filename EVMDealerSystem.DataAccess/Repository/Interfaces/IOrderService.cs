using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.Order;
using EVMDealerSystem.BusinessLogic.Models.Responses;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IOrderService
    {
        Task<Result<OrderResponse>> CreateOrderAsync(OrderCreateRequest request, Guid dealerStaffId);
        Task<Result<OrderResponse>> GetOrderByIdAsync(Guid id);
        Task<Result<IEnumerable<OrderResponse>>> GetOrdersByStaffAsync(Guid staffId);
        Task<Result<IEnumerable<AvailableVehicleForOrder>>> GetAvailableVehiclesAsync(Guid dealerId);
        Task<Result<decimal>> CalculateOrderPriceAsync(Guid vehicleId, Guid? promotionId);
    }
}
