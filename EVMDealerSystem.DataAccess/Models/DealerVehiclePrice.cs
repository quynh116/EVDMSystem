using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Models
{
    public class DealerVehiclePrice
    {
        public Guid DealerId { get; set; }
        public Guid VehicleId { get; set; }

        // Giá bán lẻ do đại lý đặt
        public decimal SellingPrice { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual Dealer Dealer { get; set; } = null!;
        public virtual Vehicle Vehicle { get; set; } = null!;
    }
}
