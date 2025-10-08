using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.Vehicle
{
    public class VehicleUpdateRequest
    {
        public string? ModelName { get; set; }
        public string? Version { get; set; }
        public string? Category { get; set; }
        public string? Color { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public decimal? BatteryCapacity { get; set; }
        public int? RangePerCharge { get; set; }
        public decimal? BasePrice { get; set; } 
        public string? Status { get; set; }
        public DateTime? LaunchDate { get; set; }
    }
}
