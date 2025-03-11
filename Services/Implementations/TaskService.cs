using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.Interfaces;
using TaskManagementSystem.Service.Implementations;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Services.Implementations
{
    public class TaskService : BaseService<TaskModel>, ITaskService
    {
        public TaskService(ITaskRepository repository) : base(repository) { }
    }
}
