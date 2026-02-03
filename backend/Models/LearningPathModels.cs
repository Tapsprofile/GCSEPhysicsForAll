namespace ModularEducationPortal.Backend.Models;

public sealed class LearningPathData
{
    public List<LearningPath> Paths { get; set; } = new();
}

public sealed class LearningPath
{
    public string Id { get; set; } = string.Empty;
    public string SubjectId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Track { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Outcomes { get; set; } = new();
    public List<LearningStep> Steps { get; set; } = new();
}

public sealed class LearningStep
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Milestone { get; set; } = string.Empty;
    public int RecommendedWeeks { get; set; }
    public List<string> ChapterIds { get; set; } = new();
    public List<string> PrerequisiteStepIds { get; set; } = new();
    public EvidenceRequirement Evidence { get; set; } = new();
    public AssessmentRequirement Assessment { get; set; } = new();
}

public sealed class EvidenceRequirement
{
    public int ExamplesRequired { get; set; }
    public List<string> EvidenceTypes { get; set; } = new();
    public string ReviewType { get; set; } = string.Empty;
}

public sealed class AssessmentRequirement
{
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PassingCriteria { get; set; } = string.Empty;
}
