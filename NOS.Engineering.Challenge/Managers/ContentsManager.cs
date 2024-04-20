using Microsoft.Extensions.Logging;
using NOS.Engineering.Challenge.Database;
using NOS.Engineering.Challenge.Models;

namespace NOS.Engineering.Challenge.Managers;

public class ContentsManager : IContentsManager
{
    private readonly IDatabase<Content?, ContentDto> _database;
    private readonly ILogger<ContentsManager> _logger;

    public ContentsManager(IDatabase<Content?, ContentDto> database, ILogger<ContentsManager> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task<IEnumerable<Content?>> GetManyContentsAsync()
    {
        return await _database.ReadAll().ConfigureAwait(false);
    }

    public async Task<IEnumerable<Content?>> GetFilteredContentsAsync(FilterDto filterDto)
    {
        var contents = await _database.ReadAll().ConfigureAwait(false);

        var filteredContents = contents.Where(content => content.Title.Contains(filterDto.Title, StringComparison.OrdinalIgnoreCase)
            && content.GenreList.Any(genre => genre.Contains(filterDto.Genre, StringComparison.OrdinalIgnoreCase)));

        return filteredContents;
    }

    public async Task<Content?> CreateContentAsync(ContentDto contentDto)
    {
        _logger.LogInformation("Creating new content");

        return await _database.Create(contentDto).ConfigureAwait(false);
    }

    public async Task<Content?> GetContentAsync(Guid id)
    {
        return await _database.Read(id).ConfigureAwait(false);
    }

    public async Task<Content?> UpdateContentAsync(Guid id, ContentDto content)
    {
        _logger.LogInformation("Updating content id: {Id}", id);

        return await _database.Update(id, content).ConfigureAwait(false);
    }

    public async Task<Guid> DeleteContentAsync(Guid id)
    {
        _logger.LogInformation("Deleting content id: {Id}", id);

        return await _database.Delete(id).ConfigureAwait(false);
    }

    public async Task<Content?> AddGenresAsync(Guid id, IEnumerable<string> genreList)
    {
        var content = await _database.Read(id).ConfigureAwait(false);

        if (content == null) return null;

        var newGenreList = genreList
            .Concat(content.GenreList)
            .Distinct();

        var contentDto = content.ToDto();
        contentDto.GenreList = newGenreList;

        _logger.LogInformation("Adding genres to content id: {Id}", id);

        return await _database.Update(id, contentDto).ConfigureAwait(false);
    }

    public async Task<Content?> RemoveGenresAsync(Guid id, IEnumerable<string> genreList)
    {
        var content = await _database.Read(id).ConfigureAwait(false);

        if (content == null) return null;
        
        var newGenreList = content.GenreList.Except(genreList);

        var contentDto = content.ToDto();
        contentDto.GenreList = newGenreList;

        _logger.LogInformation("Removing genres from content id: {Id}", id);

        return await _database.Update(id, contentDto).ConfigureAwait(false);
    }
}