# Előkészítés

Az [Entity Framework laboron](../ef/README.md) készült adatmodellt (kissé kibővítve) fogjuk hasznosítani, hogy egy
RESTful API-t készítsünk ASP.NET Core-ban.

1. Hozz létre egy új C# nyelvű ASP.NET Core Web API (nem Web App!) típusú projektet `MovieCatalog.Api` néven.
    - Érdemes a laborgépeken kikapcsolni a *Configure for HTTPS lehetőséget*, mert a gépekre nem biztos, hogy tudjuk
      telepíteni a fejlesztéshez szükséges tanúsítványt. Saját gépeken ilyen probléma nem lesz, viszont az első
      indításkor el kell fogadni a tanúsítvány telepítését a kettő megjelenő ablakban.
    - .NET verzió: 6.0
    - Minden extra opció legyen kikapcsolva, kivéve...
        - *Use controllers*
        - *Enable OpenAPI support* – ezzel egy, a műveleteink metaadata alapján
          generálódó [Swagger UI tesztoldalt](https://swagger.io/tools/swagger-ui/) kapunk
    - Az *Authentication type* is *None* legyen.
1. Nem lesz szükség a létrejött projektben az alábbi fájlokra, ezek törölhetőek:
    - `Controllers/WeatherForecastController.cs`
    - `WeatherForecast.cs`
1. Töltsd le az alábbi DACPAC fájlt [innen](./data/imdbtitles_sample.dacpac). A DACPAC egy hordozható exportformátum MS
   SQL Server adatbázisok számára.
1. Csatlakozz egy LocalDB-példányhoz a Visual Studio-s SQL Server Object Explorerben. Ezután a **Databases** mappára
   jobb klikk, majd válaszd *Publish Data-tier Application* opciót. Tallózd be a DACPAC fájlt és add meg adatbázis
   nevét, ami legyen a Neptun-kódod, majd mehet a [
   *Publish*](https://learn.microsoft.com/en-us/sql/ssdt/extract-publish-and-register-dacpac-files?view=sql-server-ver16#publish-data-tier-application).
   Ezzel telepíted a DACPAC fájlban lévő objektumokat, adatokat az adatbáziskiszolgálóra. Import után érdemes
   ráfrissíteni az adatbázisok listájára.
1. Add hozzá a fejlesztésre szánt kapcsolódási stringet az `appsettings.Development.json` fájlhoz (az
   `appsettings.json` "mögött" bújik meg). A beállítás neve is legyen a Neptun-kódod (pontosabban: `DBneptunkód`).
    ```json
    {
        "ConnectionStrings": {
            "DB<Neptun-kódra írj át>": "Server=(localdb)\\mssqllocaldb;Database=<Neptun-kódra írj át>;Trusted_Connection=True;MultipleActiveResultSets=true"
        },
        "Logging": { ... }
    }
    ```
    - **Ha már korábbról van ugyanilyen névvel adatbázisunk, azt érdemes törölni, vagy más néven elnevezni a connection
      stringben az adatbázist, hogy ne akadjanak össze.**
1. Add hozzá a projekthez a `Microsoft.EntityFrameworkCore.SqlServer` NuGet-csomag `6.0.35`-ös verzióját.
1. Add hozzá az előre elkészített [entitásmodell- és adatbáziskontextus-fájlokat](./snippets/Entities) a projektedhez
   egy új `Entities` könyvtárba. Ehhez
   érdemes [letölteni ezt a git repót](https://github.com/VIAUBC01/labor-leirasok/archive/refs/heads/master.zip). A
   DACPAC adatbázis sémája megfelel az EF modellnek, és mivel nem módosítunk rajta, így EF migrációval ezen mérés
   keretében nem kell foglalkozni.
1. Regisztráld az adatbáziskontextust a DI-rendszerbe. (`Program.cs`)
1. Add hozzá a projekthez az előkészített kivételosztályokat [innen](./snippets/Exceptions) egy új `Exceptions` mappába.
1. Add hozzá a projekthez az előkészített `GenreService` és az `IGenreService` osztályokat [innen](./snippets/Services)
   egy új `Services` mappába.
1. Regisztráld az `IGenreService`-t a DI-rendszerbe [
   *scoped* életciklussal](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectionserviceextensions.addscoped?view=dotnet-plat-ext-6.0&viewFallbackFrom=net-6.0#microsoft-extensions-dependencyinjection-servicecollectionserviceextensions-addscoped-2(microsoft-extensions-dependencyinjection-iservicecollection)). (
   `Program.cs`)

## Általános szabályok

- A kontroller nem használhatja adatbáziselérésre a kontextust, csak a [Services mappában](./snippets/Services)
  található `IXXXService` interfész műveleteit, közvetetten pedig a *XXXService* függvényeit.
- A kontroller közvetlenül nem példányosíthatja az `XXXService`-t, csak konstruktoron keresztül kaphatja `IXXXService`
  -ként
- A kontroller függvényei (azaz a műveletek) minden esetben
    - aszinkronok (`async`), de a nevüknek nem kell `Async`-ra végződni
    - `Task<ActionResult>` vagy `Task<ActionResult<T>>` visszatérési értékűek, ahol a `T` kollekció is lehet
- Az `XXXService` osztályok a különleges eseteket ([unhappy path](https://en.wikipedia.org/wiki/Happy_path))
  kivételdobással jelzik a hívó felé. A szükséges kivételtípusok már implementálva vannak a projektben,
  az [Exceptions mappából](./snippets/Exceptions) másoltuk be őket.
- Az `XXXService` osztályokban minden szükséges metódus **váza** megtalálható, de nem minden metódus van implementálva,
  a hiányzókat implementálnod kell legkésőbb a kapcsolódó feladat megoldásakor.

# 1. feladat

Készíts egy új API-kontrollert `GenresController` néven! A kontroller az alábbi műveleteket tudja elvégezni:

- `GET /api/genres`
    - az összes műfaj lekérdezése,
    - 200-as HTTP válaszkóddal tér vissza ([Ok](https://httpstatusdogs.com/200-ok)), a válasz törzsben a műfajok
      sorosított listájával
- `GET /api/genres/<ID>`
    - a megadott ID-jű `Genre` objektum lekérdezése,
    - ha az ID azonosítójú elem nem található, visszatérés `404`
      -gyel ([Not found](https://httpstatusdogs.com/404-not-found))
    - egyébként `200`-as HTTP-válaszkóddal tér vissza ([OK](https://httpstatusdogs.com/200-ok)), a válasz törzsében az
      adott ID-jű sorosított `Genre` objektum

## Általános tudnivalók, megjegyzések, tippek

- Az adatbázis sémája szinte megegyezik az EF laboron megismerttel, kivéve:
    - új index a `Title.StartYear` oszlopra
    - az új művek azonosítóját az adatbázis osztja ki
    - az ezen órai modell pont a fentiek miatt nem kompatibilis az előző órai adatbázissal
- Az `XXXService` osztályok a kivételes eseteket kivételdobással kezelik (pl. a megadott ID-val nem található elem
  `ObjectNotFoundException<>` dobást eredményez)
- Kiinduló kontroller
  kódot [lehet generáltatni](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio#scaffold-a-controller).
  Ehhez a laborhoz az **API controller with read/write actions** generátor az ajánlott, az Entity Framework-ös
  generátorok gyakran hibára futnak és egyébként is körülbelül a generált kód ugyanannyi részét kellene átírni.
- Sokszor körülményesebb az IIS Express-en történő debuggolás, helyette használhatod közvetlenül a Kestrel szervert is.
  Ehhez a zöld play gomb melletti menüben a projekt nevét viselő lehetőséget válaszd ki! Ezután indításkor az *IIS
  Express* tálcaikon helyett egy konzolalkalmazás indul el, ami hasznos üzeneteket is kiír a konzolra.
- Régebbi .NET-en, vagy Open API/Swagger nélkül az F5 hatására a szerver elindul, automatikusan a `/weatherforecast`
  URL-re kerülünk. Mivel a szerverünknek nincsen felülete, a `WeatherForecastController`-t pedig töröltük, ezért itt egy
  404-es oldal fogad minket. Ez nem gond, de ha a kezdő URL-t szeretnéd átírni, akkor a projekten belül a
  `Properties/launchSettings.json` fájlban teheted meg (a `launchUrl` mező átírása vagy törlése).
- Módosító/beszúró műveleteknél szükség van egy elemre sorosított formában, ezt kell általában ezen műveleteknél a
  törzsben küldeni. Érdemes ezt a sorosított formát a lekérdező művelet válaszából elcsenni.

## Beadandók

- Az általad írt kódrészletekről képernyőképet kell beadni. Ezek a fájlok érintettek (de minden feladatnál csak azok,
  melyeken módosítás történt):
    - `Program.cs`
    - kontrollerek kódfájljai
    - `XXXService`-ek kódfájljai
- Minden feladathoz beadandók az alábbi tesztkérésekről készítendő képek. A képet a *Swagger UI* beépített weboldalról
  kell készíteni. A kép a *Curl* résztől a *Server response*-ig terjedő részt (a *Responses* részt már nem)
  tartalmazza (_Response headers_ és _Response body_ is, ha van!). Példák:
  ![This is an image](./images/req_p%C3%A9lda.png)
  ![This is an image](./images/req_p%C3%A9lda2.png)
    - tesztkérések
        - listázás
        - egy elem sikeres lekérdezése
        - egy nem létező elem lekérdezése

## Következő feladat

Folytasd a [következő feladattal](Feladat-2.md).
