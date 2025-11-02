using System.Threading.Tasks;
using EVMDealerSystem.DataAccess.Models;

namespace EVMDealerSystem.DataAccess.Repository.Interfaces
{
    public interface IFeedbackRepository
    {
        Task AddAsync(Feedback feedback);
        Task SaveChangesAsync();
    }
}
