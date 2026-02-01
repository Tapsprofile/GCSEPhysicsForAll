using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Models;

public class Content
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ModuleType { get; set; } = string.Empty; // e.g., "Physics", "Chemistry", "Biology"
    public string ContentType { get; set; } = string.Empty; // e.g., "Lesson", "Quiz", "Video", "Assignment"
    public Dictionary<string, object> Data { get; set; } = new(); // Flexible data structure
    public string[] Tags { get; set; } = Array.Empty<string>();
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsPublished { get; set; }
    public int Order { get; set; } // For ordering content within a module
}

public class ContentProgress
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string ContentId { get; set; } = string.Empty;
    public Guid StudentId { get; set; }
    public int ProgressPercentage { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new(); // For quiz scores, etc.
}
