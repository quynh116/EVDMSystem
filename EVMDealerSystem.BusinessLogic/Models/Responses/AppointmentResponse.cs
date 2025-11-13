using System;

namespace EVMDealerSystem.BusinessLogic.Models.Responses
{
    public class AppointmentResponse
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
        public DateTime AppointmentDate { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Note { get; set; }
    }
}
