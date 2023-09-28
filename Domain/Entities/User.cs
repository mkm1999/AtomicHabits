using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ImgSrc { get; set; }
        public  Role Role { get; set; }
        public int RoleId { get; set; }
        public virtual ICollection<ToDo> ToDos { get; set; }
    }
}
