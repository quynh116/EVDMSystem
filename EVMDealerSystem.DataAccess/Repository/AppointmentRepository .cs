using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly EVMDealerSystemContext _context;

        public AppointmentRepository(EVMDealerSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Vehicle)
                .Include(a => a.DealerStaff)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(Guid id)
        {
            return await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Vehicle)
                .Include(a => a.DealerStaff)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _context.Appointments.FindAsync(id);
            if (existing != null)
            {
                _context.Appointments.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Appointment>> GetByStaffIdAsync(Guid staffId)
        {
            return await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Vehicle)
                .Where(a => a.DealerStaffId == staffId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.Appointments
                .Include(a => a.Vehicle)
                .Include(a => a.DealerStaff)
                .Where(a => a.CustomerId == customerId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }
    }
}