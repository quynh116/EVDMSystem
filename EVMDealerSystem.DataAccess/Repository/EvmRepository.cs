using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository
{
    public class EvmRepository : IEvmRepository
    {
        private readonly EVMDealerSystemContext _context;

        public EvmRepository(EVMDealerSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Evm>> GetAllEvmsAsync()
        {
            return await _context.Evms.ToListAsync();
        }

        public async Task<Evm?> GetEvmByIdAsync(Guid id)
        {
            return await _context.Evms
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Evm> AddEvmAsync(Evm evm)
        {
            await _context.Evms.AddAsync(evm);
            await _context.SaveChangesAsync();
            return evm;
        }

        public async Task UpdateEvmAsync(Evm evm)
        {
            _context.Evms.Update(evm);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEvmAsync(Guid id)
        {
            var evm = await _context.Evms.FindAsync(id);
            if (evm != null)
            {
                _context.Evms.Remove(evm);
                await _context.SaveChangesAsync();
            }
        }

    }
}
