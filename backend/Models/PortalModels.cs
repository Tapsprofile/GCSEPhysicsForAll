namespace ModularEducationPortal.Backend.Models;

public sealed class PortalData
{
    public List<Subject> Subjects { get; set; } = new();
}

public sealed class Subject
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string KeyStage { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Chapter> Chapters { get; set; } = new();
}

public sealed class Chapter
{
    public string Id { get; set; } = string.Empty;
    public int Order { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public List<string> LearningObjectives { get; set; } = new();
    public List<Lesson> Lessons { get; set; } = new();
    public List<Widget> Widgets { get; set; } = new();
}

public sealed class Lesson
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public int EstimatedMinutes { get; set; }
}

public sealed class Widget
{
    public string Type { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
}
