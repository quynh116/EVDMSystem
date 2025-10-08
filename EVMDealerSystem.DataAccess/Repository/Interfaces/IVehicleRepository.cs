using EVMDealerSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<Vehicle?> GetVehicleByIdAsync(Guid id);

        Task<Vehicle> AddVehicleAsync(Vehicle vehicle);

        Task UpdateVehicleAsync(Vehicle vehicle);

        Task DeleteVehicleAsync(Guid id);
    }
}
