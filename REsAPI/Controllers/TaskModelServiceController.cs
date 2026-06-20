using Microsoft.AspNetCore.Mvc;
using ModelsLib;
using REsAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskModelServicesInterface _taskService;

    public TasksController(ITaskModelServicesInterface taskService)
        => _taskService = taskService;

    // GET все
    [HttpGet]
    public async Task<IActionResult> GetTasks()
        => Ok(await _taskService.GetTasksAsync());

    // GET по id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        return task == null ? NotFound() : Ok(task);
    }

    // POST – создаём только из Name
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskModel incoming)
    {
        var task = new TaskModel
        {
            Name = incoming.Name,
            Ready = false,
            Created = DateTime.UtcNow,
            Ended = null,
            Deadline = incoming.Deadline
        };
        var created = await _taskService.CreateTaskAsync(task);
        return CreatedAtAction(nameof(GetTask), new { id = created.Id }, created);
    }

    // PUT – обновляем задачу
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskModel incoming)
    {
        if (id != incoming.Id)
            return BadRequest("ID mismatch");
        var task = new TaskModel
        {
            Id = id,                     
            Name = incoming.Name,
            Ready = incoming.Ready,      
            Deadline = incoming.Deadline
        };

        try
        {
            var updated = await _taskService.UpdateTaskAsync(task);
            return Ok(updated);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Task with id {id} not found");
        }
    }

    // PATCH – переключение статуса
    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> ToggleTaskStatus(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
            return NotFound();

        task.Ready = !task.Ready;
        var updated = await _taskService.UpdateTaskAsync(task);
        return Ok(updated);
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var deleted = await _taskService.DeleteTaskAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    // PENDING
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingTasks()
    {
        var tasks = await _taskService.GetPendingTasksAsync();
        return Ok(tasks);
    }

    //READY
    [HttpGet("ready")]
    public async Task<IActionResult> GetReadyTasks()
    {
        var tasks = await _taskService.GetReadyTasksAsync();
        return Ok(tasks);
    }
    //AUTO-COMPLETE
    [HttpPost("auto-complete")]
    public async Task<IActionResult> AutoCompleteExpiredTasks()
    {
        var count = await _taskService.AutoCompleteExpiredTasksAsync();
        return Ok(new { completed = count, message = $"{count} tasks auto-completed" });
    }
}