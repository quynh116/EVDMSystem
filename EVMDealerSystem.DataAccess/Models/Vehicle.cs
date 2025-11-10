using System;
using System.Collections.Generic;

namespace EVMDealerSystem.DataAccess.Models;

public partial class Vehicle
{
    public Guid Id { get; set; }

    public string ModelName { get; set; } = null!;

    public string? Version { get; set; }

    public string? Category { get; set; }

    public string? Color { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public decimal? BatteryCapacity { get; set; }

    public int? RangePerCharge { get; set; }

    public decimal BasePrice { get; set; }

    public string? Status { get; set; }

    public DateTime? LaunchDate { get; set; }

    public Guid EvmId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Evm Evm { get; set; } = null!;

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

    public virtual ICollection<DealerVehiclePrice> DealerVehiclePrices { get; set; } = new List<DealerVehiclePrice>();
    public virtual ICollection<VehicleRequestItem> VehicleRequestItems { get; set; } = new List<VehicleRequestItem>();
}
