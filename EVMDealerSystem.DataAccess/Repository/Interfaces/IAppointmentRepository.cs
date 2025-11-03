using EVMDealerSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment> AddAsync(Appointment appointment);
        Task<Appointment?> GetByIdAsync(Guid id);
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment> UpdateAsync(Appointment appointment);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Appointment>> GetByDealerIdAsync(Guid dealerStaffId);
        Task<IEnumerable<Appointment>> GetByVehicleAndDateAsync(Guid vehicleId, DateTime date);
        Task SaveChangesAsync();
    }
}
