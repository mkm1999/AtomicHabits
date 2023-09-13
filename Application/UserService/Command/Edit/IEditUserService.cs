using Application.Interfaces.Context;
using CommonProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserService.Command.Edit
{
    public interface IEditUserService
    {
        public ResultDto Execute(int UserId , RequestEditUserDto request);
    }

    public class EditUserService : IEditUserService
    {
        private readonly IDataBaseContext _context;

        public EditUserService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(int UserId , RequestEditUserDto request)
        {
            var user = _context.Users.Find(UserId);
            if (user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }

            var role = _context.Roles.Find(request.RoleId);

            user.Number = request.Number;
            user.Name = request.Name;
            user.UserName = request.UserName;
            user.LastName = request.LastName;
            user.ImgSrc = request.ImgSrc;
            user.Role = role;
            var HashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Password = HashedPassword;

            _context.SaveChanges();
            return new ResultDto
            {
                Message = "کاربر با موفقیت ویرایش شد",
                IsSuccess = true,
            };
        }
    }
    public class RequestEditUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ImgSrc { get; set; }
        public int RoleId { get; set; }
    }
}
