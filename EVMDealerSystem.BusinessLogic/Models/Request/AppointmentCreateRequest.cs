using System;
using System.ComponentModel.DataAnnotations;

namespace EVMDealerSystem.BusinessLogic.Models.Request
{
    public class AppointmentCreateRequest
    {
        public CustomerCreateRequest? NewCustomer { get; set; }

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
