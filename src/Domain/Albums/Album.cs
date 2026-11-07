namespace Domain.Albums;

public class Album
{
    public AlbumId Id { get; }
    public DateTime CreatedAt { get; }
    
    public string Title { get; private set; }
    public string Artist { get; private set; }
    public int TracksCount { get; private set; }
    public DateTime ReleaseDate { get; private set; }
    public Genre Genre { get; private set; }

    private Album(AlbumId id, DateTime createdAt, string title, string artist, int tracksCount, DateTime releaseDate,
        Genre genre)
    {
        Id = id;
        Title = title;
        Artist = artist;
        TracksCount = tracksCount;
        ReleaseDate = releaseDate;
        Genre = genre;
        CreatedAt = createdAt;
        
    }
    public static Album New(AlbumId id, string title, string artist, int tracksCount, DateTime releaseDate, Genre genre)
    => new (id, releaseDate, title, artist, tracksCount, releaseDate, genre);

    public void UpdateDetails(string title, string artist, int tracksCount, DateTime releaseDate, Genre genre)
    {
        Title = title;
        Artist = artist;
        TracksCount = tracksCount;
        ReleaseDate = releaseDate;
        Genre = genre;
    }
    
    
    
}