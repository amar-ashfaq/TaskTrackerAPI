using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerAPI.Models;

namespace TaskTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskTrackerDbContext _taskTrackerDbContext;
        public TasksController(TaskTrackerDbContext taskTrackerDbContext)
        {
            _taskTrackerDbContext = taskTrackerDbContext;
        }

        // GET ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTaskItems()
        {
            var taskItems = await _taskTrackerDbContext.Tasks
                .OrderBy(x => x.Id)
                .ToListAsync();

            return Ok(taskItems);
        }

        // GET
        [HttpGet]
        [Route("{id}")]
        public ActionResult<TaskItem> GetTaskItem(int id)
        {          
            var taskItem = _taskTrackerDbContext.Tasks.Find(id);

            if (taskItem == null) {
                return NotFound();
            }

            return Ok(taskItem);          
        }

        // CREATE
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TaskItem taskItem) 
        {
            if (taskItem == null)
            {
                return BadRequest("Task data is required.");
            }

            _taskTrackerDbContext.Tasks.Add(taskItem);
            await _taskTrackerDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaskItem), new { id = taskItem.Id }, taskItem);
        }

        // UPDATE
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] TaskItem updatedTask)
        {
            var taskItem = await _taskTrackerDbContext.Tasks.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            taskItem.Title = updatedTask.Title;
            taskItem.Description = updatedTask.Description;
            taskItem.IsCompleted = updatedTask.IsCompleted;

            await _taskTrackerDbContext.SaveChangesAsync();

            return Ok(taskItem);
        }

        // DELETE
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var taskItem = await _taskTrackerDbContext.Tasks.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            _taskTrackerDbContext.Tasks.Remove(taskItem);
            await _taskTrackerDbContext.SaveChangesAsync(); 

            return NoContent();
        }

    }
}
