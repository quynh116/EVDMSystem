using System;

namespace EVMDealerSystem.BusinessLogic.Models.Responses
{
    public class FeedbackResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? FeedbackType { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Note { get; set; }
    }
}
