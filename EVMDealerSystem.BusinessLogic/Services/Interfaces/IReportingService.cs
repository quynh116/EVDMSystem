using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services.Interfaces
{
    public interface IReportingService
    {
        Task<Result<ProfitChartResponse>> GetMonthlyProfitDataAsync(Guid userId);
        Task<Result<AllocationChartResponse>> GetMonthlyEvmAllocationDataAsync();
        Task<Result<ProfitChartResponse>> GetMonthlyAdminProfitDataAsync();
    }
}
