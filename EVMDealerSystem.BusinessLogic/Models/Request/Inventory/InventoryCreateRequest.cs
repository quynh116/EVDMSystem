using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.Inventory
{
    public class InventoryCreateRequest
    {
        public Guid VehicleId { get; set; }
        public Guid? DealerId { get; set; }
        public string VinNumber { get; set; } = null!;
        public string? Status { get; set; }
    }
}
