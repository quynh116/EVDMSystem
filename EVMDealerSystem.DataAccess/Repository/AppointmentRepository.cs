using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository.Implementations
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly EVMDealerSystemContext _context;
        public AppointmentRepository(EVMDealerSystemContext context) { _context = context; }

        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment?> GetByIdAsync(Guid id)
            => await _context.Appointments.FindAsync(id);

        public async Task<IEnumerable<Appointment>> GetAllAsync()
            => await _context.Appointments.OrderByDescending(a => a.AppointmentDate).ToListAsync();

        public async Task<Appointment> UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var a = await _context.Appointments.FindAsync(id);
            if (a == null) return false;
            _context.Appointments.Remove(a);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Appointment>> GetByDealerIdAsync(Guid dealerStaffId)
            => await _context.Appointments.Where(a => a.DealerStaffId == dealerStaffId).ToListAsync();

        public async Task<IEnumerable<Appointment>> GetByVehicleAndDateAsync(Guid vehicleId, DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1);
            return await _context.Appointments
                .Where(a => a.VehicleId == vehicleId && a.AppointmentDate >= start && a.AppointmentDate < end)
                .ToListAsync();
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
