using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Core.Entities;

namespace TaskManager.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskDto>> GetUserTasksAsync(int userId, string? search = null, int? categoryId = null, bool? isCompleted = null)
    {
        var tasks = await _taskRepository.GetUserTasksAsync(userId, search, categoryId, isCompleted);
        return tasks.Select(MapToDto);
    }

    public async Task<TaskDto?> GetTaskByIdAsync(int id, int userId)
    {
        var task = await _taskRepository.GetUserTaskByIdAsync(id, userId);
        return task == null ? null : MapToDto(task);
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto, int userId)
    {
        var task = new TaskItem
        {
            Title = createTaskDto.Title,
            Description = createTaskDto.Description,
            Priority = createTaskDto.Priority,
            CategoryId = createTaskDto.CategoryId,
            DueDate = createTaskDto.DueDate,
            UserId = userId
        };

        var createdTask = await _taskRepository.CreateAsync(task);
        return MapToDto(createdTask);
    }

    public async Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto, int userId)
    {
        var task = await _taskRepository.GetUserTaskByIdAsync(id, userId);
        if (task == null) return null;

        task.Title = updateTaskDto.Title;
        task.Description = updateTaskDto.Description;
        task.IsCompleted = updateTaskDto.IsCompleted;
        task.Priority = updateTaskDto.Priority;
        task.CategoryId = updateTaskDto.CategoryId;
        task.DueDate = updateTaskDto.DueDate;
        task.UpdatedAt = DateTime.UtcNow;

        var updatedTask = await _taskRepository.UpdateAsync(task);
        return MapToDto(updatedTask);
    }

    public async Task<bool> DeleteTaskAsync(int id, int userId)
    {
        var task = await _taskRepository.GetUserTaskByIdAsync(id, userId);
        if (task == null) return false;

        return await _taskRepository.DeleteAsync(id);
    }

    public async Task<TaskDto?> ToggleTaskCompletionAsync(int id, int userId)
    {
        var task = await _taskRepository.GetUserTaskByIdAsync(id, userId);
        if (task == null) return null;

        task.IsCompleted = !task.IsCompleted;
        task.UpdatedAt = DateTime.UtcNow;

        var updatedTask = await _taskRepository.UpdateAsync(task);
        return MapToDto(updatedTask);
    }

    private static TaskDto MapToDto(TaskItem task)
    {
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            Priority = task.Priority,
            CategoryId = task.CategoryId,
            CategoryName = task.Category?.Name,
            CategoryColor = task.Category?.Color,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}