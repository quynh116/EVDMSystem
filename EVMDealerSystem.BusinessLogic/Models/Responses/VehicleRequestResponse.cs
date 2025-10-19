using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Responses
{
    public class VehicleRequestResponse
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public string CreatedByName { get; set; } = null!;
        public Guid VehicleId { get; set; }
        public string VehicleModelName { get; set; } = null!;
        public Guid DealerId { get; set; }
        public string DealerName { get; set; } = null!;
        public int Quantity { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ApprovedBy { get; set; }
        public string? ApprovedByName { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string? Note { get; set; }
    }
}
