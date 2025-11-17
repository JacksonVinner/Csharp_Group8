using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Image
{
    public int Id { get; set; }

    public int QueueId { get; set; }
    public Queue Queue { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string ImageGroup { get; set; } = string.Empty; // 图片组标识

    [Required]
    [MaxLength(200)]
    public string FolderName { get; set; } = string.Empty; // 文件夹名称

    [Required]
    [MaxLength(200)]
    public string FileName { get; set; } = string.Empty; // 文件名

    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; } = string.Empty; // 文件存储路径

    public int Order { get; set; } = 0; // 在组内的顺序

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<SelectionRecord> SelectionRecords { get; set; } = new List<SelectionRecord>();
}

