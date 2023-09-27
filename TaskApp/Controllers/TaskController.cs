using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskApp.Models;
using TaskApp.Repositary;

namespace TaskApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IData _idata;
        public TaskController(IData data)
        {
            _idata = data;
        }
        [HttpPost]
        public TaskDetals CreateTask(TaskDetals taskDetals)
        {
            _idata.CreateTask(taskDetals);
            return taskDetals;
        }
        [HttpGet]
        public List<TaskDetals> GetTask()
        {
          var data=  _idata.GetAllTask();
          return data;
        }
        [HttpGet("/api/enddate")]
        public string GetEndDate(DateTime startDate, int daysOfComplete)
        {
            var endDate = _idata.CalculateEndDate(startDate, daysOfComplete);
            return endDate;
        }
    }
}
