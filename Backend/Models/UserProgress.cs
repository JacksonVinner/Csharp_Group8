using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class UserProgress
{
    public int Id { get; set; }

    public int QueueId { get; set; }
    public Queue Queue { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int CompletedGroups { get; set; } = 0; // 已完成的图片组数量

    public int TotalGroups { get; set; } = 0; // 总图片组数量

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

