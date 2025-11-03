using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly EVMDealerSystemContext _context;
        public PaymentRepository(EVMDealerSystemContext context) { _context = context; }

        public async Task<Payment> AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment?> GetByIdAsync(Guid id)
            => await _context.Payments.FindAsync(id);

        public async Task<IEnumerable<Payment>> GetAllAsync()
            => await _context.Payments.OrderByDescending(p => p.TransactionDate).ToListAsync();

        public async Task<Payment> UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var p = await _context.Payments.FindAsync(id);
            if (p == null) return false;
            _context.Payments.Remove(p);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid orderId)
            => await _context.Payments.Where(p => p.OrderId == orderId).ToListAsync();
    }
}
