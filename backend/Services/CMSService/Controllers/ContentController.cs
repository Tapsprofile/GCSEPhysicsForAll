using CMSService.DTOs;
using CMSService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMSService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContentController : ControllerBase
{
    private readonly IContentService _contentService;
    private readonly ILogger<ContentController> _logger;

    public ContentController(IContentService contentService, ILogger<ContentController> logger)
    {
        _contentService = contentService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ContentFilterRequest? filter)
    {
        try
        {
            var content = await _contentService.GetAllContentAsync(filter);
            return Ok(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving content");
            return StatusCode(500, "Error retrieving content");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var content = await _contentService.GetContentByIdAsync(id);
            if (content == null)
                return NotFound();

            return Ok(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving content");
            return StatusCode(500, "Error retrieving content");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContentRequest request)
    {
        try
        {
            // In a real application, get this from authenticated user claims
            var createdBy = Guid.NewGuid();
            var content = await _contentService.CreateContentAsync(request, createdBy);
            return CreatedAtAction(nameof(GetById), new { id = content.Id }, content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating content");
            return StatusCode(500, "Error creating content");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateContentRequest request)
    {
        try
        {
            var content = await _contentService.UpdateContentAsync(id, request);
            if (content == null)
                return NotFound();

            return Ok(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating content");
            return StatusCode(500, "Error updating content");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var result = await _contentService.DeleteContentAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting content");
            return StatusCode(500, "Error deleting content");
        }
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { status = "healthy", service = "CMSService" });
    }
}
