using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.Interfaces;

namespace TaskManagementSystem.Services
{
    public class TaskService : BaseService<TaskModel>
    {
        public readonly ExternalApiService _externalApiService;
        public TaskService(ITaskRepository repository, ExternalApiService externalApiService) : base(repository) 
        {
            _externalApiService = externalApiService;
        }

        public async Task ExecuteTask(int taskId)
        {
            var task = await GetByIdAsync(taskId);
            if (task == null) return;

            bool isCompleted = await _externalApiService.IsTaskCompletedAsync(taskId);

            task.Status = isCompleted ? Enum.TaskStatusEnum.Completed : Enum.TaskStatusEnum.InProgress;
            task.UpdatedAt = DateTime.UtcNow;

            await UpdateAsync(task);
        }
    }
}
