using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace TeacherStudentService.Data;

public class TeacherStudentDbContext : DbContext
{
    public TeacherStudentDbContext(DbContextOptions<TeacherStudentDbContext> options) : base(options)
    {
    }

    public DbSet<TeacherStudent> TeacherStudents { get; set; } = null!;
    public DbSet<ParentStudent> ParentStudents { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TeacherStudent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TeacherId, e.StudentId }).IsUnique();
        });

        modelBuilder.Entity<ParentStudent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ParentId, e.StudentId }).IsUnique();
        });
    }
}
