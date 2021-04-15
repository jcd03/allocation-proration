using AllocationProration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllocationProration.Services
{
    public interface IProrationService
    {
        List<InvestorInfo> Prorate(AllocationViewModel model);
    }
}
