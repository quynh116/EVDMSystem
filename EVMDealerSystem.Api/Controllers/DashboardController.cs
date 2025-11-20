using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVMDealerSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseApiController
    {
        private readonly IReportingService _reportingService;

        public DashboardController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        [HttpGet("monthly-profit")]
        public async Task<ActionResult<Result<ProfitChartResponse>>> GetMonthlyProfitData(Guid userId)
        {

            if (userId == Guid.Empty)
            {
                return Result<ProfitChartResponse>.Unauthorized("User must be authenticated.");
            }

            var result = await _reportingService.GetMonthlyProfitDataAsync(userId);
            return HandleResult(result);
        }

        [HttpGet("evm/monthly-allocation")]
        public async Task<ActionResult<Result<AllocationChartResponse>>> GetMonthlyEvmAllocationData()
        {
            var result = await _reportingService.GetMonthlyEvmAllocationDataAsync();
            return HandleResult(result);
        }

        [HttpGet("admin/monthly-profit")]
        public async Task<ActionResult<Result<ProfitChartResponse>>> GetMonthlyEvmProfitData()
        {
            var result = await _reportingService.GetMonthlyAdminProfitDataAsync();
            return HandleResult(result);
        }
    }
}
