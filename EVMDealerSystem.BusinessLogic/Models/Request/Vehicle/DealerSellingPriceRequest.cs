using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.Vehicle
{
    public class DealerSellingPriceRequest
    {
        public Guid VehicleId { get; set; }
        public decimal SellingPrice { get; set; }
    }
}
