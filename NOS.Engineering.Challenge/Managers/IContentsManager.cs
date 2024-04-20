using NOS.Engineering.Challenge.Models;

namespace NOS.Engineering.Challenge.Managers;

public interface IContentsManager
{
    Task<IEnumerable<Content?>> GetManyContentsAsync();
    Task<IEnumerable<Content?>> GetFilteredContentsAsync(FilterDto filterDto);
    Task<Content?> CreateContentAsync(ContentDto contentDto);
    Task<Content?> GetContentAsync(Guid id);
    Task<Content?> UpdateContentAsync(Guid id, ContentDto content);
    Task<Guid> DeleteContentAsync(Guid id);
    Task<Content?> AddGenresAsync(Guid id, IEnumerable<string> genreList);
    Task<Content?> RemoveGenresAsync(Guid id, IEnumerable<string> genreList);
}