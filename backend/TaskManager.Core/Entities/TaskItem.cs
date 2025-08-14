using System.ComponentModel.DataAnnotations;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Entities;

public class TaskItem
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public bool IsCompleted { get; set; } = false;

    public Priority Priority { get; set; } = Priority.Medium;

    public int? CategoryId { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int UserId { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Category? Category { get; set; }
}