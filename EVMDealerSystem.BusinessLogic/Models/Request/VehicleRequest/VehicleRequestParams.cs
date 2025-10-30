using EVMDealerSystem.BusinessLogic.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.VehicleRequest
{
    public class VehicleRequestParams : PaginationParams
    {
        public Guid? CreatedBy { get; set; }
        public string? Status { get; set; }
    }
}
