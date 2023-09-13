using Application.ReportsService.Query.GetReportService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ReportsService.Query.GetHomeDataService
{
    public interface IGetHomeData
    {
        public GetHomeDataDto Execute(int UserId);
    }

    public class GetHomeData : IGetHomeData
    {
        private readonly IGetDaysReport _getDaysReport;

        public GetHomeData(IGetDaysReport getDaysReport)
        {
            _getDaysReport = getDaysReport;
        }

        public GetHomeDataDto Execute(int UserId)
        {
            var DaysResult = _getDaysReport.Execute(DateTime.MinValue , DateTime.MaxValue , UserId);

            int totalProfit = DaysResult.Sum(i => i.Profit);

            int workedDayCount = DaysResult.Count();

            int WorkedMinutes = DaysResult.Sum(i => i.WorkedTimeMinute);
            double AverageProfitPerMinute = Math.Round((float)totalProfit / (float)WorkedMinutes);

            double GoalReachingSum = DaysResult.Sum(i => i.PurposeArrivedPercent).Value;
            double AverageGoalReaching = Math.Round(GoalReachingSum / (float)workedDayCount);

            var LastFifteen = DaysResult.Take(15).ToList();


            //Goal
            var LastWeekReports = _getDaysReport.Execute(DateTime.Now.AddDays(-7), DateTime.Now, UserId);
            var LastMonthReports = _getDaysReport.Execute(DateTime.Now.AddDays(-30), DateTime.Now, UserId);

            int LastWeekCompeletedDaysCount = LastWeekReports.Where(i => i.PurposeArrivedPercent >= 100).Count();
            int LastWeekDaysCount = LastWeekReports.Count;
            double LastWeekCompeletedDaysPercent = Math.Round(((float)LastWeekCompeletedDaysCount / (float)LastWeekDaysCount)*100);

            int LastMonthCompeletedDaysCount = LastMonthReports.Where(i => i.PurposeArrivedPercent >= 100).Count();
            int LastMonthDaysCount = LastMonthReports.Count;
            double LastMonthCompeletedDaysPercent = Math.Round(((float)LastMonthCompeletedDaysCount / (float)LastMonthDaysCount) * 100);

            int CompeletedDaysCount = DaysResult.Where(i => i.PurposeArrivedPercent >= 100).Count();
            double CompeletedDaysPercent = Math.Round(((float)CompeletedDaysCount / (float)workedDayCount) * 100);

            var Result = new GetHomeDataDto
            {
                TotalProfit = totalProfit,
                WorkedDaysCount = workedDayCount,
                AverageProfitPerMinute = AverageProfitPerMinute,
                AverageGoalReaching = AverageGoalReaching,

                LastFifteenReports = LastFifteen,

                GoalDetails = new GoalDto
                {
                    TotalReachedGoalPercent = AverageGoalReaching,
                    LastWeekCompeletedDaysPercent = LastWeekCompeletedDaysPercent,
                    LastMonthCompeletedDaysPercent = LastMonthCompeletedDaysPercent,
                    TotalCompeletedDaysPercent = CompeletedDaysPercent
                }
            };

            return Result;

        }
    }

    public class GetHomeDataDto
    {
        public int TotalProfit { get; set; }
        public int WorkedDaysCount { get; set; }
        public double AverageProfitPerMinute { get; set; }
        public double AverageGoalReaching { get; set; }

        //middle line

        public List<GetReportsDto> LastFifteenReports { get; set; }
        public GoalDto GoalDetails { get; set; }

        //Third Line


    }

    public class GoalDto
    {
        public double TotalReachedGoalPercent { get; set; }
        public double LastWeekCompeletedDaysPercent { get; set; }
        public double LastMonthCompeletedDaysPercent { get; set; }
        public double TotalCompeletedDaysPercent { get; set; }

    }
}
