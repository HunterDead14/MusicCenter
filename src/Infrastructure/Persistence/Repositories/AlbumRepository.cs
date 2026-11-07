using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Albums;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class AlbumRepository(ApplicationDbContext context) : IAlbumRepository, IAlbumQueries
{
    public async Task<IReadOnlyList<Album>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Albums.ToListAsync(cancellationToken);
    }

    public async Task<Option<Album>> GetById(AlbumId id, CancellationToken cancellationToken)
    {
        var entity = await context.Albums.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return entity ?? Option<Album>.None;
    }

    public async Task<Option<Album>> GetByTitle(string title, CancellationToken cancellationToken)
    {
        var entity = await context.Albums.FirstOrDefaultAsync(x => x.Title == title, cancellationToken);
        return entity ?? Option<Album>.None;
    }

    public async Task<Album> Add(Album album, CancellationToken cancellationToken)
    {
        await context.Albums.AddAsync(album, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return album;
    }

    public async Task<Album> Update(Album album, CancellationToken cancellationToken)
    {
        context.Albums.Update(album);
        await context.SaveChangesAsync(cancellationToken);
        return album;
    }

    public async Task Delete(Album album, CancellationToken cancellationToken)
    {
        context.Albums.Remove(album);
        await context.SaveChangesAsync(cancellationToken);
    }
}