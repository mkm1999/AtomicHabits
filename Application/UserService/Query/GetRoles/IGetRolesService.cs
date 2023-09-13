using Application.Interfaces.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserService.Query.GetRoles
{
    public interface IGetRolesService
    {
        public List<RoleDto> Execute();
    }
    public class GetRolesService : IGetRolesService
    {
        private readonly IDataBaseContext _context;
        public GetRolesService(IDataBaseContext context)
        {
            _context = context;
        }
        public List<RoleDto> Execute()
        {
            var roles = _context.Roles.Select(R => new RoleDto
            {
                Id = R.Id,
                Name = R.Name,
            }).ToList();
            return roles;
        }
    }

    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
