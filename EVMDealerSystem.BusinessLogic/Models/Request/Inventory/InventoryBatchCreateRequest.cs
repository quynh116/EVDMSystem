using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.Inventory
{
    public class InventoryBatchCreateRequest
    {
        [Required]
        public Guid VehicleId { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000.")] 
        public int Quantity { get; set; }
    }
}
