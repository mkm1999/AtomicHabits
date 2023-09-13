using Application.Interfaces.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserService.Command.EditPurposeService
{
    public interface IEditPurpose
    {
        public void Execute(int UserId, bool IsPlus);
    }

    public class EditPurpose : IEditPurpose
    {
        private readonly IDataBaseContext _context;

        public EditPurpose(IDataBaseContext context)
        {
            _context = context;
        }
        public void Execute(int UserId, bool IsPlus)
        {
            var user = _context.Users.Find(UserId);
            if (IsPlus)
            {
                user.Purpose++;
            }
            else
            {
                user.Purpose--;
            }
            _context.SaveChanges();
        }
    }
}
