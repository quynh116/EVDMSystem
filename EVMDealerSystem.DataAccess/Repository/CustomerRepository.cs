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
        public async Task<Customer?> GetByPhoneAsync(string phone)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Phone == phone);
        }
        public async Task<IEnumerable<Customer>> GetAllAsync() => await _context.Customers.ToListAsync();

        public async Task<Customer> AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var c = await _context.Customers.FindAsync(id);
            if (c == null) return false;
            _context.Customers.Remove(c);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}