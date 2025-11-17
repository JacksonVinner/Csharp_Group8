using System.Text.Json.Serialization;

namespace ImageAnnotationApp.Models
{
    public class Image
    {
        public int Id { get; set; }
        public int QueueId { get; set; }
        public string ImageGroup { get; set; } = string.Empty;
        public string FolderName { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public int Order { get; set; }
    }

    public class ImageGroup
    {
        [JsonPropertyName("ImageGroup")]
        public string GroupName { get; set; } = string.Empty;
        public List<Image> Images { get; set; } = new();
    }
}
