using Domain.Albums;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IAlbumQueries
{
    Task<IReadOnlyList<Album>> GetAll(CancellationToken cancellationToken);
    Task<Option<Album>> GetById(AlbumId id, CancellationToken cancellationToken);
}
