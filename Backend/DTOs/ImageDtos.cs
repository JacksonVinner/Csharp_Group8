namespace Backend.DTOs;

public class ImageDto
{
    public int Id { get; set; }
    public int QueueId { get; set; }
    public string ImageGroup { get; set; } = string.Empty;
    public string FolderName { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class ImageGroupDto
{
    public string ImageGroup { get; set; } = string.Empty;
    public List<ImageDto> Images { get; set; } = new List<ImageDto>();
}

