using System.ComponentModel.DataAnnotations;
using TaskManager.Core.Enums;

namespace TaskManager.Application.DTOs;

public class CreateTaskDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public Priority Priority { get; set; } = Priority.Medium;

    public int? CategoryId { get; set; }

    public DateTime? DueDate { get; set; }
}