using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagement.Repositories;
using TaskManagement.Models;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[Route("api/tasks")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly TaskRepository _taskRepository;

    public TaskController(TaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    //  Get All Tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasks()
    {
        string username = User.Identity.Name; // Get the logged-in user's username
        var tasks = await _taskRepository.GetTasksByUsernameAsync(username);
        return Ok(tasks);
    }

    //  Get Task by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskModel>> GetTaskById(int id)
    {
        var task = await _taskRepository.GetTaskByIdAsync(id);
        if (task == null)
            return NotFound(new { message = "Task not found" });

        return Ok(task);
    }

    //  Create a New Task
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskModel task)
    {
        if (task == null || string.IsNullOrEmpty(task.Title))
            return BadRequest(new { message = "Task title is required" });

        await _taskRepository.AddTaskAsync(task);
        return Ok(new { message = "Task created successfully!" });
    }

    //  Update an Existing Task
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskModel task)
    {
        var existingTask = await _taskRepository.GetTaskByIdAsync(id);
        if (existingTask == null)
            return NotFound(new { message = "Task not found" });

        task.Id = id; // Ensure the task ID matches the one in the URL
        await _taskRepository.UpdateTaskAsync(task);
        return Ok(new { message = "Task updated successfully!" });
    }

    //  Delete a Task
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var existingTask = await _taskRepository.GetTaskByIdAsync(id);
        if (existingTask == null)
            return NotFound(new { message = "Task not found" });

        await _taskRepository.DeleteTaskAsync(id);
        return Ok(new { message = "Task deleted successfully!" });
    }
}
