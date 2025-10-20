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
        public string CustomerName { get; set; } = null!;
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
        public Guid DealerStaffId { get; set; }
        public string DealerStaffName { get; set; } = null!;
        public Guid VehicleId { get; set; }
        public string VehicleModelName { get; set; } = null!;
        public string? VehicleVersion { get; set; }
        public string? VehicleColor { get; set; }
        public Guid DealerId { get; set; }
        public string DealerName { get; set; } = null!;
        public Guid InventoryId { get; set; }
        public string VinNumber { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string? Note { get; set; }
    }
}