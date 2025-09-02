# Áttekintés

A készülő alkalmazásban az [IMDB által naponta frissített, publikus adatok](https://www.imdb.com/interfaces/) felhasználásával fogjuk filmek adatait kezelni. Fontos, hogy ezt az adathalmazt saját, tanulási célra szabadon használhatjuk, minden más célra kifejezett engedélyt kell kérni.

Az elkészítendő alkalmazásunk egy osztálykönyvtár lesz, amit más kódból (pl. egy REST API-t szolgáltató web API projektből vagy egy szerveroldali renderelést használó projektből) szeretnénk majd elérni. Mivel a kódunk osztálykönyvtár, önmagában nem is futtatható, hanem egy .dll fájlként csak a kódot tartalmazza, amit más, futó .NET alkalmazásból el tudunk majd érni.

Először létrehozzuk az osztálykönyvtárat, amiben az adatbázist reprezántáló DbContextet, a táblákat reprezentáló entitásokat fogjuk definiálni. Itt lesznek még a szükséges migrációk is, amelyeket az adatbázisunkon lefuttatva az adatsémát a modellel szinkronba hozhatjuk. Az adatbázist code first módszertannal modellezzük, tehát először az entitásokat fogjuk definiálni, aztán létrehozzuk a migrációkat, amiket végül lefuttatunk, így jön majd létre az adatbázisban a modellünknek megfelelő séma.

Az osztálykönyvtárunkban található kódot nem tudjuk "csak úgy" futtatni, szükségünk lesz tehát valamilyen projektre, ami képes a kód futtatására. Ehhez ugyanabban a solutionben fogunk létrehozni egy új konzolos alkalmazást.

Emlékeztetőként:
- Amikor az adatmodellt (C# entitásosztályokat) módosítjuk, új migrációt kell létrehozni, erre használhatjuk a `dotnet ef migrations add migrációnév` parancsot (a *migrációnév* nevű migrációnak egyedinek kell lennie, érdemes értelmes nevet adni neki, pl. 'AddTotalCostColumnToProductOrder').
- Az adatbázist a legújabb migrációra a `dotnet ef update database` paranccsal frissíthetjük.
- Ha egy migrációt elrontottunk vagy szeretnénk visszavonni, használjuk a `dotnet ef migrations remove` parancsot. Ez a legutolsó migrációt törli. Ezután adjuk hozzá az új migrációt.
  - Ha a migrációt már alkalmaztuk az adatbázisra, előtte mindenképp futtassuk a `dotnet ef database update migrációnév` parancsot, ahol a *migrációnév* az **utolsóelőtti** migráció, tehát eggyel korábbi, mint amit törölni szeretnénk. Ezzel az adatbázis az előző migráció hatását visszafordítja, ezután az utolsó migráció a fenti paranccsal törölhető.
  - Ha maguk a migrációk rendben vannak, de az adatbázis adattartalmát törölni akarjuk (*kezdjük elölről*), hasznos a `dotnet ef database update 0` parancs, ami eldobja az összes migrációban érintett objektumot (pl. táblát), majd egy `dotnet ef database update` paranccsal lefuttatjuk az összes migrációt. Így egy már használható, de alaphelyzetben lévő adatbázist kapunk.
- Mivel a parancsokat a Visual Studio-tól függetlenül futtatjuk, így minden parancsfuttatás előtt érdemes minden nem mentett fájlt elmenteni, vagy fordítani a solutiont.
- Alternatívaként használhatjuk a [powershell alapú parancsokat](https://docs.microsoft.com/en-us/ef/core/cli/powershell) is a Visual Studio Package Manager Console-jából (PMC). Ilyenkor általában kevesebb paramétert kell megadnunk, mert a Visual Studio / PMC állapota alapján töltődnek.

Lássunk neki!

# Előkészületek

1. Telepítsük az [EF Core Global Tool](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet)-t: adjuk ki az alábbi parancsot egy tetszőleges parancssorban:
    - `dotnet tool install --global dotnet-ef`
      - Ha bármilyen okból kifolyólag korábban már telepítve volt, az `install` parancsot `update`-re cserélve frissíthető a tool a legfrissebb stabil verzióra.
    - Ezzel használhatók lesznek a `dotnet ef` parancsok.
1. Hozzunk létre egy új .NET (.NET 6 verziójú, a későbbiekben is) C# osztálykönyvtárat (*Class library*) **MovieCatalog.Data** néven, MovieCatalog solutionnel egy kedvenc üres munkamappánkban!
1. Adjunk a solutionhöz egy új .NET C# konzol projektet is **MovieCatalog.Terminal** néven!
1. Töröljük a létrejött helyőrző fájlt (Class1.cs) az adatréteg projektben!
1. Adjunk referenciát a konzolos projektből az adatréteg projektre! Értelemszerűen így a konzolos projektből el fogjuk érni az adatréteg típusait és API-ját, fordítva viszont nem.
1. Adjunk referenciát a `Microsoft.Extensions.Hosting` NuGet csomagra a **MovieCatalog.Terminal** projektből!
1. Adjunk referenciát a `Microsoft.EntityFrameworkCore.SqlServer` és `Microsoft.EntityFrameworkCore.Design` NuGet csomagokra a **MovieCatalog.Data** projektből!
1. Állítsuk be a konzolos projektet Startup projektként, így F5 (Start with Debugging) hatására ez fog elindulni. Ezzel a projekt neve félkövér lesz.

Ha mindent jól csináltunk, az alábbiakat kell látnunk a projektszerkezetben (a verziószámok lehetnek nagyobbak):

![Előkészületek végi állapot](./images/elokeszuletek-vege.png)

# Alap infrastruktúra kialakítása, tesztelése

1. Hozzunk létre egy új mappát az adatrétegben *Entities* néven, és adjuk hozzá a `Title` entitást:
``` C#
namespace MovieCatalog.Data.Entities
{
    public class Title
    {
        public int Id { get; set; }
        public string TConst => $"tt{Id.ToString().PadLeft(7, '0')}";
        public string PrimaryTitle { get; set; }
        
        public Title(string primaryTitle)
        {
            PrimaryTitle = primaryTitle;
        }
    }
}
```
  - Láthatjuk, hogy a `TConst` mező számított érték, az IMDb elnevezési konvenciója alapján `tt1234567` formátumban van, de mi csak a számértéket tároljuk majd az adatbázisban.

2. Hozzunk létre egy új DbContext típust az adatrétegben `MovieCatalogDbContext` néven, az alábbi tartalommal:

``` C#
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieCatalog.Data.Entities;

namespace MovieCatalog.Data
{
    public class MovieCatalogDbContext : DbContext
    {
        public MovieCatalogDbContext(ILogger<MovieCatalogDbContext> logger
                                      ,DbContextOptions<MovieCatalogDbContext> options) : base(options)
        {
            Logger = logger;
        }

        private ILogger<MovieCatalogDbContext> Logger { get; }

        public DbSet<Title> Titles => Set<Title>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Title>(title =>
            {
                title.Property(t => t.Id).ValueGeneratedNever();
                title.Property(t => t.PrimaryTitle)
                    .HasMaxLength(500);
                title.HasIndex(t => t.PrimaryTitle);
            });
        }
    }
}
```

A `Title` entitásunkon konfiguráltuk az `Id` és `PrimaryTitle` tulajdonságokat (adatbázistábla mezőket):
- Az `Id` nevű mező konvenció szerint [adatbázis által generált](https://docs.microsoft.com/en-us/ef/core/modeling/generated-properties?tabs=data-annotations#primary-keys), mi most viszont kézzel szeretnénk megadni (az IMDb-ből fog érkezni).
- A címben gyakran szeretnénk keresni, ezért indexeljük.
  - Az EF alapértelmezetten **NVARCHAR(max)** típusú string mezőket [hoz nekünk létre](https://docs.microsoft.com/en-us/ef/core/modeling/entity-properties?tabs=data-annotations%2Cwithout-nrt#column-data-types).
  - Az indexelés SQL szerveren csak bajosan alkalmazható **NVARCHAR(max)**, azaz nem korlátozott hosszúságú méretű mezőkön (ugyanis azok nem a rekordban, hanem a rekordhoz hivatkozva tárolódnak). Ezért be kell állítanunk a maximális címhosszt, és vannak igen hosszú című filmek/videók.
- Az EF alapértelmezett konvencióként a mezők nullozhatóságát [a leképzendő property típusának nullozhatósága adja](https://docs.microsoft.com/en-us/ef/core/modeling/entity-properties?tabs=data-annotations%2Cwithout-nrt#conventions). A `string` típus .NET 6-os verzió óta alapértelmezetten nem nullozhatóként van számon tartva, így az adatbázisbeli kötelezőséget külön nem kell beállítanunk.

3. A migráció létrehozásához szükséges a CLI tudtára adni, hogy milyen adatbázismotorra készítse a migrációkat (más migráció készül pl. SQL Serverre mint SQLite-ra). Hozzunk létre egy Design nevű mappát a Data projektben, benne az alábbi Factory osztályt, ami egy `DbContext`et tud gyártani nekünk. A factory-t "éles" futás közben nem használja semmi, kizárólag a migrációs fájlok elkészítése miatt szükséges most nekünk. A connection stringet az éles alkalmazás nem ezt a factory-t használva fogja átadni. Láthatjuk, hogy ez az osztály nem is használható (szabályosan) más szerelvényekből, mert `internal` láthatóságú. Értelemszerűen a connection string cserélendő, ha nem LocalDB adatbázison készül az alkalmazás, de alapértelmezetten és a laborokban az teljesen megfelelő.

``` C#
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace MovieCatalog.Data.Design
{
    internal class MovieCatalogDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MovieCatalogDbContext>
    {
        public MovieCatalogDbContext CreateDbContext(string[] args) =>
            new(new Logger<MovieCatalogDbContext>(new LoggerFactory()), 
                new DbContextOptionsBuilder<MovieCatalogDbContext>()
                    .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MovieCatalog")
                    .Options
                );
    }
}
```

4. Készítsünk migrációt, majd futtassuk le azt az adatbázison! Terminálban/PowerShell ablakban adjuk ki az alábbi parancsokat (Visual Studio-ban és Code-ban is a `Ctrl+ö` billentyűkombináció nyit egy Developer PowerShell ablakot) a **Data projekt mappájából**:
- `dotnet ef migrations add TitlesTable`
- `dotnet ef database update`

![Migrációk elkészítése és futtatása](images/migraciok-vege.png)

5. Készítsük el a konzol alkalmazást reprezentáló osztályt a Terminal projektben.

``` C#
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieCatalog.Data;
using System.Threading;
using System.Threading.Tasks;

namespace MovieCatalog.Terminal
{
    public class TestConsole : IHostedService
    {
        public TestConsole(MovieCatalogDbContext dbContext, IHost host, ILogger<TestConsole> logger)
        {
            DbContext = dbContext;
            Host = host;
            Logger = logger;
        }

        private MovieCatalogDbContext DbContext { get; }
        private IHost Host { get; }
        private ILogger<TestConsole> Logger { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Started.");
            
            // TODO: Ide jön az alkalmazás kódja.

            await Host.StopAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Stopping...");
            return Task.CompletedTask;
        }
    }
}
```

- Láthatjuk, hogy a `TestConsole` osztály számít rá, hogy kapni fog *valahonnan* egy `MovieCatalogDbContext` példányt, tehát felkészültünk arra, hogy a rendszer dependency injectiont használ.
- Érdekesség a `cancellationToken` névre hallgató paraméter. Ez egy aktiválható token, amit átpasszolhatunk további aszinkron kéréseknek, pl. a fenti `StopAsync`-nak. Ez azt eredményezi, hogy ezek a hívások megvizsgálatják, valaki "nyomott-e mégsemet" a láncban feljebb, és ha igen, akkor abbahagyják a futást. Nem szükséges használni, de szép, szofisztikált pattern, jó tudni róla. Ha egy függvényt írunk, ami `CancellationToken`-t kap, akkor a tokent illik továbbpasszolni azt minden általunk hívott függvénynek (ha van olyan változata, ami fogad ilyen paramétert).

6. Készítsük el a konzolt kiszolgáló részt az alkalmazásban. A legelegánsabb megoldás az ASP.NET-tel analóg módon egy GenericHostBuilder osztály segítségével elkészíteni a hosztkészítő objektumot, majd az megépíteni és elindítani. Cseréljük le a Program.cs fájl teljes tartalmát az alábbira:

``` C#
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieCatalog.Data;
using MovieCatalog.Terminal;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services.AddDbContext<MovieCatalogDbContext>(o => 
                    o.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MovieCatalog"))
                .AddHostedService<TestConsole>())
    .ConfigureLogging(l => l.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning))
    .Build();

await host.RunAsync();
```

A fenti indítási módszer analóg a Háttéralkalmazásokból tanult indítási móddal az ASP.NET Core kapcsán, a kivétel az indítás módjában rejlik: itt most nem egy HTTP-t kiszolgálni képes hosztot, hanem "csak" egy konzolalkalmazást indítunk.

:bulb: Ha meg akarjuk nézni az EF által generált SQL-t, állítsuk át a naplózási szintet a `ConfigureLogging` hívásban `LogLevel.Information`-re.

# Feladat 1

Szúrj be egy rekordot a Titles táblába a terminál alkalmazásból, melyben a cím a Neptun kódod! Készíts képernyőképet az ezt megvalósító kódrészletről, valamint igazold annak a tényét, hogy a rekord beszúrásra került az alábbi két módszerrel (mindkettővel!):
- SQL alapú megoldással (pl. *SQL Server Object Explorer*ben futtatott lekérdezéssel), ÉS 
- a konzol alkalmazásban történő újbóli lekérdezéssel, a konzolra (`Logger` példányra `LogInformation` hívással) történő kiírással!

## Következő feladat

Folytasd a [következő feladattal](Feladat-2.md).
