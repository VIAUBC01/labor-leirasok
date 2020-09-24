# Feladat 4

1. Hozd létre az alábbi interfészt a Data projektben:

``` C#
using MovieCatalog.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieCatalog.Data
{
    public interface IMovieCatalogDataService
    {
        Task<IEnumerable<Genre>> GetGenresAsync(); // Az összes műfaj lekérdezése név szerinti növekvő sorrendben.

        Task<Title> GetMovieByIdAsync(int id); // Mű lekérdezése ID alapján. Ha nem található, tetszőleges Exceptionnek kell keletkeznie. Az eredmény objektumban a műfajoknak is szerepelnie kell. Tipp: .Include()

        Task<IReadOnlyDictionary<int, int>> GetTitleCountsByGenreIdAsync(); // Egy olyan szótár lekérdezése, ami visszaadja, hogy melyik műfajban hány mű található. Tipp: .ToDictionary(), .Count()

        Task<IEnumerable<Title>> GetMoviesAsync(int pageSize, int page, int? startYearMin = null, int? startYearMax = null, int? genreId = null); // Filmek szűrése és listázása lapozott formában: pageSize db elem visszaadása, page-edik oldalon (0 indexű). Szűrés opcionális: ha bármely paraméter értéke null, akkor az alapján nem szűrünk, különben igen.

        Task<IEnumerable<Title>> GetMoviesByTitleAsync(string titlePart); // Azon művek lekérdezése, amiknek az elsődleges címében VAGY az eredeti címében szerepel a megadott szövegrész (kis-nagybetűtől független). Maximum 100 eredmény érkezhet a lekérdezésre, az eredmény objektumokban a műfajoknak is szerepelniük kell.

        Task InsertOrUpdateTitle(int? id, string primaryTitle, string originalTitle, TitleType titleType, int? startYear, int? endYear, int? runtimeMinutes, IEnumerable<string> genres); // Mű beszúrása vagy szerkesztése attól függően, hogy az id értéke null vagy sem. Tipp: .Attach()
    }
}

```

2. Implementáld az interfészt egy MovieCatalogDataService osztályban, ami értelemszerűen a megfelelő lekérdezéseket/műveleketek végzi el! A metódustörzseket legenerálhatod, ha az alábbi fájlt létrehozva használod a megfelelő code fix-et (`Ctrl+.`):

``` C#
namespace MovieCatalog.Data
{
    internal class MovieCatalogDataService : IMovieCatalogDataService // Fontos, hogy az osztály internal, mert kívülről csak az interfészt szeretnénk elérhetővé tenni!
    {
        public async Task<IEnumerable<Genre>> GetGenresAsync() => 
            await DbContext.Genres
                .OrderBy(g => g.Name)
                .ToListAsync();
    }
}
```

3. Hozd létre az alábbi osztályt a Data projektben:

``` C#
using MovieCatalog.Data;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MovieCatalogDataExtensions
    {
        public static IServiceCollection AddMovieDataService(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.Add(new ServiceDescriptor(typeof(IMovieCatalogDataService), typeof(MovieCatalogDataService), serviceLifetime));
            return services;
        }
    }
}
```

4. Add hozzá a szolgáltatáskonténerhez az adatszolgáltatást a Program.cs-ben:

``` C#
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
            services.AddDbContext<MovieCatalogDbContext>(o => o.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MovieCatalog"))
                    .AddHostedService<TestConsole>()
                    .AddMovieDataService()) // <<< +++ <<<
        .ConfigureLogging(l => l.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning));
```

Beadandó:
- az elkészült kódrészletekről készített képernyőképek,
- a terminal alkalmazásban futtatott lekérdezések kódjáról készült képek,
  - tipp: ha az entitások ToString() metódusát felüldefiniáljuk, értelemszerűen a kiírásuk is könnyebb lehet,
- a terminal alkalmazásban futtatott lekérdezések eredménye a konzolra (Logger) írva.

**Bónusz**: egy jegy javítás kapható, ha mindegyik lekérdezés [expression bodied method](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members#methods) formában van megírva.

## Végeztél

Végeztél a feladatokkal.
