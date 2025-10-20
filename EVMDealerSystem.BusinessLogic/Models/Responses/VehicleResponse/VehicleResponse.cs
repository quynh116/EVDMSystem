using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Responses.VehicleResponse
{
    public class VehicleResponse
    {
        public Guid Id { get; set; }
        public string ModelName { get; set; } = null!;
        public string? Version { get; set; }
        public string? Category { get; set; }
        public string? Color { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public decimal? BatteryCapacity { get; set; }
        public int? RangePerCharge { get; set; }
        public decimal BasePrice { get; set; }
        public string? Status { get; set; }
        public DateTime? LaunchDate { get; set; }
        public Guid EvmId { get; set; }
        public Guid? DealerId { get; set; }
        public string? EvmName { get; set; }
        public int CurrentStock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
