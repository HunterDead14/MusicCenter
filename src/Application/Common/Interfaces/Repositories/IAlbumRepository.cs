using Domain.Albums;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories;

public interface IAlbumRepository
{
    Task<Option<Album>> GetById(AlbumId id, CancellationToken cancellationToken);
    Task<Option<Album>> GetByTitle(string title, CancellationToken cancellationToken);
    Task<Album> Add(Album album, CancellationToken cancellationToken);
    Task<Album> Update(Album album, CancellationToken cancellationToken);
    Task Delete(Album album, CancellationToken cancellationToken);
}
