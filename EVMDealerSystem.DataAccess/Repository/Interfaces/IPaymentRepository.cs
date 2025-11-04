using EVMDealerSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> AddAsync(Payment payment);
        Task<Payment?> GetByIdAsync(Guid id);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment> UpdateAsync(Payment payment);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid orderId);
    }
}
