using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVMDealerSystem.DataAccess.Models;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task<Order> AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Order>> GetByStaffIdAsync(Guid staffId);
        Task<IEnumerable<Order>> GetByDealerIdAsync(Guid dealerId);
        Task<int> CountTodayOrdersByDealerAsync(Guid dealerId);
    }
}
