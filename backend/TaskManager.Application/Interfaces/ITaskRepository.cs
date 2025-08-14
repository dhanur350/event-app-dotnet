using TaskManager.Core.Entities;

namespace TaskManager.Application.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetUserTasksAsync(int userId, string? search = null, int? categoryId = null, bool? isCompleted = null);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem?> GetUserTaskByIdAsync(int id, int userId);
    Task<TaskItem> CreateAsync(TaskItem task);
    Task<TaskItem> UpdateAsync(TaskItem task);
    Task<bool> DeleteAsync(int id);
}