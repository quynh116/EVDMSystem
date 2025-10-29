using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.Promotion
{
    public class PromotionUpdateRequest
    {
        public Guid VehicleId { get; set; } 
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? DiscountPercent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public string? Note { get; set; }
    }
}
