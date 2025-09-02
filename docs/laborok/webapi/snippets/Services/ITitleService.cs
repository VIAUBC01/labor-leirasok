using MovieCatalogApi.Dtos;
using MovieCatalogApi.Entities;

namespace MovieCatalogApi.Services;

public interface ITitleService
{
    Task<IEnumerable<TitleQueryModel>> GetTitlesAsync(int pageSize);

    Task<TitleQueryModel> GetTitleAsync(int id);

    Task UpdateTitleAsync(int id, TitleInsertUpdateModel title);

    Task<int> InsertTitleAsync(TitleInsertUpdateModel title);

    Task DeleteTitleAsync(int id);

}