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
    public class OrderRepository : IOrderRepository
    {
        private readonly EVMDealerSystemContext _context;

        public OrderRepository(EVMDealerSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .Include(o => o.Dealer)
                .Include(o => o.DealerStaff)
                .Include(o => o.Inventory)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .Include(o => o.Dealer)
                .Include(o => o.DealerStaff)
                .Include(o => o.Inventory)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _context.Orders.FindAsync(id);
            if (existing != null)
            {
                _context.Orders.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetByStaffIdAsync(Guid staffId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Vehicle)
                .Include(o => o.Dealer)
                .Include(o => o.Inventory)
                .Where(o => o.DealerStaffId == staffId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> CountTodayOrdersByDealerAsync(Guid dealerId)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _context.Orders
                .Where(o => o.DealerId == dealerId &&
                           o.CreatedAt >= today &&
                           o.CreatedAt < tomorrow)
                .CountAsync();
        }
    }
}