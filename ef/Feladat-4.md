# Feladat 4

1. Hozd létre az alábbi interfészt a Data projektben:

``` C#
using MovieCatalog.Data.Entities;

namespace MovieCatalog.Data
{
    public interface IMovieCatalogDataService
    {
        /// <summary>
        /// Az összes <see cref="Genre"/> lekérdezése név szerinti növekvő sorrendben.
        /// </summary>
        /// <returns>Egy betöltött <see cref="Genre"/> kollekciót adó <see cref="Task{T}"/>, amiben a műfajok szerepelnek név szerinti növekvő sorrendben.</returns>
        Task<IEnumerable<Genre>> GetGenresAsync();

        /// <summary>
        /// <see cref="Title"/> lekérdezése ID alapján. Ha nem található, kivétel keletkezik.
        /// Az eredmény objektumban a műfajok is szerepelnek, a műfajokban a hozzájuk tartozó további művek viszont nem.
        /// </summary>
        /// <param name="id">A megtalálandó <see cref="Title"/> entitás.</param>
        /// <returns>A megtalált <see cref="Title"/> entitással visszatérő <see cref="Task{T}"/>, amiben a műfajok kollekciója is ki van töltve.</returns>
        Task<Title> GetTitleByIdAsync(int id);

        /// <summary>
        /// Egy olyan szótár lekérdezése, ami visszaadja, hogy melyik műfajban hány mű található. A kulcs a műfaj Id tulajdonsága.
        /// </summary>
        /// <returns>Egy <see cref="Task{T}"/>, melynek aszinkron eredménye egy <see cref="IReadOnlyDictionary{T, T}"/> szótár, ambiben benne van, hogy melyik műfajban (Id szerint) hány mű található.</returns>
        Task<IReadOnlyDictionary<int, int>> GetTitleCountsByGenreIdAsync();

       
        /// <summary>
        /// Azon művek lekérdezése, amiknek az elsődleges címében VAGY az eredeti címében szerepel a megadott szövegrész.
        /// Maximum 10 eredmény érkezhet a lekérdezésre, és az eredmény objektumokban a hozzájuk tartozó műfajok is ki vannak töltve.
        /// Kisbetű-nagybetű érzékenységgel nem foglalkozik.
        /// </summary>
        /// <param name="namePart">A szövegrész, melyre szűrést végzünk.</param>
        /// <returns>Maximum 10 elemű <see cref="Title"/> halmazt visszaadó, betöltött <see cref="IEnumerable{T}"/> kollekciót reprezentáló <see cref="Task{T}"/>.</returns>
        Task<IEnumerable<Title>> GetTitlesByNameAsync(string namePart);
    }
}
```

2. Implementáld az interfészt egy `MovieCatalogDataService` nevű osztályban, de a `GetGenresAsync` kivételével egyelőre csak dobjanak kivételt. A kivételt dobó metódusvázakat legeneráltathatod, ha az alábbi fájlt létrehozva [használod a megfelelő code fix-et (`Ctrl+.`)](https://learn.microsoft.com/en-us/visualstudio/ide/reference/implement-interface?view=vs-2022):

``` C#
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCatalog.Data
{       
    // Fontos, hogy az osztály internal, mert kívülről csak az interfészt szeretnénk elérhetővé tenni!
    internal class MovieCatalogDataService : IMovieCatalogDataService 
    {
        private MovieCatalogDbContext DbContext { get; }

         /* TODO: konstruktor, DI */

        public async Task<IEnumerable<Genre>> GetGenresAsync() => 
            // a DbContext egy MovieCatalogDbContext típusú tagváltozó
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

Ez a logika csak azért kell, hogy a következő lépésben ne kelljen hivatkozni az `internal` láthatóságú `MovieCatalogDataService` típusra.

4. Add hozzá a szolgáltatáskonténerhez az adatszolgáltatást az előbbi függvény meghívásával (Terminal projekt, Program.cs):

``` C#
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services.AddDbContext<MovieCatalogDbContext>(o => 
                    o.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MovieCatalog"))
            .AddHostedService<TestConsole>()
            .AddMovieDataService()) //csak ez az új sor
    .ConfigureLogging(l => l.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning))
    .Build();
```

5.  Injektáltass a `TestConsole` osztályba egy  `IMovieCatalogDataService` példányt.

6. A könnyebb konzolos kiírás érdekében definiáld felül a `Genre` és a `Tile` `ToString` metódusát (Data projekt)

`Genre`

```csharp
public override string ToString() => $"Genre {Name} ({Id})";
```

`Title`

```csharp
public override string ToString() => $"Title {Id}: {TitleType} - {PrimaryTitle} ({OriginalTitle}, [{StartYear?.ToString() ?? "?"}{(EndYear != null ? $"-{EndYear}" : "")}]{($"<{RuntimeMinutes} min>" )}{(TitleGenres.Any() ? $" - {string.Join(", ", TitleGenres.Select(g => $"{g.Genre.Name}"))}" : string.Empty)}";
```

7. Implementáld most már az XML kommenteknek megfelelően a `MovieCatalogDataService` legalább még egy metódusát `GetGenresAsync`-en kívül. Teszteléshez bővítsd a `TestConsole` `StartAync` függvényét. Segítségül megadjuk az alábbi, minden metódust tesztelő egyszerű kódrészletet. **A paramétereket cseréld le sajátra! (ahol van paraméter)**

```csharp
// GetGenresAsync
Logger.LogInformation(
        string.Join(Environment.NewLine,await DataService.GetGenresAsync())
);

// GetTitleByIdAsync - létező
Logger.LogInformation(
        (await DataService.GetTitleByIdAsync(1010)).ToString()
);

// GetTitleByIdAsync - nem létező
try
{
    Logger.LogInformation(
        (await DataService.GetTitleByIdAsync(-1)).ToString()
    );
}
catch (Exception ex)
{
    Logger.LogWarning($"Movie -1 could not be found: {ex}\n");
}

// GetTitleCountsByGenreIdAsync
Logger.LogInformation(string.Join(Environment.NewLine, 
                (await DataService.GetTitleCountsByGenreIdAsync())
                    .Select(kvp=>string.Format($"{kvp.Key} ({kvp.Value})")))
                );

// GetTitlesByNameAsync
Logger.LogInformation(
    string.Join(Environment.NewLine, await DataService.GetTitlesByNameAsync("scream"))
);

```

Beadandó:
- az elkészült **saját/átírt** kódrészletekről készített képernyőképek,
- a terminal alkalmazásban futtatott lekérdezések eredménye a konzolra (`Logger`) írva.

**Bónusz**: egy jegy javítás kapható, ha mindegyik lekérdezés meg van írva.

## Végeztél

Végeztél a feladatokkal.
