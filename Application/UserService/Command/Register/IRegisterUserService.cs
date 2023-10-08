using Application.Interfaces.Context;
using CommonProject.Entities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserService.Command.Register
{
    public interface IRegisterUserService
    {
        public ResultDto<int> Execute(RequestRegisterUserDto request); 
    }
    public class RegisterUserService : IRegisterUserService
    {
        private readonly IDataBaseContext _context;
        public RegisterUserService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<int> Execute(RequestRegisterUserDto request)
        {
            if(string.IsNullOrEmpty(request.Number))
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "تلفن را وارد کنید"
                };
            }
            if(string.IsNullOrEmpty(request.Password))
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = " پسورد را وارد کنید"
                };
            }
            if(string.IsNullOrEmpty(request.Name))
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "نام را وارد کنید"
                };
            }
            if(string.IsNullOrEmpty(request.UserName))
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "نام کاربری را وارد کنید"
                };
            }
            if(string.IsNullOrEmpty(request.LastName))
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "نام خانوادگی  را وارد کنید"
                };
            }
            var role = _context.Roles.Find(2);
            var user = new User
            {
                Name = request.Name,
                ImgSrc = request.ImgSrc,
                LastName = request.LastName,
                Number = request.Number,
                Role = role,
                UserName = request.UserName,
            };
            var HashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Password = HashedPassword;
            _context.Users.Add(user);
            _context.SaveChanges();
            return new ResultDto<int>
            {
                IsSuccess = true,
                Message = "کاربر با موفقیت ثبت شد",
                Data = user.Id
            };

        }
    }
    public class RequestRegisterUserDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ImgSrc { get; set; }
        //public int RoleId { get; set; }
    }
}