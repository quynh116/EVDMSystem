using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Responses
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = null!;

        // Customer Info
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }

        // Vehicle Info
        public Guid VehicleId { get; set; }
        public string VehicleModelName { get; set; } = null!;
        public string? VehicleVersion { get; set; }
        public string? VehicleColor { get; set; }

        // Inventory Info
        public Guid InventoryId { get; set; }
        public string VinNumber { get; set; } = null!;

        // Dealer Info
        public Guid DealerId { get; set; }
        public string DealerName { get; set; } = null!;
        public Guid DealerStaffId { get; set; }
        public string DealerStaffName { get; set; } = null!;

        // Pricing Info
        public decimal BasePrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalPrice { get; set; }

        // Order Status
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentType { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public string? Note { get; set; }

        // Promotion Info (if applied)
        public PromotionInfo? AppliedPromotion { get; set; }
    }

    public class PromotionInfo
    {
        public Guid PromotionId { get; set; }
        public string PromotionName { get; set; } = null!;
        public string DiscountType { get; set; } = null!;
        public decimal DiscountPercent { get; set; }
    }

    public class AvailableVehicleForOrder
    {
        public Guid VehicleId { get; set; }
        public string ModelName { get; set; } = null!;
        public string? Version { get; set; }
        public string? Color { get; set; }
        public decimal BasePrice { get; set; }
        public int AvailableStock { get; set; }
        public PromotionInfo? ActivePromotion { get; set; }
    }
}
