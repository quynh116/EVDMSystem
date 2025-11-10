using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Models
{
    public class VehicleRequestItem
    {
        public Guid Id { get; set; }

        public Guid VehicleRequestId { get; set; }

        public Guid VehicleId { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual VehicleRequest VehicleRequest { get; set; } = null!;
        public virtual Vehicle Vehicle { get; set; } = null!;
    }
}
