using Application.Albums.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Albums;
using LanguageExt;
using MediatR;

namespace Application.Albums.Commands;

public record CreateAlbumCommand : IRequest<Either<AlbumException, Album>>
{
    public required string Title { get; init; }
    public required string Artist { get; init; }
    public required int TracksCount { get; init; }
    public required DateTime ReleaseDate { get; init; }
    public required Genre Genre { get; init; }
}

public class CreateAlbumCommandHandler(IAlbumRepository albumRepository)
    : IRequestHandler<CreateAlbumCommand, Either<AlbumException, Album>>
{
    public async Task<Either<AlbumException, Album>> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
    {
        var existingAlbum = await albumRepository.GetByTitle(request.Title, cancellationToken);

        return await existingAlbum.MatchAsync(
            album => new AlbumAlreadyExistsException(album.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<AlbumException, Album>> CreateEntity(
        CreateAlbumCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var album = Album.New(AlbumId.New(), request.Title, request.Artist, request.TracksCount,
                request.ReleaseDate, request.Genre);

            return await albumRepository.Add(album, cancellationToken);
        }
        catch (Exception exception)
        {
            return new AlbumUnhandledException(AlbumId.Empty(), exception);
        }
    }
}
