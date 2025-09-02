using MovieCatalog.Web.Utils;
using MovieCatalogApi.Entities;

namespace MovieCatalogApi.Services;

public interface IMovieCatalogDataService
{
    Task<Dictionary<Genre, int>> GetGenresWithTitleCountsAsync();
    Task<PagedResult<Title>> GetTitlesAsync(int pageSize, int page, TitleFilter? filter, TitleSort titleSort, bool sortDescending);
    Task<IEnumerable<Genre>> GetGenresAsync();
    Task<Title> GetTitleByIdAsync(int id);
    Task<Title> InsertOrUpdateTitleAsync(int? id, string? primaryTitle, string? originalTitle, TitleType titleType, int? startYear, int? endYear, int? runtimeMinutes, int[]? genres);
}