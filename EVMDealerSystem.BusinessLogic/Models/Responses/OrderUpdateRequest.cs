using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Responses
{
    public class OrderUpdateRequest
    {
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentType { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string? Note { get; set; }
    }
}
