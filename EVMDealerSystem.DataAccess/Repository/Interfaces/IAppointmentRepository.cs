using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EVMDealerSystem.DataAccess.Models;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(Guid id);
        Task<Appointment> AddAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Appointment>> GetByStaffIdAsync(Guid staffId);
        Task<IEnumerable<Appointment>> GetByCustomerIdAsync(Guid customerId);
    }
}
