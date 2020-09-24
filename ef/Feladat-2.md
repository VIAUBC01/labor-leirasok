# Adatbetöltés

Az adatbetöltést az IMDb által minden nap közzétett TSV (tabulátorral elválasztott értékek) típusú fájlok alapján végezzük. A hivatalos dokumentáció ezekről a fájlokról itt található: [IMDb Datasets (https://www.imdb.com/interfaces/))](https://www.imdb.com/interfaces/). Minket most a `title.basics.tsv.gz` fájl érdekel, ami a megfelelő adatokat tartalmazza ahhoz, hogy filmek alapadatait (cím, kiadás dátuma, műfaj) tárolhassuk.

1. A Data projektbe vegyük fel az alábbi osztályt, ami a TSV fájlok dekódolásában fog nekünk segíteni. A fájl tartalmát nem kell megértenünk, de az alapműködést igen, amit alább részletezve láthatunk.

``` C#
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace MovieCatalog.Data
{
    public class TsvParser
    {
        public static IEnumerable<IReadOnlyDictionary<string, string>> ParseTsv(string filePath)
        {
            using var reader = new StreamReader(new GZipStream(File.OpenRead(filePath), CompressionMode.Decompress));

            var headers = reader.ReadLine().Split('\t');
            string line;

            while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
            {
                yield return new Dictionary<string, string>(line.Split('\t').Select((item, index) => new KeyValuePair<string, string>(headers[index], item != "\\N" ? item : null)));
            }
        }
    }
}
```

A függvényt meghívva feldolgozás kezdődik meg a `filePath` elérési útvonalon található TSV fájlon. Az első sor a fejléc, ez tartalmazza, hogy hány érték található minden ezt követő sorban. A `\N` jelölésű sorok `null` értéket jelölnek. Ezt követően soronként ennyi értéket találunk a TSV fájlban. A fájlból soronként olvasva visszaadunk 1-1 értéket egy szótár formájában, amiből a dokumentáció alapján definiált kulccsal kiindexelhetjük a TSV fájlban található string értékeket.

2. Vegyük fel az alábbi függvényt a MovieCatalogDbContext osztályba:
``` C#
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ...

public async Task ImportFromFileAsync(string filePath, int? maxValues = 100_000)
{
    var toInsert = TsvParser.ParseTsv(filePath).Select(item => new Title
    {
        Id = int.Parse(item["tconst"][2..]), // A 'tconst' értéke a fájlban pl. 'tt6723592', a [..] range operátorral a 'tt'-t az elejéről levágjuk, a maradékot pedig int-té alakítjuk
        PrimaryTitle = item["primaryTitle"] // 'Tenet'
    });
    if (maxValues != null)
      toInsert = toInsert.Take(maxValues.Value); // Alapértelmezetten csak 100 000 elemet veszünk a fájlból összesen.

    while (toInsert.Any()) // Ha van még feldolgozatlan sor,
    {
        Titles.AddRange(toInsert.Take(100_000)); // 100 000-et felcsatolunk a Titles DbSet-be,
        toInsert = toInsert.Skip(100_000); // léptetünk egy 'oldalt',
        var saved = await SaveChangesAsync(); // elmentjük a DB-be, ami visszaadja a mentett sorok számát,
        Logger.LogInformation($"Saved {saved} rows."); // végül kiírjuk a mentett sorok számát.
    }
}
```
- Mivel a fájl és tartalma óriási, ezért 100 000-es lépésközönként szúrjuk csak be az adatbázisba az elemeket. Alapértelmezetten csak az első 100 000 elemet olvassuk csak ki a fájlból.
  - Ezért kellett, hogy ne a teljes fájlt egyszerre beolvassuk, hanem gyakorlatilag soronként streameljük a fájlból a szótárakat, azokat pedig transzformáljuk Title típusú elemekre. Ha a géped nem bírja, a maxValues értéket leveheted 10 000-re.

3. Töltsük le a gépünkre az aktuális `title.basics.tsv.gz` fájlt ([IMDb data files available for download (https://datasets.imdbws.com/, Figyelem! ~124 MB!)](https://datasets.imdbws.com/)), majd töltsük be a fájl tartalmát az adatbázisba.
  - Ha korlátos erőforrással dolgozunk, akkor a fájlból egy [jelentősen kisebb, csak az első 100 000 sort tartalmazó lenyomat található itt (~2 MB)](res/title.basics.stub.tsv.gz). Ez a fájl szintén használható, de a későbbi bónusz feladat ezzel értelemszerűen nem végezhető el.

``` C#
using Microsoft.EntityFrameworkCore;

// ...

public async Task StartAsync(CancellationToken cancellationToken)
{
    Logger.LogInformation("Starting...");

    await DbContext.Database.MigrateAsync();

    if (!await DbContext.Titles.AnyAsync())
        await DbContext.ImportFromFileAsync(@"C:\------\title.basics.tsv.gz"); // Az útvonal értelemszerűen kitöltendő.

    await Host.StopAsync();
}
```

Ezzel be is kerülnek az adatok az adatbázisba:

![Adatbetöltés vége](images/adatbetoltes-vege.png)

<hr />

# Feladat 2.

A fenti példa és a dokumentáció (https://www.imdb.com/interfaces/) alapján bővítsd a Title osztály modelljét és a betöltés során a Title objektumok előállítását az alábbi tulajdonságokkal! A tulajdonság neve C#-ban PascalCase-ben legyen, a szótárban camelCasing-ben van.

- TitleType: legyen egy saját TitleType névre hallgató enum típus a Data projektben, aminek az értékei: `Unknown`, `Short`, `Movie`, `TvMovie`, `TvSeries`, `TvEpisode`, `TvShort`, `TvMiniSeries`, `TvSpecial`, `Video`, `VideoGame`. Az Enum értéke a TSV-ben string-ként van tárolva (pl. tvShort), használd a feldolgozáshoz az `Enum.Parse` függvény megfelelő overloadját! **Gyakran keresünk ez alapján, ezért indexelni kell.**
- OriginalTitle: string érték, az eredeti nyelvű címe a műnek.
- StartYear: a kiadás évszáma. Lehet null értékű is. Számként kell tárolni. **Gyakran keresünk ez alapján, ezért indexelni kell.**
- EndYear: csak sorozatok esetén a sorozat záró részének kiadási évszáma. Lehet null értékű is. Számként kell tárolni.
- RuntimeMinutes: A futási idő percben. Lehet null is.

Természetesen szükséges új migrációt hozzáadni a projekthez és frissíteni az adatbázis sémáját. A fenti betöltés csak üres Titles tábla esetén fut le, tehát törölni kell belőle az adatokat.

Beadandó: az elkészült kód képernyőképe, ill. demonstrálandó, hogy betöltődtek az adatok (minden tulajdonság megfelelően ki van töltve).