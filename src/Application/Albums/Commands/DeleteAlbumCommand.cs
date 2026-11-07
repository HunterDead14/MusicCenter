using Application.Albums.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Albums;
using LanguageExt;
using MediatR;

namespace Application.Albums.Commands;

public record DeleteAlbumCommand : IRequest<Either<AlbumException, Album>>
{
    public required Guid AlbumId { get; init; }
}

public class DeleteAlbumCommandHandler(IAlbumRepository albumRepository)
    : IRequestHandler<DeleteAlbumCommand, Either<AlbumException, Album>>
{
    public async Task<Either<AlbumException, Album>> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
    {
        var albumId = new AlbumId(request.AlbumId);
        var album = await albumRepository.GetById(albumId, cancellationToken);

        return await album.MatchAsync(
            p => DeleteEntity(p, cancellationToken),
            () => new AlbumNotFoundException(albumId));
    }

    private async Task<Either<AlbumException, Album>> DeleteEntity(
        Album album,
        CancellationToken cancellationToken)
    {
        try
        {
            await albumRepository.Delete(album, cancellationToken);
            return album;
        }
        catch (Exception exception)
        {
            return new AlbumUnhandledException(album.Id, exception);
        }
    }
}
