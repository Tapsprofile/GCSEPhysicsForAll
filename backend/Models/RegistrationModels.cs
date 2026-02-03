namespace ModularEducationPortal.Backend.Models;

public sealed class RegistrationOptions
{
    public string Version { get; set; } = "1.0";
    public string LastUpdated { get; set; } = string.Empty;
    public List<RoleOption> Roles { get; set; } = new();
    public RegistrationStateFlow StateFlow { get; set; } = new();
}

public sealed class RoleOption
{
    public string Role { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<RegistrationField> RequiredFields { get; set; } = new();
    public List<RegistrationField> OptionalFields { get; set; } = new();
    public List<VerificationStep> VerificationSteps { get; set; } = new();
    public List<ConsentRequirement> Consents { get; set; } = new();
    public RoleLinking Linking { get; set; } = new();
    public RoleCapacity Capacity { get; set; } = new();
    public List<string> SupportedSubjects { get; set; } = new();
}

public sealed class RegistrationField
{
    public string Id { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool Required { get; set; }
    public string Placeholder { get; set; } = string.Empty;
    public string Validation { get; set; } = string.Empty;
}

public sealed class VerificationStep
{
    public string Id { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Required { get; set; }
}

public sealed class ConsentRequirement
{
    public string Id { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string AppliesTo { get; set; } = string.Empty;
    public bool Required { get; set; }
}

public sealed class RoleLinking
{
    public bool CanLinkStudents { get; set; }
    public string LinkMethod { get; set; } = string.Empty;
    public int LinkCodeLength { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public sealed class RoleCapacity
{
    public int MaxStudents { get; set; }
    public int MaxWeeklyReviews { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public sealed class RegistrationStateFlow
{
    public List<RegistrationState> States { get; set; } = new();
}

public sealed class RegistrationState
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> NextStates { get; set; } = new();
}
