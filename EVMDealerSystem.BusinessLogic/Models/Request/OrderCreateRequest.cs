using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EVMDealerSystem.BusinessLogic.Models.Request
{
    public class OrderCreateRequest
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid VehicleId { get; set; }

        [Required]
        public Guid InventoryId { get; set; }

        public string? PaymentType { get; set; }

        public Guid? PromotionId { get; set; }

        public string? Note { get; set; }
    }
}
