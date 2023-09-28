using Application.Interfaces.Context;
using CommonProject.Entities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ToDoServices
{
    public interface IToDoService
    {
        ResultDto AddToDo(RequestAddTodoDto request);
        ResultDto<List<ToDoDto>> GetSpecificDayToDos(DateOnly date, int UserId);
    }

    public class ToDoService : IToDoService
    {
        private readonly IDataBaseContext _context;

        public ToDoService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto AddToDo(RequestAddTodoDto request)
        {
            var user = _context.Users.Find(request.UserId);
            if(user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }

            Status Status;
            if (!Enum.TryParse<Status>(request.Status,out Status))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "مقدار status صحیح نیست"
                };
            }
            ToDoType type;
            if (!Enum.TryParse<ToDoType>(request.Type, out type))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "مقدار type صحیح نیست"
                };
            }
            var ToDo = new ToDo
            {
                DateTime = request.DateTime,
                User = user,
                Status = Status,
                Title = request.Title,
                Type = type,
            };
            
            _context.ToDos.Add(ToDo);
            _context.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true,
                Message = "با موقیت ثبت شد"
            };
        }

        public ResultDto<List<ToDoDto>> GetSpecificDayToDos(DateOnly date, int UserId)
        {
            var user = _context.Users.Find(UserId);
            if (user == null)
            {
                return new ResultDto<List<ToDoDto>>
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }
            var ToDos = user.ToDos.Where(t => DateOnly.Parse(t.DateTime.ToString()) == date).Select(t => new ToDoDto
            {
                Status = t.Status.ToString(),
                Title = t.Title,
                Type = t.Title.ToString(),
            }).ToList();
            return new ResultDto<List<ToDoDto>>
            {
                IsSuccess = true,
                Data = ToDos
            };

        }
    }

    public class RequestAddTodoDto
    {
        public string Title { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
    public class ToDoDto
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
