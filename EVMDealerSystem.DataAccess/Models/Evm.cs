using System;
using System.Collections.Generic;

namespace EVMDealerSystem.DataAccess.Models;

public partial class Evm
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Dealer> Dealers { get; set; } = new List<Dealer>();

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
