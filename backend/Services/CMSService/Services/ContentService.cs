using CMSService.Data;
using CMSService.DTOs;
using MongoDB.Driver;
using Shared.Models;

namespace CMSService.Services;

public interface IContentService
{
    Task<List<Content>> GetAllContentAsync(ContentFilterRequest? filter = null);
    Task<Content?> GetContentByIdAsync(string id);
    Task<Content> CreateContentAsync(CreateContentRequest request, Guid createdBy);
    Task<Content?> UpdateContentAsync(string id, UpdateContentRequest request);
    Task<bool> DeleteContentAsync(string id);
}

public class ContentService : IContentService
{
    private readonly MongoDbContext _context;

    public ContentService(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Content>> GetAllContentAsync(ContentFilterRequest? filter = null)
    {
        var filterBuilder = Builders<Content>.Filter;
        var mongoFilter = filterBuilder.Empty;

        if (filter != null)
        {
            if (!string.IsNullOrEmpty(filter.ModuleType))
                mongoFilter &= filterBuilder.Eq(c => c.ModuleType, filter.ModuleType);

            if (!string.IsNullOrEmpty(filter.ContentType))
                mongoFilter &= filterBuilder.Eq(c => c.ContentType, filter.ContentType);

            if (filter.Tags?.Length > 0)
                mongoFilter &= filterBuilder.AnyIn(c => c.Tags, filter.Tags);

            if (filter.IsPublished.HasValue)
                mongoFilter &= filterBuilder.Eq(c => c.IsPublished, filter.IsPublished.Value);
        }

        return await _context.Content
            .Find(mongoFilter)
            .SortBy(c => c.Order)
            .ToListAsync();
    }

    public async Task<Content?> GetContentByIdAsync(string id)
    {
        return await _context.Content
            .Find(c => c.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Content> CreateContentAsync(CreateContentRequest request, Guid createdBy)
    {
        var content = new Content
        {
            Title = request.Title,
            Description = request.Description,
            ModuleType = request.ModuleType,
            ContentType = request.ContentType,
            Data = request.Data,
            Tags = request.Tags,
            Order = request.Order,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow,
            IsPublished = false
        };

        await _context.Content.InsertOneAsync(content);
        return content;
    }

    public async Task<Content?> UpdateContentAsync(string id, UpdateContentRequest request)
    {
        var update = Builders<Content>.Update
            .Set(c => c.Title, request.Title)
            .Set(c => c.Description, request.Description)
            .Set(c => c.ModuleType, request.ModuleType)
            .Set(c => c.ContentType, request.ContentType)
            .Set(c => c.Data, request.Data)
            .Set(c => c.Tags, request.Tags)
            .Set(c => c.Order, request.Order)
            .Set(c => c.UpdatedAt, DateTime.UtcNow);

        if (request.IsPublished.HasValue)
            update = update.Set(c => c.IsPublished, request.IsPublished.Value);

        return await _context.Content
            .FindOneAndUpdateAsync<Content>(
                c => c.Id == id,
                update,
                new FindOneAndUpdateOptions<Content, Content> { ReturnDocument = ReturnDocument.After }
            );
    }

    public async Task<bool> DeleteContentAsync(string id)
    {
        var result = await _context.Content.DeleteOneAsync(c => c.Id == id);
        return result.DeletedCount > 0;
    }
}
