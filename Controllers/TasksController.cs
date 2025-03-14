using Microsoft.AspNetCore.Mvc;

namespace TaskTrackerAPI.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
