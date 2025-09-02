using MovieCatalogApi.Entities;

namespace MovieCatalogApi.Services;

public interface IGenreService
{
    Task<IEnumerable<Genre>> GetGenresAsync();
    Task<Genre> GetGenreAsync(int id);
    Task UpdateGenreAsync(int id, Genre genre);
    Task<int> InsertGenreAsync(Genre genre);
    Task DeleteGenreAsync(int id);
}