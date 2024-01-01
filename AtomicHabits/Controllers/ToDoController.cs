using Application.ToDoServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AtomicHabits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        // GET: api/<ToDoController>

        /// <summary>
        /// های یک روز خاص برای یک شخص خاص todo دریافت لیست
        /// </summary>
        /// <param name="date">تاریخ روز مورد نظر</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(DateTime date)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            var result = _toDoService.GetSpecificDayToDos(DateOnly.Parse(date.Date.ToShortDateString()),userId);
            if(!result.IsSuccess)
            {
                return Ok(result);
            }
            foreach (var item in result.Data)
            {
                item.Links = new List<Link> 
                    {
                        new Link
                        {
                            Method = "Delet",
                            href = Url.Action(nameof(Delete),"ToDo",new{id = item.Id}),
                            rel ="Delete"
                        },
                        new Link
                        {
                            Method = "Put",
                            href = Url.Action(nameof(Put),"ToDo"),
                            rel ="Edit"
                        },
                    };
            }
            return Ok(result);
        }

        /// <summary>
        /// دریافت یک تودو با ایدی
        /// </summary>
        /// <param name="id">آیدی تودو</param>
        /// <returns></returns>
        [HttpGet("GetToDo/{id}")]
        public IActionResult GetToDo(int  id)
        {
            return Ok(_toDoService.GetToDo(id));
        }
        // POST api/<ToDoController>

        /// <summary>
        /// افزودن یک todo جدید برای یک شخص خاص
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] RequestAddTodoDto requset)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            requset.UserId = userId;
            var result = _toDoService.AddToDo(requset);
            return Ok(result);
        }

        // PUT api/<ToDoController>

        /// <summary>
        /// ویرایش یک todo
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(RequestEditTodoDto request)
        {
            var result = _toDoService.EditToDo(request);
            if (!result.IsSuccess)
            {
                return Ok(result);
            }
            return Ok(result);
        }

        // DELETE api/<ToDoController>/5

        /// <summary>
        /// حذف یک todo
        /// </summary>
        /// <param name="id">ایدی todo</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _toDoService.RemoveToDo(id);
            if (!result.IsSuccess)
            {
                return Ok(result);
            }
            return Ok(result);
        }
    }
}
