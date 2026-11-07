using Api.Dtos;
using Api.Modules.Errors;
using Application.Albums.Commands;
using Application.Common.Interfaces.Queries;
using Domain.Albums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("albums")]
[ApiController]
public class AlbumsController(
    ISender sender,
    IAlbumQueries albumQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbums(CancellationToken cancellationToken)
    {
        var albums = await albumQueries.GetAll(cancellationToken);
        return Ok(albums.Select(AlbumDto.FromDomainModel));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AlbumDto>> GetAlbum(Guid id, CancellationToken cancellationToken)
    {
        var album = await albumQueries.GetById(new AlbumId(id), cancellationToken);

        return album.Match<ActionResult<AlbumDto>>(
            p => Ok(AlbumDto.FromDomainModel(p)),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<AlbumDto>> CreateAlbum([FromBody] CreateAlbumDto request, CancellationToken cancellationToken)
    {
        var command = new CreateAlbumCommand
        {
            Title = request.Title,
            Artist = request.Artist,
            TracksCount = request.TracksCount,
            ReleaseDate = request.ReleaseDate,
            Genre = request.Genre
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<AlbumDto>>(
            p => CreatedAtAction(nameof(GetAlbum), new { id = p.Id.Value }, AlbumDto.FromDomainModel(p)),
            e => e.ToObjectResult());
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AlbumDto>> UpdateAlbum(Guid id, [FromBody] UpdateAlbumDto request, CancellationToken cancellationToken)
    {
        var command = new UpdateAlbumCommand
        {
            AlbumId = id,
            Title = request.Title,
            Artist = request.Artist,
            TracksCount = request.TracksCount,
            ReleaseDate = request.ReleaseDate,
            Genre = request.Genre
        };

        var result = await sender.Send(command, cancellationToken);

        return result.Match<ActionResult<AlbumDto>>(
            p => Ok(AlbumDto.FromDomainModel(p)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAlbum(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteAlbumCommand { AlbumId = id }, cancellationToken);

        return result.Match<IActionResult>(
            p => NoContent(),
            e => e.ToObjectResult());
    }
}