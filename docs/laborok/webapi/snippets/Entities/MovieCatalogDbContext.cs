using Microsoft.EntityFrameworkCore;

namespace MovieCatalogApi.Entities;

public class MovieCatalogDbContext : DbContext
{
    public MovieCatalogDbContext(ILogger<MovieCatalogDbContext> logger
        , DbContextOptions<MovieCatalogDbContext> options) : base(options)
    {
        Logger = logger;
    }
    private ILogger<MovieCatalogDbContext> Logger { get; }

    public DbSet<Title> Titles => Set<Title>();

    public DbSet<Genre> Genres => Set<Genre>();

    public DbSet<TitleGenre> TitleGenres => Set<TitleGenre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Title>(title =>
        {
            title.Property(t => t.PrimaryTitle)
                .HasMaxLength(500);
            title.HasIndex(t => t.PrimaryTitle);
            title.HasIndex(t => t.TitleType);
        });

        modelBuilder.Entity<TitleGenre>().HasIndex(tg => new { tg.GenreId, tg.TitleId }).IsUnique();

        modelBuilder.Entity<Genre>(genre =>
        {
            genre.Property(g => g.Name).HasMaxLength(50);
            genre.HasIndex(g => g.Name).IsUnique();
        });
    }
}