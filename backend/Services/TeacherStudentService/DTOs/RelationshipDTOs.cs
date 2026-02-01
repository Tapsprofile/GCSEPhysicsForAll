namespace TeacherStudentService.DTOs;

public class AssignTeacherRequest
{
    public Guid TeacherId { get; set; }
    public Guid StudentId { get; set; }
}

public class AssignParentRequest
{
    public Guid ParentId { get; set; }
    public Guid StudentId { get; set; }
    public string Relationship { get; set; } = string.Empty;
}

public class RelationshipDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }
    public bool IsActive { get; set; }
}
