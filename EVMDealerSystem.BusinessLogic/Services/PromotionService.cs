using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Request.Promotion;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserRepository _userRepository;

        public PromotionService(IPromotionRepository promotionRepository,
                                IVehicleRepository vehicleRepository,
                                IUserRepository userRepository)
        {
            _promotionRepository = promotionRepository;
            _vehicleRepository = vehicleRepository;
            _userRepository = userRepository;
        }

        // Mapping Helper
        private PromotionResponse MapToPromotionResponse(Promotion promotion)
        {
            return new PromotionResponse
            {
                Id = promotion.Id,
                CreatedBy = promotion.CreatedBy,
                CreatedByName = promotion.CreatedByNavigation.FullName, 
                Name = promotion.Name,
                Description = promotion.Description,
                DiscountType = promotion.DiscountType,
                DiscountPercent = promotion.DiscountPercent,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                IsActive = promotion.IsActive,
                CreatedAt = promotion.CreatedAt,
                UpdatedAt = promotion.UpdatedAt,
                Note = promotion.Note
            };
        }

        public async Task<Result<IEnumerable<PromotionResponse>>> GetAllPromotionsAsync()
        {
            try
            {
                var promotions = await _promotionRepository.GetAllPromotionsAsync();
                var responses = promotions.Select(MapToPromotionResponse).ToList();
                return Result<IEnumerable<PromotionResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PromotionResponse>>.InternalServerError($"Error retrieving promotions: {ex.Message}");
            }
        }

        public async Task<Result<PromotionResponse>> GetPromotionByIdAsync(Guid id)
        {
            try
            {
                var promotion = await _promotionRepository.GetPromotionByIdAsync(id);
                if (promotion == null)
                    return Result<PromotionResponse>.NotFound($"Promotion ID {id} not found.");

                return Result<PromotionResponse>.Success(MapToPromotionResponse(promotion));
            }
            catch (Exception ex)
            {
                return Result<PromotionResponse>.InternalServerError($"Error retrieving promotion: {ex.Message}");
            }
        }

        public async Task<Result<PromotionResponse>> CreatePromotionAsync(PromotionCreateRequest request)
        {
            try
            {

                if (await _userRepository.GetUserByIdAsync(request.CreatedBy) == null)
                    return Result<PromotionResponse>.NotFound($"User ID {request.CreatedBy} not found.");

                if (request.StartDate >= request.EndDate)
                    return Result<PromotionResponse>.Invalid("Start Date must be before End Date.");

                if (request.DiscountPercent == null || request.DiscountPercent < 0 || request.DiscountPercent > 100)
                    return Result<PromotionResponse>.Invalid("DiscountPercent is required and must be between 0 and 100 for Percent type.");

                var newPromotion = new Promotion
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = request.CreatedBy,
                    Name = request.Name,
                    Description = request.Description,
                    DiscountType = "Percent",
                    DiscountPercent = request.DiscountPercent,
                    StartDate = request.StartDate.ToUniversalTime(),
                    EndDate = request.EndDate.ToUniversalTime(),
                    IsActive = request.IsActive,
                    CreatedAt = DateTime.UtcNow,
                    Note = request.Note
                };

                var addedPromotion = await _promotionRepository.AddPromotionAsync(newPromotion);
                var resultPromotion = await _promotionRepository.GetPromotionByIdAsync(addedPromotion.Id);

                return Result<PromotionResponse>.Success(MapToPromotionResponse(resultPromotion!), "Promotion created successfully.");
            }
            catch (Exception ex)
            {
                return Result<PromotionResponse>.InternalServerError($"Error creating promotion: {ex.Message}");
            }
        }

        public async Task<Result<PromotionResponse>> UpdatePromotionAsync(Guid id, PromotionUpdateRequest request)
        {
            try
            {
                var existingPromotion = await _promotionRepository.GetPromotionByIdAsync(id);
                if (existingPromotion == null)
                    return Result<PromotionResponse>.NotFound($"Promotion ID {id} not found.");

                existingPromotion.Name = request.Name ?? existingPromotion.Name;
                existingPromotion.Description = request.Description ?? existingPromotion.Description;
                existingPromotion.DiscountPercent = request.DiscountPercent ?? existingPromotion.DiscountPercent;
                existingPromotion.StartDate = request.StartDate?.ToUniversalTime() ?? existingPromotion.StartDate;
                existingPromotion.EndDate = request.EndDate?.ToUniversalTime() ?? existingPromotion.EndDate;
                existingPromotion.IsActive = request.IsActive ?? existingPromotion.IsActive;
                existingPromotion.Note = request.Note ?? existingPromotion.Note;
                existingPromotion.UpdatedAt = DateTime.UtcNow;

                if (existingPromotion.StartDate >= existingPromotion.EndDate)
                    return Result<PromotionResponse>.Invalid("Start Date must be before End Date.");

                if (existingPromotion.DiscountType == "Percent" && (existingPromotion.DiscountPercent == null || existingPromotion.DiscountPercent < 0 || existingPromotion.DiscountPercent > 100))
                    return Result<PromotionResponse>.Invalid("DiscountPercent is required and must be between 0 and 100 for Percent type.");

                var updatedPromotion = await _promotionRepository.UpdatePromotionAsync(existingPromotion);

                var resultPromotion = await _promotionRepository.GetPromotionByIdAsync(updatedPromotion.Id);

                return Result<PromotionResponse>.Success(MapToPromotionResponse(resultPromotion!), "Promotion updated successfully.");
            }
            catch (Exception ex)
            {
                return Result<PromotionResponse>.InternalServerError($"Error updating promotion: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeletePromotionAsync(Guid id)
        {
            try
            {
                var promotion = await _promotionRepository.GetPromotionByIdAsync(id);
                if (promotion == null)
                    return Result<bool>.NotFound($"Promotion ID {id} not found.");

                await _promotionRepository.DeletePromotionAsync(id);
                return Result<bool>.Success(true, "Promotion deleted successfully.");
            }
            catch (Exception ex)
            {
                return Result<bool>.InternalServerError($"Error deleting promotion: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<PromotionResponse>>> GetDealerPromotionsAsync(Guid dealerId)
        {
            try
            {
                var promotions = await _promotionRepository.GetPromotionsByDealerIdAsync(dealerId);
                var responses = promotions.Select(MapToPromotionResponse).ToList();
                return Result<IEnumerable<PromotionResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PromotionResponse>>.InternalServerError($"Error retrieving dealer promotions: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ApplyPromotionToVehicleAsync(Guid promotionId, Guid vehicleId)
        {
            try
            {
                var promotion = await _promotionRepository.GetByIdAsync(promotionId);
                if (promotion == null)
                    return Result<bool>.NotFound($"Promotion ID {promotionId} not found.");

                var vehicle = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);
                if (vehicle == null)
                    return Result<bool>.NotFound($"Vehicle ID {vehicleId} not found.");

                await _promotionRepository.ApplyPromotionToVehicleAsync(promotionId, vehicleId);
                return Result<bool>.Success(true, "Promotion applied to vehicle successfully.");
            }
            catch (Exception ex)
            {
                return Result<bool>.InternalServerError($"Error applying promotion: {ex.Message}");
            }
        }

        public async Task<Result<bool>> RemovePromotionFromVehicleAsync(Guid promotionId, Guid vehicleId)
        {
            try
            {
                await _promotionRepository.RemovePromotionFromVehicleAsync(promotionId, vehicleId);
                return Result<bool>.Success(true, "Promotion removed from vehicle successfully.");
            }
            catch (Exception ex)
            {
                return Result<bool>.InternalServerError($"Error removing promotion: {ex.Message}");
            }
        }
    }
}
