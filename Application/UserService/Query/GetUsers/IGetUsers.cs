using Application.Interfaces.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GetUsers
{
    public interface IGetUsers
    {
        public List<UserDto> Execute();
    }
    public class GetUsers : IGetUsers
    {
        private readonly IDataBaseContext _context;
        public GetUsers(IDataBaseContext context)
        {
            _context = context;
        }
        public List<UserDto> Execute()
        {
            var users = _context.Users.Select(u => new UserDto
            {
                UserName = u.UserName,
                Id = u.Id,
                ImgSrc = u.ImgSrc,
                LastName = u.LastName,
                Name = u.Name,
                Number = u.Number,
            }).ToList();
            return users;
        }
    }
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }
        public string UserName { get; set; }
        public string ImgSrc { get; set; }
        public int purpose { get; set; }
    }
}
