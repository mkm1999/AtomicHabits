using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProject.Entities
{
    public class ResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
    public class ResultDto<Ttype>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Ttype Data{ get; set; }
    }
}
