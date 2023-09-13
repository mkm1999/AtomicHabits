using Application.ReportsService.Command.AddReportService;
using Application.ReportsService.Query.GetReportService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Ui.Models.Dto;

namespace Ui.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IAddReport _addReport;
        private readonly IGetReport _getReports;
        private readonly IGetDaysReport _getDaysReport;
        public ReportController(IAddReport addReport , IGetReport getReport, IGetDaysReport getDaysReport)
        {
            _addReport = addReport;
            _getReports = getReport;
            _getDaysReport = getDaysReport;
        }
        public IActionResult Index(string StartDate = "1400/01/01" , string EndDate = "1500/01/01")
        {
            PersianCalendar pc = new PersianCalendar();

            //Set Start Date
            if (StartDate == null) StartDate = "1400/01/01";
            var StartDateVar = StartDate.Split("/");
            int StartDateYear = int.Parse(StartDateVar[0]);
            int StartDateMonth = int.Parse(StartDateVar[1]);
            int StartDateDay = int.Parse(StartDateVar[2]);
            DateTime SD = pc.ToDateTime(StartDateYear, StartDateMonth, StartDateDay, 0,0,0,0);
            //

            //Set End Date
            if (EndDate == null) EndDate = "1500/01/01";
            var EndDateVar = EndDate.Split("/");
            int EndDateYear = int.Parse(EndDateVar[0]);
            int EndDateMonth = int.Parse(EndDateVar[1]);
            int EndDateDay = int.Parse(EndDateVar[2]);
            DateTime ED = pc.ToDateTime(EndDateYear, EndDateMonth, EndDateDay, 0, 0, 0, 0);
            //


            int UserId = 0;
            if (User.Identity.IsAuthenticated) UserId = int.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier.ToString()).FirstOrDefault().Value);

            var result = _getReports.Execute(SD,ED,UserId);
            return View(result);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add([FromBody]AddRequestViewModel request)
        {
            bool IsLast = bool.Parse(request.IsLast);
            int Costs = 0;
            int Income = 0;
            int Kilometers = 0;
            int UserId = 0;
            if (User.Identity.IsAuthenticated) UserId = int.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier.ToString()).FirstOrDefault().Value);
            int.TryParse(request.TodayCosts , out Costs);
            int.TryParse(request.TodayNavigatedPerKilometers , out Kilometers);
            int.TryParse(request.Income, out Income);

            var result = _addReport.Execute(new RequestAddReportDto
            {
                BeginTime = DateTime.Parse(request.BeginTime),
                IsLast = IsLast,
                EndTime = DateTime.Parse(request.EndTime),
                Income = Income,
                TodayCosts = Costs,
                UserId = UserId,
                TodayNavigatedPerKilometers = Kilometers,
            });
            return Json(result);
        }
        public IActionResult DayReports(string StartDate = "1400/01/01", string EndDate = "1500/01/01")
        {
            PersianCalendar pc = new PersianCalendar();

            //Set Start Date
            if (StartDate == null) StartDate = "1400/01/01";
            var StartDateVar = StartDate.Split("/");
            int StartDateYear = int.Parse(StartDateVar[0]);
            int StartDateMonth = int.Parse(StartDateVar[1]);
            int StartDateDay = int.Parse(StartDateVar[2]);
            DateTime SD = pc.ToDateTime(StartDateYear, StartDateMonth, StartDateDay, 0, 0, 0, 0);
            //

            //Set End Date
            if (EndDate == null) EndDate = "1500/01/01";
            var EndDateVar = EndDate.Split("/");
            int EndDateYear = int.Parse(EndDateVar[0]);
            int EndDateMonth = int.Parse(EndDateVar[1]);
            int EndDateDay = int.Parse(EndDateVar[2]);
            DateTime ED = pc.ToDateTime(EndDateYear, EndDateMonth, EndDateDay, 0, 0, 0, 0);
            //


            int UserId = 0;
            if (User.Identity.IsAuthenticated) UserId = int.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier.ToString()).FirstOrDefault().Value);

            var result = _getDaysReport.Execute(SD, ED, UserId);
            return View(result);
        }

    }
}
