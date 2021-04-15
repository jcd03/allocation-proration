using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllocationProration.Models
{
    public class AllocationViewModel
    {
        public decimal TotalAvailableAllocation { get; set; }
        public List<InvestorInfo> InvestorInfos { get; set; } = new List<InvestorInfo>();
    }
}
