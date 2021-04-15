using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllocationProration.Models;

namespace AllocationProration.Services
{
    public class ProrationService : IProrationService
    {
        public ProrationService()
        {
        }

        /// <summary>
        /// This function will check if there is enough allocated money. If there is, it will return 
        /// with all the investers fully funded. If not, we will calculate the proration. After the
        /// initial calculation each result is checked to make sure that the investor isn't receiving 
        /// more than requested.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<InvestorInfo> Prorate(AllocationViewModel model)
        {
            // If there is enough allocated money for all investers then return.
            if (model.InvestorInfos.Sum(x => x.RequestAmount) <= model.TotalAvailableAllocation)
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
                // Calculate the average investment sum then set the prorated amount.
                decimal? averageInvestmentSum = model.InvestorInfos.Sum(x => x.AveragAmount);

                foreach (var investor in model.InvestorInfos)
                {
                    investor.ProratedAmount = (model.TotalAvailableAllocation * (investor.AveragAmount) / (averageInvestmentSum));
                }

                isDone = calculateProration(model, resultList);
            }

            return resultList.Concat(model.InvestorInfos).ToList();
        }

        /// <summary>
        /// Helper function to determine if the prorated amout is greater than the requested amount.
        /// If the prorated amount is greater, we set that value to the requested amount and move it
        /// to a temporary list. Then begin the caluclation process over again with the remaining list.
        /// </summary>
        /// <param name="model">Object used in parent function that is used to check prorated amount.</param>
        /// <param name="infoList">Temporary list to hold InvestorInfo</param>
        /// <returns></returns>
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
