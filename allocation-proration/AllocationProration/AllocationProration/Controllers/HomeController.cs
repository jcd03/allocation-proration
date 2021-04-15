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
        private readonly IProrationService _prorationService;

        public HomeController(ILogger<HomeController> logger, IProrationService prorationService)
        {
            _logger = logger;
            _prorationService = prorationService;
        }

        public IActionResult Index()
        {
            var model = new AllocationViewModel();
            model.InvestorInfos.Add(new InvestorInfo());
            model.InvestorInfos.Add(new InvestorInfo());
            model.InvestorInfos.Add(new InvestorInfo());

            ViewData["results"] = new List<InvestorInfo>();

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(AllocationViewModel model)
        {
            if (!validateFormData(model))
            {
                ViewData["results"] = new List<InvestorInfo>();
                return View(model);
            }

            // Copies the 'model' into a new alocation model. This is so the view can display the original info since the 
            // proration service modifies the 'model' data.
            AllocationViewModel dataModel = new AllocationViewModel()
            {
                InvestorInfos = model.InvestorInfos.Where(x => x.AveragAmount != null && x.RequestAmount != null).ToList(),
                TotalAvailableAllocation = model.TotalAvailableAllocation
            };
            
            ViewData["results"] = _prorationService.Prorate(dataModel);

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Checks the form data to make sure the user inputs enough data to do the calculation.
        /// 
        /// This will make sure there is a Total Available Allocation number set as well as the RequestAmount
        /// and Average Amount.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
