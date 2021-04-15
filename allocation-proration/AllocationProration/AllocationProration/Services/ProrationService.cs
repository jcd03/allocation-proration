using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllocationProration.Models;

namespace AllocationProration.Services
{
    public class ProrationService
    {
        public ProrationService()
        {
        }

        public List<InvestorInfo> Prorate(AllocationViewModel model)
        {
            // If there is enough allocated money for all investers then return.
            decimal? totalRequestedAmount = model.InvestorInfos.Sum(x => x.RequestAmount);

            if (totalRequestedAmount <= model.TotalAvailableAllocation)
            {
                return model.InvestorInfos.Select(x => new InvestorInfo
                {
                    ProratedAmount = x.RequestAmount,
                    Name = x.Name
                }).ToList();
            }

            List<InvestorInfo> resultList = new List<InvestorInfo>();

            bool isDone = false;

            while (!isDone)
            {
                // Final amount should not be greater than what was requested
                decimal? averageInvestmentSum = model.InvestorInfos.Sum(x => x.AveragAmount);

                foreach (var investor in model.InvestorInfos)
                {
                    investor.ProratedAmount = (model.TotalAvailableAllocation * (investor.AveragAmount) / (averageInvestmentSum));
                }

                isDone = calculateProration(model, resultList);
            }

            resultList = resultList.Concat(model.InvestorInfos).ToList();

            return resultList;
        }

        private bool calculateProration(AllocationViewModel model, List<InvestorInfo> infoList)
        {
            foreach(InvestorInfo investorInfo in model.InvestorInfos)
            {
                if(investorInfo.ProratedAmount > investorInfo.RequestAmount)
                {
                    investorInfo.ProratedAmount = investorInfo.RequestAmount;
                    model.TotalAvailableAllocation = (decimal)(model.TotalAvailableAllocation - investorInfo.RequestAmount);
                    infoList.Add(investorInfo);
                    model.InvestorInfos.Remove(investorInfo);
                    return false;
                }
            }

            return true;
        }
    }
}
