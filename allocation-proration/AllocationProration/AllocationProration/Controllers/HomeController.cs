using AllocationProration.Models;
using AllocationProration.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AllocationProration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new AllocationViewModel();
            model.InvestorInfos.Add(new InvestorInfo());
            model.InvestorInfos.Add(new InvestorInfo());
            model.InvestorInfos.Add(new InvestorInfo());

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(AllocationViewModel model)
        {
            if (!validateFormData(model))
                return View(model);

            ProrationService prorationService = new ProrationService();
            prorationService.Prorate(model);

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool validateFormData(AllocationViewModel model)
        {
            if(model.TotalAvailableAllocation == 0)
            {
                ModelState.AddModelError("error", "Total Available Allocation Required. Must be decimal value");
                return false;
            }

            foreach(var investorInfo in model.InvestorInfos)
            {
                if((investorInfo.RequestAmount == null && investorInfo.AveragAmount != null) || (investorInfo.RequestAmount != null && investorInfo.AveragAmount == null)) 
                {
                    ModelState.AddModelError("error", "One of the input values is either missing or is not a decimal value");
                    return false;
                }
            }
            if (ModelState.ContainsKey("error"))
                ModelState["error"].Errors.Clear();
            return true;
        }
    }
}
