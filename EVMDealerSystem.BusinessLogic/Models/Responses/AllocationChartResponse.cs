using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Responses
{
    public class AllocationChartResponse
    {
        public IEnumerable<MonthlyAllocationData> Data { get; set; }
    }
    public class MonthlyAllocationData
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int? AllocatedCount { get; set; }
    }
}
