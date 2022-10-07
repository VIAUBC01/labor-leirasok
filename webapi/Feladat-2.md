# Feladat 2.

Valósítsd meg az alábbi API műveleteket:
  - `PUT /api/genres/<ID>`
    - az ID azonosítójú műfaj módosítása,
    -   a Controller függvény `Genre`-t vár, de csak a műfaj közvetlen tulajdonságait (azaz egyedül a nevét) lehet módosítani, a kapcsolódó entitásokat (`TitleGenres` elemek) nem; ez volna az ún. "overposting" támadás,
    - a név nem URL-ből, hanem a kérés törzséből jön,
    - sikeres visszatérés 204-es válasszal,
    - ha az ID azonosítójú elem nem található, visszatérés 404-es HTTP válaszkóddal ([Not found](https://httpstatusdogs.com/404-not-found)),
    - ha már van megadott nevű műfaj, visszatérés a 419-es HTTP válaszkóddal ([Conflict](https://httpstatusdogs.com/409-conflict)).
  - `POST /api/genres`
    - új műfaj létrehozása,
    - a Controller függvény `Genre`-t vár, de csak a műfaj közvetlen tulajdonságait (azaz a nevét) lehet megadni (csak azt mentjük)
    - a név nem URL-ből, hanem a kérés törzséből jön,
    - ha már van megadott nevű műfaj, visszatérés a 419-es HTTP válaszkóddal ([Conflict](https://httpstatusdogs.com/409-conflict)),
    - sikeres visszatérés 201-gyel, a válasz tartalmaz egy Location header-t, aminek értéket megmutatja, hogy honnan (melyik URL-ről) kérhető le az új erőforrás. (Tipp: [CreatedAtAction függvény - ezesetben a kétparaméteres nem jó](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.createdataction?view=aspnetcore-6.0))
  - `DELETE /api/genres/<ID>`
    - a megadott ID-jú genre objektum törlése,
    - ha az ID azonosítójú elem nem található, visszatérés 404-es HTTP válaszkóddal ([Not found](https://httpstatusdogs.com/404-not-found))
  - Az első feladatban elkészült két `GET`-es kérés sikeres eredményeképpen előálló objektumokban legyen benne az is egy `NumberOfTitles` nevű tulajdonságban, hogy hány mű tartozik a műfajba!

Beadandó:
- Az érintett kódok a szokásos formában.
- Mindegyik módosító végpontra egy sikeres és egy tetszőleges sikertelen kérés/válasz bemutatása.
- A GET-es kérés bemutatása egyetlen és több elem lekérdezésére.

## Következő feladat

Folytasd a [következő feladattal](Feladat-3.md).
