using EVMDealerSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IDealerRepository
    {
        Task<IEnumerable<Dealer>> GetAllDealersAsync();
        Task<Dealer?> GetDealerByIdAsync(Guid id);
        Task<Dealer> AddDealerAsync(Dealer dealer);
        Task UpdateDealerAsync(Dealer dealer);
        Task DeleteDealerAsync(Guid id);
    }
}
