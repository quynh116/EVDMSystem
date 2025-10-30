using System;
using System.Collections.Generic;

namespace EVMDealerSystem.DataAccess.Models;

public partial class VehicleRequest
{
    public Guid Id { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid VehicleId { get; set; }

    public Guid DealerId { get; set; }

    public int Quantity { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public string? Note { get; set; }
    public string? CancellationReason { get; set; }
    public Guid? CanceledBy { get; set; }
    public DateTime? CanceledAt { get; set; }
    public virtual User? ApprovedByNavigation { get; set; }
    public virtual User? CanceledByNavigation { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Dealer Dealer { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
