using System.Net;
using Microsoft.AspNetCore.Mvc;
using NOS.Engineering.Challenge.API.Models;
using NOS.Engineering.Challenge.Managers;
using NOS.Engineering.Challenge.Models;

namespace NOS.Engineering.Challenge.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ContentController : Controller
{
    private readonly IContentsManager _manager;
    public ContentController(IContentsManager manager)
    {
        _manager = manager;
    }
    
    [Obsolete]
    [HttpGet]
    public async Task<IActionResult> GetManyContentsAsync()
    {
        var contents = await _manager.GetManyContentsAsync().ConfigureAwait(false);

        if (!contents.Any())
            return NotFound();
        
        return Ok(contents);
    }

    [HttpGet("filtered")]
    public async Task<IActionResult> GetFilteredContentsAsync(string? title, string? genre)
    {
        var filterDto = new FilterDto(title, genre);

        var contents = await _manager.GetFilteredContentsAsync(filterDto).ConfigureAwait(false);

        if (!contents.Any())
            return NotFound();

        return Ok(contents);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetContentAsync(Guid id)
    {
        var content = await _manager.GetContentAsync(id).ConfigureAwait(false);

        if (content == null)
            return NotFound();
        
        return Ok(content);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateContentAsync(
        [FromBody] ContentInput content
        )
    {
        var createdContent = await _manager.CreateContentAsync(content.ToDto()).ConfigureAwait(false);

        return createdContent == null ? Problem() : Ok(createdContent);
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateContentAsync(
        Guid id,
        [FromBody] ContentInput content
        )
    {
        var updatedContent = await _manager.UpdateContentAsync(id, content.ToDto()).ConfigureAwait(false);

        return updatedContent == null ? NotFound() : Ok(updatedContent);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContentAsync(
        Guid id
    )
    {
        var deletedId = await _manager.DeleteContentAsync(id).ConfigureAwait(false);
        return Ok(deletedId);
    }
    
    [HttpPost("{id}/genre")]
    public async Task<IActionResult> AddGenresAsync(
        Guid id,
        [FromBody] IEnumerable<string> genreList
    )
    {
        var updatedContent = await _manager.AddGenresAsync(id, genreList).ConfigureAwait(false);

        return updatedContent == null ? NotFound() : Ok(updatedContent);
    }
    
    [HttpDelete("{id}/genre")]
    public async Task<IActionResult> RemoveGenresAsync(
        Guid id,
        [FromBody] IEnumerable<string> genreList
    )
    {
        var updatedContent = await _manager.RemoveGenresAsync(id, genreList).ConfigureAwait(false);

        return updatedContent == null ? NotFound() : Ok(updatedContent);
    }
}