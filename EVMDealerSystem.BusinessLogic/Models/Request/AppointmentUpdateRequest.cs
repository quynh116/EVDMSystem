using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Request.Appointment
{
    public class AppointmentUpdateRequest
    {
        public DateTime? AppointmentDate { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
    }
}
