using Application.ReportsService.Query.GetHomeDataService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ui.Models;

namespace Ui.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGetHomeData _getHomeData;

        public HomeController(ILogger<HomeController> logger , IGetHomeData getHomeData)
        {
            _logger = logger;
            _getHomeData = getHomeData;
        }

        public IActionResult Index()
        {
            int UserId = 0;
            if (User.Identity.IsAuthenticated) UserId = int.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier.ToString()).FirstOrDefault().Value);
            var Result = _getHomeData.Execute(UserId);
            return View(Result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
