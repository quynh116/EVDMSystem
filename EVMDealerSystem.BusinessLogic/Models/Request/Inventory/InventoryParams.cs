using EVMDealerSystem.BusinessLogic.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.Inventory
{
    public class InventoryParams : PaginationParams
    {
        public string? VehicleModelName { get; set; }
        public string? DealerName { get; set; }
        public string? Status { get; set; }
    }
}
