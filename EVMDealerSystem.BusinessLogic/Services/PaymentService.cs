using EVMDealerSystem.BusinessLogic.Models.Request;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using EVMDealerSystem.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;
        public PaymentService(IPaymentRepository repo) { _repo = repo; }

        public async Task<Result<PaymentResponse>> CreateAsync(PaymentCreateRequest request)
        {
            var p = new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                TransactionDate = request.TransactionDate ?? TimeHelper.GetVietNamTime(),
                Note = request.Note
            };

            var created = await _repo.AddAsync(p);
            var resp = Map(created);
            return Result<PaymentResponse>.Success(resp, "Payment created");
        }

        public async Task<Result<PaymentResponse>> GetByIdAsync(Guid id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return Result<PaymentResponse>.NotFound("Payment not found");
            return Result<PaymentResponse>.Success(Map(p));
        }

        public async Task<Result<IEnumerable<PaymentResponse>>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            var mapped = list.Select(Map).ToList();
            return Result<IEnumerable<PaymentResponse>>.Success(mapped);
        }

        public async Task<Result<PaymentResponse>> UpdateAsync(Guid id, PaymentUpdateRequest request)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return Result<PaymentResponse>.NotFound("Payment not found");

            p.Amount = request.Amount ?? p.Amount;
            p.PaymentMethod = request.PaymentMethod ?? p.PaymentMethod;
            p.TransactionDate = request.TransactionDate ?? p.TransactionDate;
            p.Note = request.Note ?? p.Note;

            var updated = await _repo.UpdateAsync(p);
            return Result<PaymentResponse>.Success(Map(updated), "Payment updated");
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            var ok = await _repo.DeleteAsync(id);
            if (!ok) return Result<bool>.NotFound("Payment not found");
            return Result<bool>.Success(true, "Payment deleted");
        }

        public async Task<Result<IEnumerable<PaymentResponse>>> GetByOrderIdAsync(Guid orderId)
        {
            var list = await _repo.GetByOrderIdAsync(orderId);
            var mapped = list.Select(Map).ToList();
            return Result<IEnumerable<PaymentResponse>>.Success(mapped);
        }

        private PaymentResponse Map(Payment p) => new PaymentResponse
        {
            Id = p.Id,
            OrderId = p.OrderId,
            Amount = p.Amount,
            PaymentMethod = p.PaymentMethod,
            TransactionDate = p.TransactionDate,
            Note = p.Note
        };
    }
}
