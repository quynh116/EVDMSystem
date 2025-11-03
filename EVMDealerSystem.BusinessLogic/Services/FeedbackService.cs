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
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repo;
        public FeedbackService(IFeedbackRepository repo) { _repo = repo; }

        public async Task<Result<FeedbackResponse>> CreateAsync(FeedbackCreateRequest request, Guid customerId)
        {
            var fb = new Feedback
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                OrderId = request.OrderId,
                Subject = request.Subject,
                Content = request.Content,
                FeedbackType = request.FeedbackType,
                Status = "open",
                CreatedAt = DateTime.UtcNow,
                Note = request.Rating?.ToString()
            };

            var created = await _repo.AddAsync(fb);
            return Result<FeedbackResponse>.Success(Map(created), "Feedback created");
        }

        public async Task<Result<FeedbackResponse>> GetByIdAsync(Guid id)
        {
            var f = await _repo.GetByIdAsync(id);
            if (f == null) return Result<FeedbackResponse>.NotFound("Feedback not found");
            return Result<FeedbackResponse>.Success(Map(f));
        }

        public async Task<Result<IEnumerable<FeedbackResponse>>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            var mapped = list.Select(Map).ToList();
            return Result<IEnumerable<FeedbackResponse>>.Success(mapped);
        }

        public async Task<Result<FeedbackResponse>> UpdateAsync(Guid id, FeedbackUpdateRequest request)
        {
            var f = await _repo.GetByIdAsync(id);
            if (f == null) return Result<FeedbackResponse>.NotFound("Feedback not found");

            f.Subject = request.Subject ?? f.Subject;
            f.Content = request.Content ?? f.Content;
            f.FeedbackType = request.FeedbackType ?? f.FeedbackType;
            f.Status = request.Status ?? f.Status;
            f.Note = request.Note ?? f.Note;
            f.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(f);
            return Result<FeedbackResponse>.Success(Map(updated), "Feedback updated");
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            var ok = await _repo.DeleteAsync(id);
            if (!ok) return Result<bool>.NotFound("Feedback not found");
            return Result<bool>.Success(true, "Feedback deleted");
        }

        public async Task<Result<IEnumerable<FeedbackResponse>>> GetByOrderIdAsync(Guid orderId)
        {
            var list = await _repo.GetByOrderIdAsync(orderId);
            var mapped = list.Select(Map).ToList();
            return Result<IEnumerable<FeedbackResponse>>.Success(mapped);
        }

        private FeedbackResponse Map(Feedback f) => new FeedbackResponse
        {
            Id = f.Id,
            CustomerId = f.CustomerId,
            OrderId = f.OrderId,
            Subject = f.Subject,
            Content = f.Content,
            FeedbackType = f.FeedbackType,
            Status = f.Status,
            CreatedAt = f.CreatedAt,
            UpdatedAt = f.UpdatedAt,
            Note = f.Note
        };
    }
}
