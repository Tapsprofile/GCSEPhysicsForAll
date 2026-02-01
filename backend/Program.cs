using System.Text.Json;
using ModularEducationPortal.Backend.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("dev", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseCors("dev");

var portalData = LoadPortalData(app.Environment.ContentRootPath);

app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/api/subjects", () =>
{
    var subjects = portalData.Subjects
        .Select(subject => new
        {
            subject.Id,
            subject.Title,
            subject.Level,
            subject.KeyStage,
            subject.Description,
            ChapterCount = subject.Chapters.Count
        })
        .OrderBy(subject => subject.Title);

    return Results.Ok(subjects);
});

app.MapGet("/api/subjects/{subjectId}", (string subjectId) =>
{
    var subject = portalData.Subjects.FirstOrDefault(s =>
        s.Id.Equals(subjectId, StringComparison.OrdinalIgnoreCase));

    if (subject is null)
    {
        return Results.NotFound(new { message = "Subject not found." });
    }

    return Results.Ok(subject);
});

app.MapGet("/api/subjects/{subjectId}/chapters", (string subjectId) =>
{
    var subject = portalData.Subjects.FirstOrDefault(s =>
        s.Id.Equals(subjectId, StringComparison.OrdinalIgnoreCase));

    if (subject is null)
    {
        return Results.NotFound(new { message = "Subject not found." });
    }

    var chapters = subject.Chapters
        .OrderBy(chapter => chapter.Order)
        .Select(chapter => new
        {
            chapter.Id,
            chapter.Order,
            chapter.Title,
            chapter.Summary,
            LessonCount = chapter.Lessons.Count
        });

    return Results.Ok(chapters);
});

app.MapGet("/api/subjects/{subjectId}/chapters/{chapterId}", (string subjectId, string chapterId) =>
{
    var subject = portalData.Subjects.FirstOrDefault(s =>
        s.Id.Equals(subjectId, StringComparison.OrdinalIgnoreCase));

    if (subject is null)
    {
        return Results.NotFound(new { message = "Subject not found." });
    }

    var chapter = subject.Chapters.FirstOrDefault(c =>
        c.Id.Equals(chapterId, StringComparison.OrdinalIgnoreCase));

    if (chapter is null)
    {
        return Results.NotFound(new { message = "Chapter not found." });
    }

    return Results.Ok(chapter);
});

app.Run();

static PortalData LoadPortalData(string contentRootPath)
{
    var dataPath = Path.Combine(contentRootPath, "Data", "subjects.json");

    if (!File.Exists(dataPath))
    {
        return new PortalData();
    }

    var json = File.ReadAllText(dataPath);
    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    return JsonSerializer.Deserialize<PortalData>(json, options) ?? new PortalData();
}
