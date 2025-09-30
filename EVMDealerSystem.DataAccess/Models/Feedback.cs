using System;
using System.Collections.Generic;

namespace EVMDealerSystem.DataAccess.Models;

public partial class Feedback
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public Guid OrderId { get; set; }

    public string Subject { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? FeedbackType { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Note { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
