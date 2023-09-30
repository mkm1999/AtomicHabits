using Application.ToDoServices;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AtomicHabits.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        // GET: api/<ToDoController>
        [HttpGet]
        public IActionResult Get(DateTime date, [FromQuery] int UserId)
        {
            var result = _toDoService.GetSpecificDayToDos(DateOnly.Parse(date.Date.ToShortDateString()),UserId);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
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

        // GET api/<ToDoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ToDoController>
        [HttpPost]
        public IActionResult Post([FromBody] RequestAddTodoDto requset)
        {
            var result = _toDoService.AddToDo(requset);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        // PUT api/<ToDoController>
        [HttpPut]
        public IActionResult Put(RequestEditTodoDto request)
        {
            var result = _toDoService.EditToDo(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<ToDoController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _toDoService.RemoveToDo(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
