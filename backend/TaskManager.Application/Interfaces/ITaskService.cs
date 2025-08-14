using TaskManager.Application.DTOs;

namespace TaskManager.Application.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetUserTasksAsync(int userId, string? search = null, int? categoryId = null, bool? isCompleted = null);
    Task<TaskDto?> GetTaskByIdAsync(int id, int userId);
    Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto, int userId);
    Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto, int userId);
    Task<bool> DeleteTaskAsync(int id, int userId);
    Task<TaskDto?> ToggleTaskCompletionAsync(int id, int userId);
}