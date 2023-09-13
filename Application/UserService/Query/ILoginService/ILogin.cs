using Application.Interfaces.Context;
using CommonProject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserService.Query.ILoginService
{
    public interface ILogin
    {
        public ResultDto<LoginUserDto> Execute(RequestLoginDto request);
    }

    public class Login : ILogin
    {
        private readonly IDataBaseContext _context;
        public Login(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<LoginUserDto> Execute(RequestLoginDto request)
        {
            var user = _context.Users.Include(u => u.Role).Where(u => u.UserName == request.UserName).FirstOrDefault();
            if (user == null)
            {
                return new ResultDto<LoginUserDto>
                {
                    Message = "کاربری با این نام کاربری یافت نشد",
                    IsSuccess = false
                };
            }
            bool Verifyed = BCrypt.Net.BCrypt.Verify(request.Password,user.Password);
            if (!Verifyed)
            {
                return new ResultDto<LoginUserDto>
                {
                    IsSuccess = false,
                    Message = "پسورد صحیح نیست"
                };
            }
            else
            {
                return new ResultDto<LoginUserDto>
                {
                    IsSuccess = true,
                    Message = "شما وارد شدید!",
                    Data = new LoginUserDto
                    {
                        UserName = user.UserName,
                        Id = user.Id,
                        role = user.Role.Name
                    }
                };
            }
        }
    }

    public class RequestLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class LoginUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string role { get; set; }
    }
}
