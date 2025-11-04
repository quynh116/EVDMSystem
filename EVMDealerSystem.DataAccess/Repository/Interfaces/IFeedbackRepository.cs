using EVMDealerSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<Feedback> AddAsync(Feedback feedback);
        Task<Feedback?> GetByIdAsync(Guid id);
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task<Feedback> UpdateAsync(Feedback feedback);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Feedback>> GetByOrderIdAsync(Guid orderId);
    }
}
