using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Queue
{
    public int Id { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(2, 10)]
    public int ImageCount { get; set; } = 2; // 同时对比的图片数量

    public int TotalImages { get; set; } = 0; // 总图片组数

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<SelectionRecord> SelectionRecords { get; set; } = new List<SelectionRecord>();
    public ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
}

