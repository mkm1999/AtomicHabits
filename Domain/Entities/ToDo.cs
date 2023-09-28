using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ToDo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        public ToDoType Type { get; set; }
        public Status Status { get; set; }
    }

    public enum ToDoType
    {
        Task = 1,
        Information = 2,
        Report = 3,
        Event = 4,
    }

    public enum Status
    {
        Finished = 1,
        InProgress = 2,
        Cancelled = 3,
        Transferred = 4,
    }
}
