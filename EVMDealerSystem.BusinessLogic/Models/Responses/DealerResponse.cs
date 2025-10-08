using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Responses
{
    public class DealerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public Guid? EvmId { get; set; }
        public string? EvmName { get; set; } 
        public string? ContractNumber { get; set; }
        public decimal? SalesTarget { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
