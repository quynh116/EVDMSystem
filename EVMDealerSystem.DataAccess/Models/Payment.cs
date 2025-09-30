using System;
using System.Collections.Generic;

namespace EVMDealerSystem.DataAccess.Models;

public partial class Payment
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public decimal Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime TransactionDate { get; set; }

    public string? Note { get; set; }

    public virtual Order Order { get; set; } = null!;
}
