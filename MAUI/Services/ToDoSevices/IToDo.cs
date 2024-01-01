using MAUI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI.Services.ToDoSevices
{
    public interface IToDo
    {
        public Task<ResultDto<List<ToDoDto>>> GetToDosByDate(DateOnly date);
        public Task<ResultDto> AddToDo(RequestAddTodoDto request);
        public Task<ResultDto> DeleteToDo(int  id);
        public Task<ResultDto<ToDoDto>> GetToDo(int id);
        public Task<ResultDto> UpdateToDo(RequestEditTodoDto request);
    }
}
