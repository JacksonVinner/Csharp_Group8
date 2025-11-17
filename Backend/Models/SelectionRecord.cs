using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class SelectionRecord
{
    public int Id { get; set; }

    public int QueueId { get; set; }
    public Queue Queue { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string ImageGroup { get; set; } = string.Empty; // 选择的是哪一组图片

    public int SelectedImageId { get; set; }
    public Image SelectedImage { get; set; } = null!; // 用户选择的图片

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

