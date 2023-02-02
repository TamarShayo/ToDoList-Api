using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Interfaces;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace ToDoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyTaskController : ControllerBase
    {
        ITaskService TaskService;
        ITokenService TokenService;
        public MyTaskController(ITaskService TaskService)
        {
            this.TaskService = TaskService;
        }

        [HttpGet]
        [Authorize(Policy = ("User"))]
        public ActionResult<List<MyTask>> GetAll()
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return TaskService.GetAll(token);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = ("User"))]
        public ActionResult<MyTask> Get(int id)
        {
            var task = TaskService.Get(id);
            if (task == null)
                return NotFound();
            return task;
        }

        [HttpPost]
        [Authorize(Policy = ("User"))]
        public IActionResult Add(MyTask task)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            TaskService.Add(token, task);
            return CreatedAtAction(nameof(Add), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = ("User"))]
        public IActionResult Update(int id, MyTask task)
        {
            if (id != task.Id)
                return BadRequest();
            var existingTask = TaskService.Get(id);
            if (existingTask is null)
                return NotFound();
            TaskService.Update(task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = ("User"))]
        public IActionResult Delete(int id)
        {
            var task = TaskService.Get(id);
            if (task is null)
                return NotFound();
            TaskService.Delete(id);
            return Content(TaskService.Count.ToString());
        }
    }
}