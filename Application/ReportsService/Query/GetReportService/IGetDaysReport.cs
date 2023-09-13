using Application.Interfaces.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ReportsService.Query.GetReportService
{
    public interface IGetDaysReport
    {
        public List<GetReportsDto> Execute(DateTime StartDate, DateTime EndDate, int UserId);

    }

    public class GetDaysReport : IGetDaysReport
    {
        private readonly IDataBaseContext _context;
        public GetDaysReport(IDataBaseContext context)
        {
            _context = context;
        }
        public List<GetReportsDto> Execute(DateTime StartDate, DateTime EndDate, int UserId)
        {
            if (UserId == 0)
            {
                return new List<GetReportsDto>();
            }

            var user = _context.Users.Find(UserId);

            var reports = _context.Reports.Where(r => r.BeginTime >= StartDate && r.EndTime <= EndDate && r.UserId == UserId).OrderByDescending(r => r.Id).AsEnumerable().GroupBy(r => r.SubmitTime.ToShortDateString());
            List<GetReportsDto> DayReportsList = new List<GetReportsDto>();

            int Iterator = 1;
            foreach (var day in reports)
            {
                int Id = Iterator;
                DateTime BeginTime = day.LastOrDefault().BeginTime;
                DateTime EndTime = day.FirstOrDefault().EndTime;
                int Income = day.Sum(i => i.Income); ;
                int? Costs = day.Sum(i => i.TodayCosts);
                float TodayNavigatedPerKilometers = day.Sum(i => i.TodayNavigatedPerKilometers).Value;
                int Profit = Income - Costs.Value;
                int WorkedTimeMinute = int.Parse((EndTime - BeginTime).TotalMinutes.ToString());
                float ProfitPerMinute = (float)Profit / (float)WorkedTimeMinute;
                double ArrivedGoalPercent = Math.Round(((float)day.Count() / (float)user.Purpose)*100);

                PersianCalendar persianCalendar = new PersianCalendar();

                DayReportsList.Add(new GetReportsDto
                {
                    Id = Id,
                    BeginTime = $"{persianCalendar.GetDayOfWeek(BeginTime)}, {persianCalendar.GetYear(BeginTime)}/{persianCalendar.GetMonth(BeginTime)}/{persianCalendar.GetDayOfMonth(BeginTime)}  {persianCalendar.GetHour(BeginTime)}:{persianCalendar.GetMinute(BeginTime)}",
                    Costs = Costs,
                    WorkedTimeMinute = WorkedTimeMinute,
                    ProfitPerMinute = ProfitPerMinute,
                    EndTime = $"{persianCalendar.GetDayOfWeek(EndTime)}, {persianCalendar.GetYear(EndTime)}/{persianCalendar.GetMonth(EndTime)}/{persianCalendar.GetDayOfMonth(EndTime)}  {persianCalendar.GetHour(EndTime)}:{persianCalendar.GetMinute(EndTime)}",
                    TodayNavigatedPerKilometers = TodayNavigatedPerKilometers,
                    Income = Income,
                    Profit = Profit,
                    PurposeArrivedPercent = ArrivedGoalPercent,
                });



                Iterator++;
            }
            return DayReportsList;
        }
    }
}
