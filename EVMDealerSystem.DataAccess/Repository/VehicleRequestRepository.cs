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
    public class VehicleRequestRepository : IVehicleRequestRepository
    {
        private readonly EVMDealerSystemContext _context;

        public VehicleRequestRepository(EVMDealerSystemContext context)
        {
            _context = context;
        }

        private IQueryable<VehicleRequest> GetQueryWithIncludes()
        {
            return _context.VehicleRequests
                .Include(r => r.Dealer)
                .Include(r => r.Vehicle)
                .Include(r => r.CreatedByNavigation)
                .Include(r => r.ApprovedByNavigation);
        }

        public async Task<IQueryable<VehicleRequest>> GetAllVehicleRequestsAsync()
        {
            return _context.VehicleRequests
        .Include(vr => vr.CreatedByNavigation)
        .Include(vr => vr.Dealer)
        .Include(vr => vr.Vehicle)
        .AsNoTracking();
        }

        public async Task<VehicleRequest?> GetVehicleRequestByIdAsync(Guid id)
        {
            return await GetQueryWithIncludes()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<VehicleRequest> AddVehicleRequestAsync(VehicleRequest request)
        {
            await _context.VehicleRequests.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task UpdateVehicleRequestAsync(VehicleRequest request)
        {
            _context.VehicleRequests.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVehicleRequestAsync(Guid id)
        {
            var request = await _context.VehicleRequests.FindAsync(id);
            if (request != null)
            {
                _context.VehicleRequests.Remove(request);
                await _context.SaveChangesAsync();
            }
        }
    }
}
