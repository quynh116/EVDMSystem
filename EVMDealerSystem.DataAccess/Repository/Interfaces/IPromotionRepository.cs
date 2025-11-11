using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVMDealerSystem.DataAccess.Models;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IPromotionRepository
    {
        Task<Promotion?> GetByIdAsync(Guid id);
        //Task<Promotion?> GetActiveByVehicleIdAsync(Guid vehicleId);
        Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
        Task<Promotion?> GetPromotionByIdAsync(Guid id);
        Task<Promotion> AddPromotionAsync(Promotion promotion);
        Task<Promotion> UpdatePromotionAsync(Promotion promotion);
        Task DeletePromotionAsync(Guid id);
        Task<IEnumerable<Promotion>> GetActivePromotionsByVehicleIdAsync(Guid vehicleId, Guid dealerId);

        // THÊM: Hàm mới để lấy Promotion theo DealerId (lấy từ User.DealerId)
        Task<IEnumerable<Promotion>> GetPromotionsByDealerIdAsync(Guid dealerId);

        // THÊM: Hàm mới để liên kết Promotion với Vehicle (Áp dụng giảm giá)
        Task ApplyPromotionToVehicleAsync(Guid promotionId, Guid vehicleId);

        // THÊM: Hàm mới để hủy liên kết Promotion với Vehicle (Hủy áp dụng giảm giá)
        Task RemovePromotionFromVehicleAsync(Guid promotionId, Guid vehicleId);

        // THÊM: Hàm mới để lấy Promotion theo ID, BAO GỒM danh sách Vehicles đã áp dụng
        Task<Promotion?> GetPromotionWithVehiclesByIdAsync(Guid id);
    }
}
