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
            if (request.CustomerId == null && request.NewCustomer == null)
                return Result<OrderResponse>.Invalid("Either CustomerId or NewCustomer must be provided.");

            // find an available inventory item for this vehicle at the dealer
            var invQuery = await _inventoryRepo.GetInventoryQueryAsync();
            var inventory = await invQuery
                .Where(i => i.VehicleId == request.VehicleId && i.DealerId == request.DealerId && i.Status.ToLower() == "available")
                .Include(i => i.Vehicle)
                .FirstOrDefaultAsync();

            if (inventory == null)
                return Result<OrderResponse>.NotFound("No available inventory for the selected vehicle at this dealer.");

            using (var tx = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Guid customerId;
                    if (request.CustomerId.HasValue && request.CustomerId != Guid.Empty)
                    {
                        var existing = await _customerRepo.GetByIdAsync(request.CustomerId.Value);
                        if (existing == null)
                            return Result<OrderResponse>.NotFound("Customer not found.");
                        customerId = existing.Id;
                    }
                    else
                    {
                        // create customer directly via DbContext because repository interface lacks add method
                        var c = new Customer
                        {
                            Id = Guid.NewGuid(),
                            FullName = request.NewCustomer!.FullName,
                            Phone = request.NewCustomer.Phone,
                            Email = request.NewCustomer.Email,
                            Address = request.NewCustomer.Address,
                            DealerStaffId = dealerStaffId,
                            CreatedAt = DateTime.UtcNow
                        };
                        await _context.Customers.AddAsync(c);
                        await _context.SaveChangesAsync();
                        customerId = c.Id;
                    }

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

                    // update inventory status to sold
                    inventory.Status = "sold";
                    inventory.UpdatedAt = DateTime.UtcNow;
                    await _inventoryRepo.UpdateInventoryAsync(inventory);

                    // optional feedback
                    if (request.Feedback != null && _feedbackRepo != null)
                    {
                        var fb = new Feedback
                        {
                            Id = Guid.NewGuid(),
                            CustomerId = customerId,
                            OrderId = created.Id,
                            Subject = request.Feedback.Subject ?? "Order Feedback",
                            Content = request.Feedback.Content ?? string.Empty,
                            FeedbackType = "general",
                            Status = "open",
                            CreatedAt = DateTime.UtcNow,
                            Note = request.Feedback.Rating?.ToString()
                        };
                        await _feedbackRepo.AddAsync(fb);
                    }
                    else if (request.Feedback != null && _feedbackRepo == null)
                    {
                        // fallback: insert feedback via context
                        var fb2 = new Feedback
                        {
                            Id = Guid.NewGuid(),
                            CustomerId = customerId,
                            OrderId = created.Id,
                            Subject = request.Feedback.Subject ?? "Order Feedback",
                            Content = request.Feedback.Content ?? string.Empty,
                            FeedbackType = "general",
                            Status = "open",
                            CreatedAt = DateTime.UtcNow,
                            Note = request.Feedback.Rating?.ToString()
                        };
                        await _context.Feedbacks.AddAsync(fb2);
                        await _context.SaveChangesAsync();
                    }

                    await tx.CommitAsync();

                    // Build response (light)
                    var cust = await _customerRepo.GetByIdAsync(customerId);
                    var user = await _userRepo.GetUserByIdAsync(dealerStaffId);

                    var resp = new BusinessLogic.Models.Responses.OrderResponse
                    {
                        Id = created.Id,
                        CustomerId = created.CustomerId,
                        CustomerName = cust?.FullName ?? string.Empty,
                        CustomerPhone = cust?.Phone,
                        DealerStaffId = created.DealerStaffId,
                        DealerStaffName = user?.FullName ?? string.Empty,
                        VehicleId = created.VehicleId,
                        VehicleModelName = inventory.Vehicle?.ModelName ?? string.Empty,
                        VehicleVersion = inventory.Vehicle?.Version,
                        DealerId = created.DealerId,
                        DealerName = inventory.Dealer?.Name ?? string.Empty,
                        InventoryId = created.InventoryId,
                        VinNumber = inventory.VinNumber,
                        TotalPrice = created.TotalPrice,
                        OrderStatus = created.OrderStatus,
                        PaymentStatus = created.PaymentStatus,
                        PaymentType = created.PaymentType,
                        CreatedAt = created.CreatedAt,
                        Note = created.Note
                    };

                    return Result<BusinessLogic.Models.Responses.OrderResponse>.Success(resp, "Order created");
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    return Result<BusinessLogic.Models.Responses.OrderResponse>.InternalServerError("Create order failed: " + ex.Message);
                }
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