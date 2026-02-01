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

var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

var portalData = LoadJson<PortalData>(app.Environment.ContentRootPath, "Data/subjects.json", jsonOptions);
var registrationOptions = LoadJson<RegistrationOptions>(
    app.Environment.ContentRootPath,
    "Data/registration-options.json",
    jsonOptions);
var learningPathData = LoadJson<LearningPathData>(
    app.Environment.ContentRootPath,
    "Data/learning-paths.json",
    jsonOptions);

app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/api/subjects", () =>
{
    var subjects = portalData.Subjects
        .Select(subject => new
        {
            subject.Id,
            subject.Title,
            subject.Domain,
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

app.MapGet("/api/registration/options", () => Results.Ok(registrationOptions));

app.MapGet("/api/learning-paths", () =>
{
    var paths = learningPathData.Paths
        .OrderBy(path => path.Title)
        .ToList();

    return Results.Ok(paths);
});

app.MapGet("/api/subjects/{subjectId}/learning-paths", (string subjectId) =>
{
    var paths = learningPathData.Paths
        .Where(path => path.SubjectId.Equals(subjectId, StringComparison.OrdinalIgnoreCase))
        .OrderBy(path => path.Title)
        .ToList();

    return Results.Ok(paths);
});

app.MapGet("/api/learning-paths/{pathId}", (string pathId) =>
{
    var path = learningPathData.Paths.FirstOrDefault(item =>
        item.Id.Equals(pathId, StringComparison.OrdinalIgnoreCase));

    if (path is null)
    {
        return Results.NotFound(new { message = "Learning path not found." });
    }

    return Results.Ok(path);
});

app.Run();

static T LoadJson<T>(string contentRootPath, string relativePath, JsonSerializerOptions options)
    where T : new()
{
    var dataPath = Path.Combine(contentRootPath, relativePath);

    if (!File.Exists(dataPath))
    {
        return new T();
    }

    var json = File.ReadAllText(dataPath);
    return JsonSerializer.Deserialize<T>(json, options) ?? new T();
}
