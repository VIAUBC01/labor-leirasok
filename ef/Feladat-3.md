# 3. feladat

A művekhez műfajok tartoznak. A műfajok a `genres` kulcsú elemben találhatók vesszővel elválasztva. **A műfajokat saját táblában kell tárolni, hogy kevesebb helyet fogyasszanak a diszken és ne legyen redundáns a tárolásuk.** A műfajról tároljuk az azonosítóját (szám), amit az adatbázis oszt majd ki, valamint a nevét, ami kötelező, és **egyedinek** kell lennie, és **maximum 50 karakter hosszúságú** lehet. Egy film több műfajban is lehet, egy műfajhoz több film is tartozhat. Mindkét irányban navigation property-t kell definiálni a műfaj és a mű között. Egy filmhez egy műfaj csak egyszer tartozhat!

## Tippek

:bulb: Az adatbázisban csak 1-többes kapcsolat létezik, több-többes kapcsolatot (many-to-many relationship) kapcsolótábla bevezetésével és 1-többes kapcsolatokkal oldjuk meg. C# szinten vagy egy-az-egyben ezt a modellt képezzük le (explicit több-többes kapcsolat), vagy csak egyszerűen a két eredeti entitásba veszünk fel kollekciókat, és csak az EF szinten jelenik meg a kapcsolótábla ([implicit több-többes kapcsolat](https://learn.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#many-to-many)). Bár az utóbbi egyszerűbb és kényelmesebb, de korlátozottabb, nehezebben bővíthető.

:bulb: Explicit esetben a `Title`-ön kívül két entitást kell definiálni:
  - az egyik maga a műfaj (`Genre`), amiben az azonosító és a név tulajdonságok szerepelnek, 
  - a másik a kapcsolótábla, `TitleGenre` névre hallgató entitás lehet, amiben egy egyedi azonosító van, és két külső kulcs: egy `TitleId` és egy  `GenreId`,
  - a `TitleGenre`-ban definiáljunk a külső kulcsokhoz egy-egy navigation property-t, a `Title` és `Genre` entitásokban pedig egy-egy `ICollection<TitleGenre>` típusút.

:bulb: Hogy ne kapj fordítási figyelmeztetést `null` értékek kezelése miatt, a kollekció típusú navigációs property-ket mindig inicializáld üres listára, míg a nem kollekció típusú navigációs property-ket explicit `null` értékre a null forgiving operátorral. Példa az utóbbira: `public Genre Genre { get; set; } = null!;`

:bulb: Ellenőrizd, hogy a `Genre` és `TitleGenre` létrejön-e táblaként a DB-ben! Ha nem többesszámú a neve, akkor valószínűleg kihagytad a `DbSet` definícióját a `MovieCatalogDbContext`-ből.

:bulb: Fontos, hogy egy mű egy műfajban csak egyszer szerepelhet, ezért [unique indexet](https://learn.microsoft.com/en-us/ef/core/modeling/indexes?tabs=fluent-api) kell létrehozni a `TitleGenre` entitásban a két külső kulcsra (együttesen)

```csharp
modelBuilder.Entity<TitleGenre>()
            .HasIndex(tg => new { tg.GenreId, tg.TitleId })
            .IsUnique()
```

:bulb: Betöltéskor az alábbi megoldással kevés kóddal megoldhatjuk az új táblák feltöltését
  1. A `ImportFromFileAsync` függvény elején vegyünk fel egy `Dictionary<string, Genre>`-t, amibe folyamatosan gyűjtjük a `Genre` példányokat és a műfaj neve a kulcs, így név alapján gyorsan tudunk keresni benne
  1. Elég csak az új `Title` példány kitöltésekor az új `TitleGenres` property-t kitölteni, az EF automatikusan felveszi a kapcsolódó entitásokat, ha a navigációs property-k ki vannak töltve (ráadásul kapcsolatonként elég az egyik irányt kitölteni) nem kell külön `Add` hívás. A *genres* mező szövegét [feldaraboljuk](https://learn.microsoft.com/en-us/dotnet/api/system.string.split?view=net-6.0#system-string-split(system-char-system-stringsplitoptions)) a vesszők mentén, az így keletkező `string[]` darablista minden elemét leképezzük egy új `TitleGenre` példányra, amiben megint csak elég csak a `Genre` navigációs property-t kitölteni. Itt kap szerepet az előző lépésben felvett szótár, mert kikeressük belőle, hogy az aktuális műfajszöveghez vettünk-e már fel `Genre` példányt: ha igen, akkor ezt a példányt adjuk meg a `Genre` értékének; ha még nincs ilyen példány, akkor egyrészt felvesszük a szótárba új példányként, másrészt ugyanezt az új példányt adjuk meg a `Genre` értékének is. Ez a két művelet, azaz a `Genre` property beállítása egyetlen összetett kifejezéssel is lehetséges, így a LINQ kifejezésben könnyen felhasználható (`ng` az aktuális műfaj, még szövegként):
```csharp
Genre = genres.TryGetValue(ng, out var g) ? g : genres[ng] =new Genre(ng)
```
:bulb: Futtatás előtt érdemes most is törölni a **Titles** táblát, de a TRUNCATE nem használható olyan táblákon, amire idegen kulcs hivatkozás van (a **Titles** tábla már ilyen), helyette a `DELETE FROM táblanév` használható. Minden tábla kitörlésére/eldobására használható a `dotnet ef database update 0` parancssorból - ez kitörli minden táblánkat, majd a normál `dotnet ef database update` létrehozza őket üresen.

## Beadandó

Az adatmodell kódjáról készült képek, a betöltött adatokat reprezentáló képek (a kapcsolótábla és az új tábla tartalmai) és az adatbázis sémáját reprezentáló képek (legyenek láthatók a táblák, azok oszlopai, indexei, kulcsai pl. SQL Server Object Explorerben vagy SSMS-ben).

**Bónusz (nehéz!)**: egy jegy javítás kapható, ha [a teljes adathalmazt tartalmazó fájl](https://datasets.imdbws.com/) betöltése megtörténik! Ekkor beadandó még rövid magyarázat a megoldás módjáról és tetszőleges módon demonstrálandó, hogy ténylegesen bekerült adatbázisba az összes adat (kimutatható minden táblának a számossága).

## Következő feladat

Folytasd a [következő feladattal](Feladat-4.md).
