using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.Vehicle
{
    public class VehicleCreateRequest
    {
        public string ModelName { get; set; } = null!;
        
        public decimal BasePrice { get; set; }
        public string? Version { get; set; }
        public string? Category { get; set; }
        public string? Color { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public decimal? BatteryCapacity { get; set; }
        public int? RangePerCharge { get; set; }
        public DateTime? LaunchDate { get; set; }
    }
}
