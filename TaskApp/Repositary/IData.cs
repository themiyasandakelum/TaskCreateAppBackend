using TaskApp.Models;

namespace TaskApp.Repositary
{
    public interface IData
    {
        public TaskDetals CreateTask(TaskDetals taskDetals);
        public List<TaskDetals> GetAllTask();
        public string CalculateEndDate(DateTime startDate, int daysOfComplete);
    }
}
