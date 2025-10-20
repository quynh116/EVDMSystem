using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.Appointment
{
    public class AppointmentCreateRequest
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid VehicleId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        public string? Note { get; set; }
    }
}
