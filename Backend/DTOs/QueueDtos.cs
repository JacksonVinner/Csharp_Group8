using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class CreateQueueDto
{
    [Required(ErrorMessage = "项目ID是必需的")]
    public int ProjectId { get; set; }

    [Required(ErrorMessage = "队列名称是必需的")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "对比图片数量是必需的")]
    [Range(2, 10, ErrorMessage = "对比图片数量必须在2到10之间")]
    public int ImageCount { get; set; } = 2;
}

public class UpdateQueueDto
{
    [Required(ErrorMessage = "队列名称是必需的")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "对比图片数量是必需的")]
    [Range(2, 10, ErrorMessage = "对比图片数量必须在2到10之间")]
    public int ImageCount { get; set; }
}

public class QueueDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int ImageCount { get; set; }
    public int TotalImages { get; set; }
    public DateTime CreatedAt { get; set; }
}

