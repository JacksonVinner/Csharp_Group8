namespace ImageAnnotationApp.Models
{
    public class Selection
    {
        public int Id { get; set; }
        public int QueueId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string ImageGroup { get; set; } = string.Empty;
        public int SelectedImageId { get; set; }
        public string SelectedImagePath { get; set; } = string.Empty;
        public string SelectedFolderName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateSelectionDto
    {
        public int QueueId { get; set; }
        public string ImageGroup { get; set; } = string.Empty;
        public int SelectedImageId { get; set; }
    }

    public class UserProgress
    {
        public int QueueId { get; set; }
        public string QueueName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int CompletedGroups { get; set; }
        public int TotalGroups { get; set; }
        public double ProgressPercentage { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
