using System;

namespace Application.ReportsService.Query.GetReportService
{
    public class GetReportsDto
    {
        public int Id { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public int Income { get; set; }
        public int? Costs { get; set; }
        public float TodayNavigatedPerKilometers { get; set; }
        public int Profit { get; set; }
        public int WorkedTimeMinute { get; set; }
        public float ProfitPerMinute { get; set; }
        public double? PurposeArrivedPercent { get; set; }

    }
}
