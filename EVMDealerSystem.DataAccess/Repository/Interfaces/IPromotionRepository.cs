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
        Task<Promotion?> GetActiveByVehicleIdAsync(Guid vehicleId);
        Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
        Task<Promotion?> GetPromotionByIdAsync(Guid id);
        Task<Promotion> AddPromotionAsync(Promotion promotion);
        Task<Promotion> UpdatePromotionAsync(Promotion promotion);
        Task DeletePromotionAsync(Guid id);
    }
}
