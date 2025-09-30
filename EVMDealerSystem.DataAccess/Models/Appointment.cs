using System;
using System.Collections.Generic;

namespace EVMDealerSystem.DataAccess.Models;

public partial class Appointment
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public Guid DealerStaffId { get; set; }

    public Guid VehicleId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Note { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual User DealerStaff { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
