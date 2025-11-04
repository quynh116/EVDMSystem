using System;
using System.ComponentModel.DataAnnotations;

namespace EVMDealerSystem.BusinessLogic.Models.Request
{

    public class OrderCreateRequest
    {
        [Required] public Guid VehicleId { get; set; }
        [Required] public Guid DealerId { get; set; }
        public CustomerCreateRequest? NewCustomer { get; set; }

        [Required]
        [RegularExpression("^(full|installment)$", ErrorMessage = "PaymentType must be 'full' or 'installment'")]
        public string PaymentType { get; set; } = "full";

        public decimal? TotalPrice { get; set; }
    }
}