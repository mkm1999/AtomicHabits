using Application.Interfaces.Context;
using CommonProject.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ReportsService.Command.AddReportService
{
    public interface IAddReport
    {
        public ResultDto<Double> Execute(RequestAddReportDto request);
    }

    public class AddReport : IAddReport
    {
        private readonly IDataBaseContext _context;

        public AddReport(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<double> Execute(RequestAddReportDto request)
        {
            if(request.EndTime < request.BeginTime)
            {
                return new ResultDto<double>
                {
                    Message = "زمان شروع باید قبل از زمان پایان باشد",
                    IsSuccess = false,
                };
            }
            if(request.UserId == 0)
            {
                return new ResultDto<double>
                {
                    Message = "کاربر یافت نشد",
                    IsSuccess = false
                };
            }
            if (request.Income == 0)
            {
                return new ResultDto<double>
                {
                    Message = "درآمد را وارد کنید",
                    IsSuccess = false
                };
            }
            if(request.BeginTime == DateTime.MinValue)
            {
                return new ResultDto<double>
                {
                    Message = "ساعت شروع سفر را وارد کنید",
                    IsSuccess = false
                };
            }
            if(request.EndTime == DateTime.MinValue)
            {
                return new ResultDto<double>
                {
                    Message = "ساعت پایان سفر را وارد کنید",
                    IsSuccess = false
                };
            }
            if (request.IsLast)
            {
                if (request.TodayNavigatedPerKilometers == 0)
                {
                    return new ResultDto<double>
                    {
                        Message = "پیمایش امروز را وارد کنید",
                        IsSuccess = false
                    };
                }
            }
            var user = _context.Users.Include(u => u.Reports).Where(u => u.Id == request.UserId).FirstOrDefault();

            var report = new Report
            {
                BeginTime = request.BeginTime,
                EndTime = request.EndTime,
                Income = request.Income,
                SubmitTime = DateTime.Now,
                TodayCosts = request.TodayCosts,
                TodayNavigatedPerKilometers = request.TodayNavigatedPerKilometers,
                User = user,
            };
            var TodayCompeleteTravelCount = user.Reports.Where(r => r.SubmitTime.Day == DateTime.Now.Day).Count();

            TodayCompeleteTravelCount++;
            Double CompletedPercent = Math.Round(((float)TodayCompeleteTravelCount / (float)user.Purpose) * 100);

            _context.Reports.Add(report);
            _context.SaveChanges();
            return new ResultDto<Double>
            {
                Data = CompletedPercent,
                IsSuccess = true,
                Message = "گزارش شما با موفقیت ثبت شد"
            };

        }
    }

    public class RequestAddReportDto
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsLast { get; set; }
        public int Income { get; set; }
        public int? TodayCosts { get; set; }
        public float? TodayNavigatedPerKilometers { get; set; }
        public int UserId { get; set; }
    }
}
