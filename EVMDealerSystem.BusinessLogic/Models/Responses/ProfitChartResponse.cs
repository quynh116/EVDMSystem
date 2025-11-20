using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.BusinessLogic.Models.Responses
{
    public class ProfitChartResponse
    {
        public IEnumerable<MonthlyProfitData> Data { get; set; }
    }

    public class MonthlyProfitData
    {
        public int Month { get; set; } 
        public int Year { get; set; } 
        public decimal? Revenue { get; set; } 
        public decimal? Cost { get; set; }    
        public decimal? Profit { get; set; }  
    }
}
