using Application.Interfaces.Context;
using CommonProject.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
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
        ResultDto RemoveToDo(int Id);
        ResultDto<ToDoDto> GetToDo(int Id);
        ResultDto EditToDo(RequestEditTodoDto request);
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

        public ResultDto EditToDo(RequestEditTodoDto request)
        {
            var ToDo = _context.ToDos.Find(request.Id);
            if(ToDo == null)
            {
                return new ResultDto
                {
                    Message = "todoیافت نشد.",
                    IsSuccess = false
                };
            }
            if(! string.IsNullOrEmpty(request.Title))
            {
                ToDo.Title = request.Title;
            }

            if (!string.IsNullOrEmpty(request.Type))
            {
                ToDo.Type = Enum.Parse<ToDoType>(request.Type);
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                ToDo.Status = Enum.Parse<Status>(request.Status);
            }

            if (request.DateTime != default(DateTime) && request.DateTime != null)
            {
                ToDo.DateTime = request.DateTime.Value;
            }
            _context.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true,
                Message = "با موفقیت ویرایش شد"
            };
        }

        public ResultDto<List<ToDoDto>> GetSpecificDayToDos(DateOnly date, int UserId)
        {
            var user = _context.Users.Where(u => u.Id == UserId).Include(u => u.ToDos).SingleOrDefault();
            if (user == null)
            {
                return new ResultDto<List<ToDoDto>>
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }
            var ToDos = user.ToDos.Where(t => DateOnly.Parse(t.DateTime.ToShortDateString()) == date).Select(t => new ToDoDto
            {
                Status = t.Status.ToString(),
                Title = t.Title,
                Type = t.Type.ToString(),
                Id = t.Id,
            }).ToList();
            return new ResultDto<List<ToDoDto>>
            {
                IsSuccess = true,
                Data = ToDos
            };

        }

        public ResultDto<ToDoDto> GetToDo(int Id)
        {
            var ToDo = _context.ToDos.Find(Id);
            if(ToDo == null)
            {
                return new ResultDto<ToDoDto>
                {
                    IsSuccess = false,
                    Message = "یافت نشد"
                };
            }
            var returnToDo = new ToDoDto
            {
                Id = Id,
                Title = ToDo.Title,
                Type = ToDo.Type.ToString(),
                Status = ToDo.Status.ToString(),
            };
            return new ResultDto<ToDoDto>
            {
                IsSuccess = true,
                Data = returnToDo
            };
        }

        public ResultDto RemoveToDo(int Id)
        {
            var ToDo = _context.ToDos.Find(Id);
            if(ToDo == null)
            {
                return new ResultDto
                {
                    Message = "todoیافت نشد.",
                    IsSuccess = false
                };
            }
            _context.ToDos.Remove(ToDo);
            _context.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true,
                Message = "todo مورد نظر حذف شد"
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
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public List<Link> Links { get; set; }
    }

    public class RequestEditTodoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? DateTime { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }

    public class Link
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string Method { get; set; }
    }
}
