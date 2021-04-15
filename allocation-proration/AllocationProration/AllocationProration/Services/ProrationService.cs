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

        public List<InvestmentProrationResult> Prorate(AllocationViewModel model)
        {
            // Situations to check.
            // If there is enough allocated money for all investers then return.
            decimal? totalRequestedAmount = model.InvestorInfos.Sum(x => x.RequestAmount);

            if (totalRequestedAmount <= model.TotalAvailableAllocation)
            {
                return model.InvestorInfos.Select(x=> new InvestmentProrationResult
                {
                    Amount = x.RequestAmount,
                    Name = x.Name
                }).ToList();
            }

            List<InvestmentProrationResult> resultList = new List<InvestmentProrationResult>();

            // If a user requests less than what they have invested then they should get that total amount.
            List<InvestorInfo> investorsThatRequestLessThanAverage = model.InvestorInfos.Where(x => x.RequestAmount <= x.AveragAmount).ToList();
            foreach(var investor in investorsThatRequestLessThanAverage)
            {
                resultList.Add(new InvestmentProrationResult()
                {
                    Amount = investor.RequestAmount,
                    Name = investor.Name
                });

                model.TotalAvailableAllocation = (decimal)(model.TotalAvailableAllocation - investor.RequestAmount);
                model.InvestorInfos.Remove(investor);
            }

            // Final amount should not be greater than what was requested
            decimal? averageInvestmentSum = model.InvestorInfos.Sum(x => x.AveragAmount);

            foreach(var investor in model.InvestorInfos)
            {
                resultList.Add(new InvestmentProrationResult()
                {
                    Amount = (model.TotalAvailableAllocation * (investor.AveragAmount) / (averageInvestmentSum)),
                    Name = investor.Name
                });
            }

            return resultList;
        }
    }
}
