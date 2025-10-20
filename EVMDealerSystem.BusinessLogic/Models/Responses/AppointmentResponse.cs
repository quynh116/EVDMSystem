using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Responses
{
    public class AppointmentResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string? CustomerPhone { get; set; }
        public Guid DealerStaffId { get; set; }
        public string DealerStaffName { get; set; } = null!;
        public Guid VehicleId { get; set; }
        public string VehicleModelName { get; set; } = null!;
        public string? VehicleVersion { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Note { get; set; }
    }
}
