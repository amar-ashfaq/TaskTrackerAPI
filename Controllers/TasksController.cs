using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerAPI.Models;

namespace TaskTrackerAPI.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskTrackerDbContext _taskTrackerDbContext;
        public TasksController(TaskTrackerDbContext taskTrackerDbContext)
        {
            _taskTrackerDbContext = taskTrackerDbContext;
        }

        // GET
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

    }
}
