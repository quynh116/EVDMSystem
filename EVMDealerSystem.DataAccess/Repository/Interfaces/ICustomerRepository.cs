using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EVMDealerSystem.DataAccess.Models;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid id);
        Task<IEnumerable<Customer>> GetByStaffIdAsync(Guid staffId);
    }
}
