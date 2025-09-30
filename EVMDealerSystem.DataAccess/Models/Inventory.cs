using System;
using System.Collections.Generic;

namespace EVMDealerSystem.DataAccess.Models;

public partial class Inventory
{
    public Guid Id { get; set; }

    public Guid VehicleId { get; set; }

    public Guid DealerId { get; set; }

    public string VinNumber { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Dealer Dealer { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Vehicle Vehicle { get; set; } = null!;
}
