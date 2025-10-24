namespace MovieCatalog.Web.Utils;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

    
/// <summary>
/// Lapozott lekérdezési eredményeket tároló objektum.
/// </summary>
/// <typeparam name="TEntity">Az eredményhalmaz elemeinek típusa.</typeparam>
public sealed class PagedResult<TEntity>
{
    /// <summary>
    /// Privát konstruktor, mert ő végzi a távoli objektumok lekérdezését, ezért aszinkron kell előállítani.
    /// </summary>
    private PagedResult()
    {
        Results = ImmutableArray<TEntity>.Empty;
    }

    /// <summary>
    /// Üres példány nem nullozható változók inicializációjához.
    /// </summary>
    public static PagedResult<TEntity> Empty { get; } = new ();

    /// <summary>
    /// Az eredményeket tartalmazó betöltött, nem módosítható kollekció.
    /// </summary>
    public IReadOnlyCollection<TEntity> Results { get; private set; }

    /// <summary>
    /// Az összes találat száma.
    /// </summary>
    public int AllResultsCount { get; private set; }

    /// <summary>
    /// Az eredményhez tartozó oldalszám.
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    /// A jelenlegi oldal száma. 0-tól induló érték.
    /// </summary>
    public int CurrentPageNumber { get; private set; }

    /// <summary>
    /// Az utolsó oldal száma. 0-tól induló érték. Ha nincs találat, értéke -1.
    /// </summary>
    public int LastPageNumber => (AllResultsCount - 1) / PageSize;

    /// <summary>
    /// Entitások materializálása megadott lekérdezés alapján. Adatbázisban kerül elvégzésre a lapozás. Két lekérdezés fut: egy az adatok, egy pedig a teljes találati halmaz számosságának lekérdezésére. A két lekérdezés tranzakcióban futtatásának biztosítása a hívó fél felelőssége.
    /// </summary>
    /// <param name="entities">A materializálandó lekérdezés.</param>
    /// <param name="pageSize">A lekérdezendő oldal mérete. Min. 1.</param>
    /// <param name="pageNumber">A lekérdezendő oldal száma. 0-tól induló érték.</param>
    /// <returns>A materializált entitásokat és oldalinformációt tartalmazó <see cref="PagedResult{T}"/> objektumot visszaadó <see cref="Task{T}"/>.</returns>
    public static async Task<PagedResult<TEntity>> ExecuteQueryAsync(IQueryable<TEntity> entities, int pageSize, int pageNumber) =>
        new()
        {
            PageSize = pageSize >= 1 ? pageSize : throw new ArgumentException(null, nameof(pageSize)),
            CurrentPageNumber = pageNumber >= 0 ? pageNumber : throw new ArgumentException(null, nameof(pageNumber)),
            Results = (await entities.Skip(pageSize * pageNumber).Take(pageSize).ToListAsync()).ToImmutableArray(),
            AllResultsCount = await entities.CountAsync()
        };

    /// <summary>
    /// A találati lista transzformálása egy új <see cref="PagedResult{TEntity}"/> objektummá.
    /// </summary>
    /// <typeparam name="TResult">A transzformálás eredményeképpen előálló objektum típusa. Elhagyható, mert a transzformációs függvényből következik.</typeparam>
    /// <param name="transformationFunction">A transzformációs függvény, amelyet minden elemre futtatunk.</param>
    /// <returns>A transzformált eredményhalmazt tartalmazó <see cref="PagedResult{TEntity}"/> objektummásolat.</returns>
    public PagedResult<TResult> Transform<TResult>(Func<TEntity, TResult> transformationFunction) =>
        new()
        {
            Results = Results.Select(transformationFunction).ToImmutableArray(),
            AllResultsCount = AllResultsCount,
            PageSize = PageSize,
            CurrentPageNumber = CurrentPageNumber
        };
}