using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim ?? "0");
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks([FromQuery] string? search, [FromQuery] int? categoryId, [FromQuery] bool? isCompleted)
    {
        try
        {
            var userId = GetUserId();
            var tasks = await _taskService.GetUserTasksAsync(userId, search, categoryId, isCompleted);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving tasks" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        try
        {
            var userId = GetUserId();
            var task = await _taskService.GetTaskByIdAsync(id, userId);
            
            if (task == null)
                return NotFound(new { message = "Task not found" });

            return Ok(task);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the task" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
    {
        try
        {
            var userId = GetUserId();
            var task = await _taskService.CreateTaskAsync(createTaskDto, userId);
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the task" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto updateTaskDto)
    {
        try
        {
            var userId = GetUserId();
            var task = await _taskService.UpdateTaskAsync(id, updateTaskDto, userId);
            
            if (task == null)
                return NotFound(new { message = "Task not found" });

            return Ok(task);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the task" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        try
        {
            var userId = GetUserId();
            var success = await _taskService.DeleteTaskAsync(id, userId);
            
            if (!success)
                return NotFound(new { message = "Task not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the task" });
        }
    }

    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> ToggleTaskCompletion(int id)
    {
        try
        {
            var userId = GetUserId();
            var task = await _taskService.ToggleTaskCompletionAsync(id, userId);
            
            if (task == null)
                return NotFound(new { message = "Task not found" });

            return Ok(task);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the task" });
        }
    }
}