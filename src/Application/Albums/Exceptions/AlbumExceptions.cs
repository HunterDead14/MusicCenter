using Domain.Albums;

namespace Application.Albums.Exceptions;

public abstract class AlbumException(AlbumId albumId, string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public AlbumId AlbumId { get; } = albumId;
}

public class AlbumAlreadyExistsException(AlbumId albumId)
    : AlbumException(albumId, $"Album already exists under id {albumId}");

public class AlbumNotFoundException(AlbumId albumId)
    : AlbumException(albumId, $"Album not found under id {albumId}");

public class AlbumUnhandledException(AlbumId albumId, Exception? innerException = null)
    : AlbumException(albumId, $"Unhandled exception under id {albumId}", innerException);
