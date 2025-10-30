using System;
using System.Collections.Generic;

namespace EVMDealerSystem.DataAccess.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public int RoleId { get; set; }

    public Guid? DealerId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual Dealer? Dealer { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<VehicleRequest> VehicleRequestApprovedByNavigations { get; set; } = new List<VehicleRequest>();

    public virtual ICollection<VehicleRequest> VehicleRequestCreatedByNavigations { get; set; } = new List<VehicleRequest>();
    public virtual ICollection<VehicleRequest> VehicleRequestCanceledByNavigations { get; set; } = new List<VehicleRequest>();

}
