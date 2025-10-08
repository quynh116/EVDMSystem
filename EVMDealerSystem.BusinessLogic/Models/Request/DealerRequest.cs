using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request
{
    public class DealerRequest
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ContractNumber { get; set; }
        public decimal? SalesTarget { get; set; }
        public bool? IsActive { get; set; }
    }
}
