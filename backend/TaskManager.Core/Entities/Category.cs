using System.ComponentModel.DataAnnotations;

namespace TaskManager.Core.Entities;

public class Category
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(7)]
    public string Color { get; set; } = "#3B82F6";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}