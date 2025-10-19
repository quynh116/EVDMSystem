using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.VehicleRequest
{
    public class VehicleRequestCreateRequest
    {
        [Required]
        public Guid CreatedBy { get; set; }
        [Required]
        public Guid VehicleId { get; set; }
        [Required]
        public Guid DealerId { get; set; }
        public int Quantity { get; set; }
        public string? Note { get; set; }
    }
}
