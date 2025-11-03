using System;
using System.ComponentModel.DataAnnotations;

namespace EVMDealerSystem.BusinessLogic.Models.Request
{
    public class PaymentCreateRequest
    {
        [Required] public Guid OrderId { get; set; }
        [Required] public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? Note { get; set; }
    }

    public class PaymentUpdateRequest
    {
        public decimal? Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? Note { get; set; }
    }
}
