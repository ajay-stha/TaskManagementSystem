using TaskManagementSystem.Database;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.Interfaces;

namespace TaskManagementSystem.Repository.Implementations
{
    public class TaskRepository : BaseRepository<TaskModel>, ITaskRepository
    {
        public TaskRepository(TaskDbContext context) : base(context) {}
    }
}
