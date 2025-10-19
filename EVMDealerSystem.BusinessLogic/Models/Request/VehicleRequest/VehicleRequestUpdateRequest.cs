using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.VehicleRequest
{
    public class VehicleRequestUpdateRequest
    {
        public int? Quantity { get; set; }
        public string? Note { get; set; }
    }
}
