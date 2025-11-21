using EVMDealerSystem.BusinessLogic.Commons;
using EVMDealerSystem.BusinessLogic.Models.Responses;
using EVMDealerSystem.BusinessLogic.Services.Interfaces;
using EVMDealerSystem.DataAccess.Repository;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Services
{
    public class ReportingService : IReportingService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public ReportingService(IOrderRepository orderRepository, IUserRepository userRepository, IInventoryRepository inventoryRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Result<ProfitChartResponse>> GetMonthlyAdminProfitDataAsync()
        {
            var today = TimeHelper.GetVietNamTime();
            var startDate = today.AddMonths(-11);
            startDate = new DateTime(startDate.Year, startDate.Month, 1);

            var orders = await _orderRepository.GetMonthlyAdminSalesDataAsync(startDate);

            var monthlyData = orders
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .Select(g => new MonthlyProfitData
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(o => o.TotalPrice), 
                    Cost = g.Sum(o => o.Vehicle.BasePrice), 
                                                            
                    Profit = g.Sum(o => o.TotalPrice - o.Vehicle.BasePrice)
                })
                .ToDictionary(d => (d.Year, d.Month));

            var resultList = new List<MonthlyProfitData>();
            for (int i = 0; i < 12; i++)
            {
                var currentMonth = startDate.AddMonths(i);
                var yearMonthKey = (currentMonth.Year, currentMonth.Month);

                if (monthlyData.TryGetValue(yearMonthKey, out var data))
                {
                    resultList.Add(data);
                }
                else
                {
                    resultList.Add(new MonthlyProfitData
                    {
                        Year = currentMonth.Year,
                        Month = currentMonth.Month,
                        Revenue = null,
                        Cost = null,
                        Profit = null
                    });
                }
            }

            return Result<ProfitChartResponse>.Success(new ProfitChartResponse { Data = resultList });
        }

        public async Task<Result<AllocationChartResponse>> GetMonthlyEvmAllocationDataAsync()
        {
            var today = TimeHelper.GetVietNamTime();
            var startDate = today.AddMonths(-11);
            startDate = new DateTime(startDate.Year, startDate.Month, 1);

            var inventories = await _inventoryRepository.GetMonthlyAllocatedInventoryAsync(startDate);

            var monthlyData = inventories
                .GroupBy(i => new { i.CreatedAt.Year, i.CreatedAt.Month })
                .Select(g => new MonthlyAllocationData
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    AllocatedCount = g.Count() 
                })
                .ToDictionary(d => (d.Year, d.Month));

            var resultList = new List<MonthlyAllocationData>();

            for (int i = 0; i < 12; i++)
            {
                var currentMonth = startDate.AddMonths(i);
                var yearMonthKey = (currentMonth.Year, currentMonth.Month);

                if (monthlyData.TryGetValue(yearMonthKey, out var data))
                {
                    resultList.Add(data); 
                }
                else
                {
                    resultList.Add(new MonthlyAllocationData
                    {
                        Year = currentMonth.Year,
                        Month = currentMonth.Month,
                        AllocatedCount = null
                    });
                }
            }

            return Result<AllocationChartResponse>.Success(new AllocationChartResponse { Data = resultList });
        }

        public async Task<Result<ProfitChartResponse>> GetMonthlyProfitDataAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || !user.DealerId.HasValue)
            {
                return Result<ProfitChartResponse>.NotFound("User is not associated with a Dealer.");
            }
            Guid dealerId = user.DealerId.Value;

            var today = TimeHelper.GetVietNamTime();
            var startDate = today.AddMonths(-11);
            startDate = new DateTime(startDate.Year, startDate.Month, 1);

            var orders = await _orderRepository.GetMonthlySalesDataAsync(dealerId, startDate);

            var monthlyData = orders
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .Select(g => new MonthlyProfitData
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(o => o.TotalPrice), 
                    Cost = g.Sum(o => o.Vehicle.BasePrice), 
                                                            
                    Profit = g.Sum(o => o.TotalPrice - o.Vehicle.BasePrice)
                })
                .ToDictionary(d => (d.Year, d.Month));

            var resultList = new List<MonthlyProfitData>();

            for (int i = 0; i < 12; i++)
            {
                var currentMonth = startDate.AddMonths(i);
                var yearMonthKey = (currentMonth.Year, currentMonth.Month);

                if (monthlyData.TryGetValue(yearMonthKey, out var data))
                {
                    resultList.Add(data); 
                }
                else
                {
                    resultList.Add(new MonthlyProfitData
                    {
                        Year = currentMonth.Year,
                        Month = currentMonth.Month,
                        Revenue = null,
                        Cost = null,
                        Profit = null
                    });
                }
            }

            return Result<ProfitChartResponse>.Success(new ProfitChartResponse { Data = resultList });
        }
    }
}
