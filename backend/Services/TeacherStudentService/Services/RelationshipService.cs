using Microsoft.EntityFrameworkCore;
using Shared.Models;
using TeacherStudentService.Data;

namespace TeacherStudentService.Services;

public interface IRelationshipService
{
    Task<TeacherStudent> AssignTeacherToStudentAsync(Guid teacherId, Guid studentId);
    Task<List<Guid>> GetStudentsByTeacherAsync(Guid teacherId);
    Task<List<Guid>> GetTeachersByStudentAsync(Guid studentId);
    Task<bool> RemoveTeacherStudentAsync(Guid teacherId, Guid studentId);
    Task<ParentStudent> AssignParentToStudentAsync(Guid parentId, Guid studentId, string relationship);
    Task<List<Guid>> GetStudentsByParentAsync(Guid parentId);
}

public class RelationshipService : IRelationshipService
{
    private readonly TeacherStudentDbContext _context;

    public RelationshipService(TeacherStudentDbContext context)
    {
        _context = context;
    }

    public async Task<TeacherStudent> AssignTeacherToStudentAsync(Guid teacherId, Guid studentId)
    {
        var existing = await _context.TeacherStudents
            .FirstOrDefaultAsync(ts => ts.TeacherId == teacherId && ts.StudentId == studentId);

        if (existing != null)
        {
            existing.IsActive = true;
            await _context.SaveChangesAsync();
            return existing;
        }

        var teacherStudent = new TeacherStudent
        {
            Id = Guid.NewGuid(),
            TeacherId = teacherId,
            StudentId = studentId,
            AssignedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.TeacherStudents.Add(teacherStudent);
        await _context.SaveChangesAsync();
        return teacherStudent;
    }

    public async Task<List<Guid>> GetStudentsByTeacherAsync(Guid teacherId)
    {
        return await _context.TeacherStudents
            .Where(ts => ts.TeacherId == teacherId && ts.IsActive)
            .Select(ts => ts.StudentId)
            .ToListAsync();
    }

    public async Task<List<Guid>> GetTeachersByStudentAsync(Guid studentId)
    {
        return await _context.TeacherStudents
            .Where(ts => ts.StudentId == studentId && ts.IsActive)
            .Select(ts => ts.TeacherId)
            .ToListAsync();
    }

    public async Task<bool> RemoveTeacherStudentAsync(Guid teacherId, Guid studentId)
    {
        var relationship = await _context.TeacherStudents
            .FirstOrDefaultAsync(ts => ts.TeacherId == teacherId && ts.StudentId == studentId);

        if (relationship == null)
            return false;

        relationship.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ParentStudent> AssignParentToStudentAsync(Guid parentId, Guid studentId, string relationship)
    {
        var existing = await _context.ParentStudents
            .FirstOrDefaultAsync(ps => ps.ParentId == parentId && ps.StudentId == studentId);

        if (existing != null)
        {
            existing.Relationship = relationship;
            await _context.SaveChangesAsync();
            return existing;
        }

        var parentStudent = new ParentStudent
        {
            Id = Guid.NewGuid(),
            ParentId = parentId,
            StudentId = studentId,
            Relationship = relationship,
            AssignedAt = DateTime.UtcNow
        };

        _context.ParentStudents.Add(parentStudent);
        await _context.SaveChangesAsync();
        return parentStudent;
    }

    public async Task<List<Guid>> GetStudentsByParentAsync(Guid parentId)
    {
        return await _context.ParentStudents
            .Where(ps => ps.ParentId == parentId)
            .Select(ps => ps.StudentId)
            .ToListAsync();
    }
}
