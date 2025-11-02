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
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public Guid DealerStaffId { get; set; }
        public string DealerStaffName { get; set; } = string.Empty;
        public Guid VehicleId { get; set; }
        public string VehicleModelName { get; set; } = string.Empty;
        public string? VehicleVersion { get; set; }
        public Guid DealerId { get; set; }
        public string DealerName { get; set; } = string.Empty;
        public Guid InventoryId { get; set; }
        public string VinNumber { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string? Note { get; set; }
    }
}