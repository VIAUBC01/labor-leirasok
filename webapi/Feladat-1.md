# Előkészítés

Az [Entity Framework laboron](../ef/README.md) készült adatmodellt (kissé kibővítve) fogjuk hasznosítani, hogy egy RESTful API-t készítsünk ASP.NET Core-ban.

1. Hozz létre egy új C# nyelvű ASP.NET Core Web API típusú projektet `MovieCatalog.Api` néven
    - Érdemes a laborgépeken kikapcsolni a *Configure for HTTPS lehetőséget*, mert a gépekre nem biztos, hogy tudjuk telepíteni a fejlesztéshez szükséges tanúsítványt. Saját gépeken ilyen probléma nem lesz, viszont az első indításkor el kell fogadni a tanúsítvány telepítését a kettő megjelenő ablakban.
    - .NET verzió: 6.0
    - Minden extra opció legyen kikapcsolva, kivéve 
      - *Use controllers*
      - *Enable OpenAPI support* - ezzel a műveleteink metaadata alapján generálódó [Swagger UI tesztoldalt](https://swagger.io/tools/swagger-ui/) kapunk
    - *Authentication type* is *None* legyen
    
1. Nem lesz szükség a létrejött projektben az alábbi fájlokra, ezek törölhetők:
    - `Controllers/WeatherForecastController.cs`
    - `WeatherForecast.cs`

1. Töltsd le az alábbi DACPAC fájlt [innen](./data/moviecatalogdb.dacpac). Ami egy hordozható export formátum MS SQL Server adatbázisok számára.

1. Csatlakozz egy LocalDB példányhoz az SQL Server Object Explorerben. A **Databases** mappán jobbklikk, majd válaszd *Publish Data-tier Application* opciót. Tallózd be a DACPAC fájlt és add meg adatbázis nevét, ami legyen a neptun kódod, majd mehet a [*Publish*](https://learn.microsoft.com/en-us/sql/ssdt/extract-publish-and-register-dacpac-files?view=sql-server-ver16#publish-data-tier-application). Ezzel telepíted a DACPAC fájlban lévő objektumokat, adatokat az adatbázis kiszolgálóra.

1. Add hozzá a fejlesztésre szánt kapcsolódási stringet az appsettings.Development.json fájlhoz (az appsettings.json "mögött" bújik meg). A beállítás neve is legyen a neptun kódod (pontosabban *DBneptunkód*).

    ``` JSON
    {
        "ConnectionStrings": {
            "DB<neptun kódra írj át>": "Server=(localdb)\\mssqllocaldb;Database=<neptun kódra írj át>;Trusted_Connection=True;MultipleActiveResultSets=true"
        },
        "Logging": { ... }
    }
    ```

    - **Ha már korábbról van ugyanilyen névvel adatbázisunk, azt érdemes törölni, vagy más néven elnevezni a connection stringben az adatbázist, hogy ne akadjanak össze.**

1. Add hozzá az előre elkészített [entitásmodell és adatbázis kontextus fájlokat](./snippets/Entities) a projektedhez egy új Entities könyvtárba. Ehhez érdemes [letölteni ezt a git repot](https://github.com/VIAUBC01/labor-leirasok/archive/refs/heads/master.zip).

1. Regisztráld a kontextust a DI rendszerbe. (Program.cs)

1. Add hozzá a projekthez az előre elkészített kivétel osztályokat [innen](./snippets/Exceptions) egy új *Exceptions* mappába. 

1. Add hozzá a projekthez az előre elkészített `GenreService` és az `IGenreService` osztályokat [innen](./snippets/Services) egy új *Services* mappába. 

1. Regisztráld az `IGenreService`-t a DI rendszerbe [*scoped* életciklussal](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectionserviceextensions.addscoped?view=dotnet-plat-ext-6.0&viewFallbackFrom=net-6.0#microsoft-extensions-dependencyinjection-servicecollectionserviceextensions-addscoped-2(microsoft-extensions-dependencyinjection-iservicecollection)). (Program.cs)

# Általános szabályok

- A kontroller nem használhatja adatbáziselérésre a kontextust, csak a *IXXXService* interfész műveleteit.
- A kontroller függvényei (azaz a műveletek)
    - aszinkronok (`async`), de a nevüknek nem kell `Async`-ra végződni
    - `Task<ActionResult>` vagy `Task<ActionResult<T>>` visszatérési értékűek, ahol a `T` kollekció is lehet

# Feladat 1.

Készíts egy új API kontrollert `GenresController` néven! A controller az alábbi műveleteket tudja elvégezni:
- `GET /api/genres`
  - az összes műfaj lekérdezése,
  - 200-as HTTP válaszkóddal tér vissza ([Ok](https://httpstatusdogs.com/200-ok)), a válasz törzsben a műfajok sorosított listájával
- `GET /api/genres/<ID>`
  - a megadott ID-jú `Genre` objektum lekérdezése,
  - ha az ID azonosítójú elem nem található, visszatérés 404-gyel ([Not found](https://httpstatusdogs.com/404-not-found))
  - egyébként 200-as HTTP válaszkóddal tér vissza ([Ok](https://httpstatusdogs.com/200-ok)), a válasz törzsében az adott ID-jú sorosított `Genre` objektum

## Beadandó
- Az elkészült kontroller kódjáról készült kép(ek).
- 3 képernyőkép, ahol a 3 feltételnek megfelelő kérésre érkező válaszokat láthatjuk tetszőleges böngészőből vagy a Swagger UI tesztoldalról.

# Tudnivalók, megjegyzések, tippek

(A teljes laborra vonatkoznak)

- Az adatbázis szinte sémája szinte megegyezik az EF laboron megismerttel, kivéve:
  - új mezők kerültek be a művekhez
  - az új művek azonosítóját az adatbázis osztja ki
- A XXXService osztályok a kivételes eseteket kivétel dobással kezelik (pl. a megadott ID-val nem található elem)
- Kiinduló kontroller kódot [lehet generáltatni](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio#scaffold-a-controller), de ehhez a laborhoz az **API controller with read/write actions** generátor az ajánlott, az Entity Framework-ös generátorok gyakran hibára futnak és egyébként is körülbelül a generált kód ugyanannyi részét kellene átírni
- Sokszor körülményesebb az IIS Express-en történő debuggolás, helyette használhatod közvetlenül a Kestrel szervert is. Ehhez a zöld play gomb melletti menüben a projekt nevét viselő lehetőséget válaszd ki! Ezután indításkor az *IIS Express* tálcaikon helyett egy konzolalkalmazás indul el, ami hasznos üzeneteket is kiír a konzolra.
- Régebbi .NET-en, vagy Open API/Swagger nélkül az F5 hatására a szerver elindul, automatikusan a */weatherforecast* URL-re kerülünk. Mivel a szerverünknek nincsen felülete, a `WeatherForecastController`t pedig töröltük, ezért itt egy 404-es oldal fogad minket. Ez nem gond, de ha a kezdő URL-t szeretnéd átírni, akkor a projekten belül a Properties/launchSettings.json fájlban teheted meg (`launchUrl` mező átírása vagy törlése).
- Módosító/beszúró műveleteknél szükség van egy elemre sorosított formában, ezt kell általában ezen műveleteknél a törzsben küldeni. Érdemes ezt a sorosított formát a lekérdező művelet válaszából elcsenni.

## Következő feladat

Folytasd a [következő feladattal](Feladat-2.md).
