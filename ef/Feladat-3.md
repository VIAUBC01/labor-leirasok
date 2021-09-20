# Feladat 3

A művekhez műfajok tartoznak. A műfajok a `genres` kulcsú elemben találhatók vesszővel elválasztva. **A műfajokat saját táblában kell tárolni, hogy kevesebb helyet fogyasszanak a diszken és ne legyen redundáns a tárolásuk.** A műfajról tároljuk az azonosítóját (szám), amit az adatbázis oszt majd ki, valamint a nevét, ami kötelező, egyedinek kell lennie, és maximum 16 karakter hosszúságú lehet. Egy film több műfajban is lehet, egy műfajhoz több film is tartozhat. Mindkét irányban navigation property-t kell definiálni a műfaj és a mű között. Egy filmhez egy műfaj csak egyszer tartozhat!

Tippek: 
- Több-többes kapcsolat esetén kapcsolótábla létrehozása javasolt, ami önálló entitás lesz! 
- Minden kapcsolat az egyszerű relációs adatbázisban 1-többes ("natív" több-többes kapcsolat nem létezik), ami azt jelenti, hogy a kapcsolódó entitások közül az egyikben mindig egy `ICollection<BEntity>`, a másikban pedig `AEntity` típusú tulajdonság van. 
- A Title-ön kívül két entitást kell definiálni:
  - az egyik maga a műfaj (Genre), amiben az Id és Name tulajdonságok szerepelnek, 
  a másik a kapcsolótábla, TitleGenre névre hallgató entitás lehet, amiben egy egyedi azonosító van, és két külső kulcs: egy TitleId és egy GenreId,
  - a TitleGenre-ban definiáljunk egy Title és egy Genre típusú navigation property-t, a hivatkozottakban pedig egy `ICollection<TitleGenre>` típusút (inicializálni ezeket nem kell, lekérdezéskor lesznek feltöltve adattal).
- Ellenőrizd, hogy a Genre és TitleGenre létrejön-e táblaként a DB-ben! Ha nem többesszámú a neve, akkor valószínűleg kihagytad a DbSet definícióját a DbContext-ből.
- Fontos, hogy egy mű egy műfajban csak egyszer szerepelhet, ezért Unique Indexet kell létrehozni a TitleGenre entitásban a két külső kulcsra (együttesen)
  - `titleGenre.HasIndex(tg => new { tg.GenreId, tg.TitleId }).IsUnique();`
- Jó, ha nem rontjuk el rögtön és engedjük, hogy több műfaj kerüljön a DB-be ugyanazzal a névvel, így az is jó, ha a műfajnév egyedi. Sőt, indexelni nem is lehet NVARCHAR(max) típusú mezőt, ezért legyen reálisan rövid a max. hossza (mondjuk 15 karakter hosszú)!
- Betöltéskor gyűjtsük a Genre típusú entitásokat egy lokális `string-Genre` szótárba; ha még nincs, adjuk hozzá; ha már van, vegyük ki!
``` C#
var genres = new Dictionary<string, Genre>();
Genre = genres.TryGetValue(genre, out var g) ? g : genres[genre] = new() { Name = genre }
```
- Használhatjuk az alábbi segédfüggvényt opcionális int-ek parse-olásához (újabb C# verziókban a függvényeken belül is definiálhatunk függvényeket):
  - `static int? ParseIntOrNull(string text) => text == null ? null : int.Parse(text);`

Beadandó: az új adatmodell kódjáról készült képek, a betöltött adatokat reprezentáló képek (a kapcsolótábla és az új tábla tartalmai) és az adatbázis sémáját reprezentáló képek (legyenek láthatók a táblák, azok oszlopai, indexei, kulcsai pl. SQL Server Object Explorerben vagy SSMS-ben).

**Bónusz (nehéz!)**: egy jegy javítás kapható, ha a teljes adathalmaz betöltése megtörténik! Ekkor beadandó még rövid magyarázat a megoldás módjáról és tetszőleges módon demonstrálandó, hogy ténylegesen bekerült adatbázisba az összes adat (kimutatható minden táblának a számossága).

## Következő feladat

Folytasd a [következő feladattal](Feladat-4.md).
