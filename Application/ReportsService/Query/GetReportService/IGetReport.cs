using Application.Interfaces.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ReportsService.Query.GetReportService
{
    public interface IGetReport
    {
        public List<GetReportsDto> Execute(DateTime StartDate , DateTime EndDate , int UserId);
    }

    public class GetReport : IGetReport
    {
        private readonly IDataBaseContext _context;
        public GetReport(IDataBaseContext context)
        {
            _context = context;
        }
        public List<GetReportsDto> Execute(DateTime StartDate, DateTime EndDate , int UserId)
        {
            if(UserId == 0)
            {
                return new List<GetReportsDto>();
            }
            PersianCalendar persianCalendar = new PersianCalendar();
            var reports = _context.Reports.Where(r => r.BeginTime >= StartDate && r.EndTime <= EndDate && r.UserId == UserId).OrderByDescending(r => r.Id).Select(r => new GetReportsDto
            {
                TodayNavigatedPerKilometers = r.TodayNavigatedPerKilometers.Value,
                Id = r.Id,
                Income = r.Income,
                BeginTime = $"{persianCalendar.GetDayOfWeek(r.BeginTime)}, {persianCalendar.GetYear(r.BeginTime)}/{persianCalendar.GetMonth(r.BeginTime)}/{persianCalendar.GetDayOfMonth(r.BeginTime)}  {persianCalendar.GetHour(r.BeginTime)}:{persianCalendar.GetMinute(r.BeginTime)}",
                EndTime = $"{persianCalendar.GetDayOfWeek(r.EndTime)}, {persianCalendar.GetYear(r.EndTime)}/{persianCalendar.GetMonth(r.EndTime)}/{persianCalendar.GetDayOfMonth(r.EndTime)}  {persianCalendar.GetHour(r.EndTime)}:{persianCalendar.GetMinute(r.EndTime)}",
                Costs = r.TodayCosts,
                Profit = r.Income - r.TodayCosts.Value,
                WorkedTimeMinute = int.Parse(((r.EndTime - r.BeginTime).TotalMinutes).ToString()),
                ProfitPerMinute = float.Parse((r.Income - r.TodayCosts.Value).ToString()) / (float.Parse(((r.EndTime - r.BeginTime).TotalMinutes).ToString()))
            }).ToList();
            return reports;
        }
    }
}
