using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI.Dtos
{
    public class RequestAddTodoDto
    {
        public string Title { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
    public class RequestEditTodoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? DateTime { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
