using EVMDealerSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IEvmRepository
    {
        Task<IEnumerable<Evm>> GetAllEvmsAsync();
        Task<Evm?> GetEvmByIdAsync(Guid id);
        Task<Evm> AddEvmAsync(Evm evm);
        Task UpdateEvmAsync(Evm evm);
        Task DeleteEvmAsync(Guid id);
    }
}
