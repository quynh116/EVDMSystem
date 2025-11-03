using System;

namespace EVMDealerSystem.BusinessLogic.Models.Responses
{
    public class PaymentResponse
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Note { get; set; }
    }
}
