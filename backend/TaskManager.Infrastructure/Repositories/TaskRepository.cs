using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Core.Entities;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItem>> GetUserTasksAsync(int userId, string? search = null, int? categoryId = null, bool? isCompleted = null)
    {
        var query = _context.Tasks
            .Include(t => t.Category)
            .Where(t => t.UserId == userId);

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(t => t.Title.Contains(search) || 
                                   (t.Description != null && t.Description.Contains(search)));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == categoryId.Value);
        }

        if (isCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == isCompleted.Value);
        }

        return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TaskItem?> GetUserTaskByIdAsync(int id, int userId)
    {
        return await _context.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    public async Task<TaskItem> CreateAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        
        // Reload with category information
        return await _context.Tasks
            .Include(t => t.Category)
            .FirstAsync(t => t.Id == task.Id);
    }

    public async Task<TaskItem> UpdateAsync(TaskItem task)
    {
        task.UpdatedAt = DateTime.UtcNow;
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
        
        // Reload with category information
        return await _context.Tasks
            .Include(t => t.Category)
            .FirstAsync(t => t.Id == task.Id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }
}