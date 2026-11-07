using Domain.Albums;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AlbumsConfiguration: IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new AlbumId(x));

        builder.Property(x => x.Title)
            .HasColumnType("varchar(255)")
            .IsRequired();
        
        builder.HasIndex(x => x.Title)
            .IsUnique();
        
        builder.Property(x => x.Artist)
            .HasColumnType("varchar(255)")
            .IsRequired();
        
        builder.Property(x => x.TracksCount)
            .IsRequired();
        
        builder.Property(x => x.ReleaseDate)
            .IsRequired();
        
        builder.Property(x => x.Genre)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .IsRequired();
        
        
        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();
    }
}