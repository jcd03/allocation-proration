using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllocationProration.Services
{
    public class ProrationService
    {
        public ProrationService()
        {
        }

        public void Prorate(AllocationProration.Models.AllocationViewModel model)
        {
            decimal? averageInvestmentSum = model.InvestorInfos.Sum(x => x.AveragAmount);

            Console.WriteLine("average investment sum: {0}", averageInvestmentSum);

            foreach(var investor in model.InvestorInfos)
            {
                Console.WriteLine("{0}: ${1}",investor.Name, (model.TotalAvailableAllocation * (investor.AveragAmount) / (averageInvestmentSum)));
            }
        }
    }
}
