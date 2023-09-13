using Application.Interfaces.Context;
using CommonProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserService.Command.Deleate
{
    public interface IDeleateUser
    {
        public ResultDto Execute(int Id);
    }
    public class DeleateUser : IDeleateUser
    {
        private readonly IDataBaseContext _context;
        public DeleateUser(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(int Id)
        {
            var User = _context.Users.Find(Id);
            if (User == null)
            {
                return new ResultDto { 
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }
            _context.Users.Remove(User);
            _context.SaveChanges();
            return new ResultDto
            {
                Message = "کاربر با موفقیت حذف شد",
                IsSuccess = true
            };
        }
    }
}
