using TaskManagementSystem.Enum;

namespace TaskManagementSystem.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime ExecutionDateTime { get; set; }
        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
