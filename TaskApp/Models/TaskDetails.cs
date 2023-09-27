namespace TaskApp.Models
{
    public class TaskDetals
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TaskDescription { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
    }
}
