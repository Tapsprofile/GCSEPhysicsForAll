namespace Shared.Models;

public class TeacherStudent
{
    public Guid Id { get; set; }
    public Guid TeacherId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime AssignedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public User? Teacher { get; set; }
    public User? Student { get; set; }
}

public class ParentStudent
{
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public Guid StudentId { get; set; }
    public string Relationship { get; set; } = string.Empty; // e.g., "Father", "Mother", "Guardian"
    public DateTime AssignedAt { get; set; }
    
    // Navigation properties
    public User? Parent { get; set; }
    public User? Student { get; set; }
}
