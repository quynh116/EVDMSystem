using EVMDealerSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IVehicleRequestRepository
    {
        Task<IQueryable<VehicleRequest>> GetAllVehicleRequestsAsync();
        Task<VehicleRequest?> GetVehicleRequestByIdAsync(Guid id);
        Task<VehicleRequest> AddVehicleRequestAsync(VehicleRequest request);
        Task UpdateVehicleRequestAsync(VehicleRequest request);
        Task DeleteVehicleRequestAsync(Guid id);
    }
}
