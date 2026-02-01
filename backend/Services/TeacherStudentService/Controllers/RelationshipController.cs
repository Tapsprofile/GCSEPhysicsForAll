using Microsoft.AspNetCore.Mvc;
using TeacherStudentService.DTOs;
using TeacherStudentService.Services;

namespace TeacherStudentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RelationshipController : ControllerBase
{
    private readonly IRelationshipService _relationshipService;
    private readonly ILogger<RelationshipController> _logger;

    public RelationshipController(IRelationshipService relationshipService, ILogger<RelationshipController> logger)
    {
        _relationshipService = relationshipService;
        _logger = logger;
    }

    [HttpPost("teacher-student")]
    public async Task<IActionResult> AssignTeacher([FromBody] AssignTeacherRequest request)
    {
        try
        {
            var result = await _relationshipService.AssignTeacherToStudentAsync(request.TeacherId, request.StudentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning teacher to student");
            return StatusCode(500, "Error assigning teacher");
        }
    }

    [HttpGet("teacher/{teacherId}/students")]
    public async Task<IActionResult> GetStudentsByTeacher(Guid teacherId)
    {
        try
        {
            var students = await _relationshipService.GetStudentsByTeacherAsync(teacherId);
            return Ok(students);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving students");
            return StatusCode(500, "Error retrieving students");
        }
    }

    [HttpGet("student/{studentId}/teachers")]
    public async Task<IActionResult> GetTeachersByStudent(Guid studentId)
    {
        try
        {
            var teachers = await _relationshipService.GetTeachersByStudentAsync(studentId);
            return Ok(teachers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving teachers");
            return StatusCode(500, "Error retrieving teachers");
        }
    }

    [HttpDelete("teacher-student")]
    public async Task<IActionResult> RemoveTeacherStudent([FromQuery] Guid teacherId, [FromQuery] Guid studentId)
    {
        try
        {
            var result = await _relationshipService.RemoveTeacherStudentAsync(teacherId, studentId);
            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing teacher-student relationship");
            return StatusCode(500, "Error removing relationship");
        }
    }

    [HttpPost("parent-student")]
    public async Task<IActionResult> AssignParent([FromBody] AssignParentRequest request)
    {
        try
        {
            var result = await _relationshipService.AssignParentToStudentAsync(
                request.ParentId, request.StudentId, request.Relationship);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning parent to student");
            return StatusCode(500, "Error assigning parent");
        }
    }

    [HttpGet("parent/{parentId}/students")]
    public async Task<IActionResult> GetStudentsByParent(Guid parentId)
    {
        try
        {
            var students = await _relationshipService.GetStudentsByParentAsync(parentId);
            return Ok(students);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving students");
            return StatusCode(500, "Error retrieving students");
        }
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { status = "healthy", service = "TeacherStudentService" });
    }
}
