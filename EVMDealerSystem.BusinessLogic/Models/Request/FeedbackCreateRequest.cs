using System;
using System.ComponentModel.DataAnnotations;

namespace EVMDealerSystem.BusinessLogic.Models.Request
{
    public class FeedbackCreateRequest
    {
        [Required] public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
         public string Content { get; set; } = string.Empty;
    }

    public class FeedbackUpdateRequest
    {
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public string? FeedbackType { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
    }
}
