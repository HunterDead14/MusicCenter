namespace Domain.Albums;

public record AlbumId(Guid Value)
{
    public static AlbumId Empty() => new(Guid.Empty);
    public static AlbumId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
