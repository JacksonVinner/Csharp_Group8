namespace ImageAnnotationApp.Models
{
    public class Queue
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int ImageCount { get; set; }
        public int TotalImages { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateQueueDto
    {
        public int ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ImageCount { get; set; }
    }

    public class UpdateQueueDto
    {
        public string Name { get; set; } = string.Empty;
        public int ImageCount { get; set; }
    }
}
