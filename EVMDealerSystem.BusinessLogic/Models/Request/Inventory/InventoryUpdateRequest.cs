using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.Inventory
{
    public class InventoryUpdateRequest
    {
        public string? VinNumber { get; set; }
        public string? Status { get; set; }
    }
}
