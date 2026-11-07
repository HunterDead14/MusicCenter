using Application.Albums.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Albums;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Albums.Commands;

public record UpdateAlbumCommand : IRequest<Either<AlbumException, Album>>
{
    public required Guid AlbumId { get; init; }
    public required string Title { get; init; }
    public required string Artist { get; init; }
    public required int TracksCount { get; init; }
    public required DateTime ReleaseDate { get; init; }
    public required Genre Genre { get; init; }
}

public class UpdateAlbumCommandHandler(IAlbumRepository albumRepository)
    : IRequestHandler<UpdateAlbumCommand, Either<AlbumException, Album>>
{
    public async Task<Either<AlbumException, Album>> Handle(UpdateAlbumCommand request, CancellationToken cancellationToken)
    {
        var albumId = new AlbumId(request.AlbumId);
        var album = await albumRepository.GetById(albumId, cancellationToken);

        return await album.MatchAsync(
            p => CheckDuplicates(p.Id, request.Title, cancellationToken)
                .BindAsync(_ => UpdateEntity(p, request, cancellationToken)),
            () => new AlbumNotFoundException(albumId));
    }

    private async Task<Either<AlbumException, Unit>> CheckDuplicates(
        AlbumId currentAlbumId,
        string title,
        CancellationToken cancellationToken)
    {
        var album = await albumRepository.GetByTitle(title, cancellationToken);

        return album.Match<Either<AlbumException, Unit>>(
            p => p.Id == currentAlbumId ? Unit.Default : new AlbumAlreadyExistsException(p.Id),
            () => Unit.Default);
    }

    private async Task<Either<AlbumException, Album>> UpdateEntity(
        Album album,
        UpdateAlbumCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            album.UpdateDetails(request.Title, request.Artist, request.TracksCount, request.ReleaseDate, request.Genre);
            return await albumRepository.Update(album, cancellationToken);
        }
        catch (Exception exception)
        {
            return new AlbumUnhandledException(album.Id, exception);
        }
    }
}
