using System;
using System.ComponentModel.DataAnnotations;

namespace EVMDealerSystem.BusinessLogic.Models.Request
{
    public class AppointmentCreateRequest
    {
        public Guid? CustomerId { get; set; }           // optional
        public string? CustomerPhone { get; set; }      // if provided, try lookup
        public CustomerCreateRequest? NewCustomer { get; set; } // if provided, create new

        [Required] public Guid VehicleId { get; set; }
        [Required] public Guid DealerId { get; set; }
        [Required] public DateTime AppointmentDate { get; set; }
        public string? Note { get; set; }
    }

    public class AppointmentUpdateRequest
    {
        public DateTime? AppointmentDate { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
    }
}
