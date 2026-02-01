namespace CMSService.DTOs;

public class CreateContentRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ModuleType { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public Dictionary<string, object> Data { get; set; } = new();
    public string[] Tags { get; set; } = Array.Empty<string>();
    public int Order { get; set; }
}

public class UpdateContentRequest : CreateContentRequest
{
    public bool? IsPublished { get; set; }
}

public class ContentFilterRequest
{
    public string? ModuleType { get; set; }
    public string? ContentType { get; set; }
    public string[]? Tags { get; set; }
    public bool? IsPublished { get; set; }
}
