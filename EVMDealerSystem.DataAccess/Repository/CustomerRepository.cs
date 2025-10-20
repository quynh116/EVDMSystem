using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly EVMDealerSystemContext _context;

        public CustomerRepository(EVMDealerSystemContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _context.Customers
                .Include(c => c.DealerStaff)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetByStaffIdAsync(Guid staffId)
        {
            return await _context.Customers
                .Where(c => c.DealerStaffId == staffId)
                .ToListAsync();
        }
    }
}