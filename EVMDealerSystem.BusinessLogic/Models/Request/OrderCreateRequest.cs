using System;
using System.ComponentModel.DataAnnotations;

namespace EVMDealerSystem.BusinessLogic.Models.Request
{
    public class CustomerCreateRequest
    {
        [Required] public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

    public class FeedbackCreateRequest
    {
        public int? Rating { get; set; } // 1..5
        public string? Content { get; set; }
        public string? Subject { get; set; }
    }

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