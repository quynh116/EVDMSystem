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
    public class DealerRepository : IDealerRepository
    {
        private readonly EVMDealerSystemContext _context;

        public DealerRepository(EVMDealerSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dealer>> GetAllDealersAsync()
        {
            return await _context.Dealers.Include(d => d.Evm).ToListAsync();
        }

        public async Task<Dealer?> GetDealerByIdAsync(Guid id)
        {
            return await _context.Dealers
                .Include(d => d.Evm) 
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Dealer> AddDealerAsync(Dealer dealer)
        {
            await _context.Dealers.AddAsync(dealer);
            await _context.SaveChangesAsync();
            return dealer;
        }

        public async Task UpdateDealerAsync(Dealer dealer)
        {
            _context.Dealers.Update(dealer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDealerAsync(Guid id)
        {
            var dealer = await _context.Dealers.FindAsync(id);
            if (dealer != null)
            {
                _context.Dealers.Remove(dealer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
