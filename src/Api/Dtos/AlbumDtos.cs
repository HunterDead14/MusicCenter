using Domain.Albums;

namespace Api.Dtos;

public record AlbumDto(Guid Id, string Title, string Artist, int TracksCount, DateTime ReleaseDate, string Genre, DateTime CreatedAt)
{
    public static AlbumDto FromDomainModel(Album album)
        => new(album.Id.Value, album.Title, album.Artist, album.TracksCount, album.ReleaseDate, album.Genre.ToString(), album.CreatedAt);
}
public record CreateAlbumDto(
    string Title,
    string Artist,
    int TracksCount,
    DateTime ReleaseDate,
    Genre Genre
);
public record UpdateAlbumDto(
    string Title,
    string Artist,
    int TracksCount,
    DateTime ReleaseDate,
    Genre Genre
);