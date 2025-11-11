using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.Promotion
{
    public class PromotionCreateRequest
    {
        [Required]
        public Guid CreatedBy { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }


        public decimal? DiscountPercent { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public string? Note { get; set; }
    }
}
