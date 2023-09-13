using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime SubmitTime { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Income { get; set; }
        public int? TodayCosts { get; set; }
        public float? TodayNavigatedPerKilometers { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
