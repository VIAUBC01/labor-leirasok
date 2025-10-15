using Microsoft.EntityFrameworkCore;
using MovieCatalogApi.Dtos;
using MovieCatalogApi.Entities;
using MovieCatalogApi.Exceptions;

namespace MovieCatalogApi.Services;

public class TitleService : ITitleService
{

    private MovieCatalogDbContext ctx;

    public TitleService(MovieCatalogDbContext pctx)
    {
        ctx = pctx;
    }

    public async Task<IEnumerable<TitleQueryModel>> GetTitlesAsync(int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<TitleQueryModel> GetTitleAsync(int id)
    {
        var tplusg = await ctx.Titles
            .Select(t => new { Title = t, GenreIDs = t.TitleGenres.Select(tg => tg.GenreId) })
            .SingleOrDefaultAsync(x=>x.Title.Id== id);
        if (tplusg == null)
            throw new ObjectNotFoundException<Title>(id);
        return new TitleQueryModel(tplusg.Title, tplusg.GenreIDs.ToArray());

    }

    public async Task UpdateTitleAsync(int id, TitleInsertUpdateModel title)
    {
        var item = await ctx.Titles.FindAsync(id)
                   ?? throw new ObjectNotFoundException<Title>(id);
        ctx.Entry(item).CurrentValues.SetValues(title);
        await ctx.SaveChangesAsync();
    }

    public async Task<int> InsertTitleAsync(TitleInsertUpdateModel title)
    {
       throw new NotImplementedException();
    }

    public async Task DeleteTitleAsync(int id)
    {
        throw new NotImplementedException();
    }
}