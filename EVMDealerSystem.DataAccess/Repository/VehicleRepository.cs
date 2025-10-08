using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly EVMDealerSystemContext _context;

        public VehicleRepository(EVMDealerSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _context.Vehicles.Include(v => v.Evm).ToListAsync();
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(Guid id)
        {
            return await _context.Vehicles
                .Include(v => v.Evm) 
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVehicleAsync(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }
    }
}
