using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskModelsController : ControllerBase
    {
        private readonly TaskService _taskService;
        private readonly ILogger<TaskModelsController> _logger;

        public TaskModelsController(TaskService taskService, ILogger<TaskModelsController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        // GET: api/TaskModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasks()
        {
            try
            {
                _logger.LogInformation("Fetching Tasks");
                return await _taskService.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching tasks");
                return StatusCode(500, "Internal server error");
            }
        }
            
        // GET: api/TaskModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModel>> GetTaskModel(int id)
        {
            try
            {
                var taskModel = await _taskService.GetByIdAsync(id);

                if (taskModel == null)
                {
                    _logger.LogError($"Task with id {id} not found");
                    return NotFound();
                }

                return taskModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching task with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/TaskModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskModel(int id, TaskModel taskModel)
        {
            if (id != taskModel.Id)
            {
                _logger.LogError($"Task id {id} does not match task id {taskModel.Id}");
                return BadRequest();
            }

            try
            {
                await _taskService.UpdateAsync(taskModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskModelExists(id))
                {
                    _logger.LogError($"Task with id {id} not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating task with id {id}");
                return StatusCode(500, "Internal server error");
            }

            return NoContent();
        }

        // POST: api/TaskModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskModel>> PostTaskModel(TaskModel taskModel)
        {
            try
            {
                await _taskService.AddAsync(taskModel);
                BackgroundJob.Schedule(() => _taskService.ExecuteTask(taskModel.Id), taskModel.ExecutionDateTime - DateTime.UtcNow);
                return CreatedAtAction("GetTaskModel", new { id = taskModel.Id }, taskModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new task");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/TaskModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskModel(int id)
        {
            try
            {
                var taskId = await _taskService.DeleteAsync(id);
                if (taskId == 0)
                {
                    _logger.LogError($"Task with id {id} not found");
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting task with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        private bool TaskModelExists(int id)
        {
            try
            {
                return _taskService.GetByIdAsync(id) != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while checking if task with id {id} exists");
                return false;
            }
        }
    }
}
