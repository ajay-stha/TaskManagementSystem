using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskModelsController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskModelsController> _logger;

        public TaskModelsController(ITaskService taskService, ILogger<TaskModelsController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        // GET: api/TaskModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasks()
        {
            _logger.LogInformation("Fetching Tasks");
            return await _taskService.GetAllAsync();
        }

        // GET: api/TaskModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModel>> GetTaskModel(int id)
        {
            var taskModel = await _taskService.GetByIdAsync(id);

            if (taskModel == null)
            {
                _logger.LogError($"Task with id {id} not found");
                return NotFound();
            }

            return taskModel;
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

            return NoContent();
        }

        // POST: api/TaskModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskModel>> PostTaskModel(TaskModel taskModel)
        {
            await _taskService.AddAsync(taskModel);

            return CreatedAtAction("GetTaskModel", new { id = taskModel.Id }, taskModel);
        }

        // DELETE: api/TaskModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskModel(int id)
        {
            var taskId = await _taskService.DeleteAsync(id);
            if (taskId == 0)
            {
                _logger.LogError($"Task with id {id} not found");
                return NotFound();
            }

            return NoContent();
        }

        private bool TaskModelExists(int id)
        {
            return _taskService.GetByIdAsync(id) != null;
        }
    }
}
