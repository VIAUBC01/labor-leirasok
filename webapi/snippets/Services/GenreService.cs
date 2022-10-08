using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Entities;
using MovieCatalogApi.Exceptions;

namespace MovieCatalogApi.Services;

public class GenreService : IGenreService
{
    private MovieCatalogDbContext ctx;

    public GenreService(MovieCatalogDbContext pctx)
    {
        ctx = pctx;
    }

    public async Task<IEnumerable<Genre>> GetGenresAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Genre> GetGenreAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateGenreAsync(int id, Genre genre)
    {
        var item = await ctx.Genres.FindAsync(id) 
                   ?? throw new ObjectNotFoundException<Genre>(id);
        var conflict = ctx.Genres.FirstOrDefault(g => g.Name == genre.Name);
        if (conflict != null)
            throw new ConflictException(conflict.Id, nameof(Genre));
        item.Name = genre.Name;
        await ctx.SaveChangesAsync();
    }

    public async Task<int> InsertGenreAsync(Genre genre)
    {
        var conflict = ctx.Genres.FirstOrDefault(g => g.Name == genre.Name);
        if(conflict != null)
            throw new ConflictException(conflict.Id, nameof(Genre));

        var newItem = new Genre(genre.Name);
        ctx.Add(newItem);
        await ctx.SaveChangesAsync();
        return newItem.Id;
    }

    public async Task DeleteGenreAsync(int id)
    {
        var g = await ctx.Genres.FindAsync(id) 
                ?? throw new ObjectNotFoundException<Genre>(id);
        ctx.Remove(g);
        await ctx.SaveChangesAsync();
    }
}