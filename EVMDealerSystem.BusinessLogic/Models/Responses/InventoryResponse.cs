using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Responses
{
    public class InventoryResponse
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public string VehicleModelName { get; set; } = null!; 
        public Guid? DealerId { get; set; }
        public Guid? VehicleRequestId { get; set; }
        public string DealerName { get; set; } = null!; 
        public string VinNumber { get; set; } = null!;
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
