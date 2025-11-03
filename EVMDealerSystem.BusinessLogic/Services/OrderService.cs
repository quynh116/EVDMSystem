using System;
using System.Linq;
using System.Threading.Tasks;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using EVMDealerSystem.BusinessLogic.Models.Request;
using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using EVMDealerSystem.BusinessLogic.Models.Responses;

namespace EVMDealerSystem.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IInventoryRepository _inventoryRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly EVMDealerSystemContext _context;
        private readonly IUserRepository _userRepo;
        private readonly IFeedbackRepository? _feedbackRepo;

        public OrderService(
            IOrderRepository orderRepo,
            IInventoryRepository inventoryRepo,
            ICustomerRepository customerRepo,
            EVMDealerSystemContext context,
            IUserRepository userRepo,
            IFeedbackRepository? feedbackRepo = null)
        {
            _orderRepo = orderRepo;
            _inventoryRepo = inventoryRepo;
            _customerRepo = customerRepo;
            _context = context;
            _userRepo = userRepo;
            _feedbackRepo = feedbackRepo;
        }

        public async Task<Result<OrderResponse>> CreateOrderAsync(OrderCreateRequest request, Guid dealerStaffId)
        {
            if (request.NewCustomer == null || string.IsNullOrWhiteSpace(request.NewCustomer.Phone))
                return Result<OrderResponse>.Invalid("Customer phone must be provided.");

            // Tìm customer theo phone
            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Phone == request.NewCustomer.Phone);

            Guid customerId;
            if (existingCustomer != null)
            {
                customerId = existingCustomer.Id;
            }
            else
            {
                var newCust = new Customer
                {
                    Id = Guid.NewGuid(),
                    FullName = request.NewCustomer.FullName,
                    Phone = request.NewCustomer.Phone,
                    Email = request.NewCustomer.Email,
                    Address = request.NewCustomer.Address,
                    DealerStaffId = dealerStaffId,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.Customers.AddAsync(newCust);
                await _context.SaveChangesAsync();
                customerId = newCust.Id;
            }

            // Tìm inventory
            var invQuery = await _inventoryRepo.GetInventoryQueryAsync();
            var inventory = await invQuery
                .Where(i => i.VehicleId == request.VehicleId && i.DealerId == request.DealerId && i.Status.ToLower() == "Allocated to Dealer")
                .Include(i => i.Vehicle)
                .FirstOrDefaultAsync();

            if (inventory == null)
                return Result<OrderResponse>.NotFound("No available inventory for this vehicle at dealer.");

            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    DealerStaffId = dealerStaffId,
                    VehicleId = request.VehicleId,
                    DealerId = request.DealerId,
                    InventoryId = inventory.Id,
                    TotalPrice = request.TotalPrice ?? (inventory.Vehicle?.BasePrice ?? 0),
                    OrderStatus = "confirmed",
                    PaymentStatus = request.PaymentType.ToLower() == "full" ? "paid" : "partial_paid",
                    PaymentType = request.PaymentType.ToLower(),
                    CreatedAt = DateTime.UtcNow
                };

                var created = await _orderRepo.AddAsync(order);

                inventory.Status = "sold";
                inventory.UpdatedAt = DateTime.UtcNow;
                await _inventoryRepo.UpdateInventoryAsync(inventory);

                await tx.CommitAsync();

                var cust = await _customerRepo.GetByIdAsync(customerId);
                var user = await _userRepo.GetUserByIdAsync(dealerStaffId);

                var resp = new OrderResponse
                {
                    Id = created.Id,
                    CustomerId = cust!.Id,
                    CustomerName = cust.FullName,
                    CustomerPhone = cust.Phone,
                    DealerStaffId = dealerStaffId,
                    DealerStaffName = user?.FullName ?? string.Empty,
                    VehicleId = inventory.VehicleId,
                    VehicleModelName = inventory.Vehicle?.ModelName ?? string.Empty,
                    VehicleVersion = inventory.Vehicle?.Version,
                    DealerId = created.DealerId,
                    DealerName = inventory.Dealer?.Name ?? string.Empty,
                    InventoryId = inventory.Id,
                    VinNumber = inventory.VinNumber,
                    TotalPrice = created.TotalPrice,
                    OrderStatus = created.OrderStatus,
                    PaymentStatus = created.PaymentStatus,
                    PaymentType = created.PaymentType,
                    CreatedAt = created.CreatedAt
                };

                return Result<OrderResponse>.Success(resp, "Order created successfully.");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return Result<OrderResponse>.InternalServerError("Failed to create order: " + ex.Message);
            }
        }


        public async Task<Result<BusinessLogic.Models.Responses.OrderResponse>> GetByIdAsync(Guid orderId)
        {
            var o = await _orderRepo.GetByIdAsync(orderId);
            if (o == null) return Result<BusinessLogic.Models.Responses.OrderResponse>.NotFound("Order not found.");

            var resp = new BusinessLogic.Models.Responses.OrderResponse
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer?.FullName ?? string.Empty,
                CustomerPhone = o.Customer?.Phone,
                DealerStaffId = o.DealerStaffId,
                DealerStaffName = o.DealerStaff?.FullName ?? string.Empty,
                VehicleId = o.VehicleId,
                VehicleModelName = o.Vehicle?.ModelName ?? string.Empty,
                VehicleVersion = o.Vehicle?.Version,
                DealerId = o.DealerId,
                DealerName = o.Dealer?.Name ?? string.Empty,
                InventoryId = o.InventoryId,
                VinNumber = o.Inventory?.VinNumber ?? string.Empty,
                TotalPrice = o.TotalPrice,
                OrderStatus = o.OrderStatus,
                PaymentStatus = o.PaymentStatus,
                PaymentType = o.PaymentType,
                CreatedAt = o.CreatedAt,
                DeliveredAt = o.DeliveredAt,
                Note = o.Note
            };

            return Result<BusinessLogic.Models.Responses.OrderResponse>.Success(resp);
        }
    }
}