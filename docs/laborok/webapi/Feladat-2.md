# Feladat 2: Műfajok

Valósítsd meg az alábbi API műveleteket:

- `PUT /api/genres/<ID>`

    * az ID azonosítójú műfaj módosítása,
    * a művelet `Genre`-t vár, de csak a műfaj közvetlen tulajdonságait (azaz egyedül a nevét) lehet módosítani, a kapcsolódó entitásokat (`TitleGenres` elemek) nem; ez volna az ún. "overposting" támadás,
    * a műfaj nem URL-ből, hanem a kérés törzséből jön,
    * sikeres visszatérés 204-es HTTP válaszkóddal ([No Content](https://httpstatusdogs.com/204-no-content)),
    * ha az ID azonosítójú elem nem található, visszatérés 404-es HTTP válaszkóddal ([Not found](https://httpstatusdogs.com/404-not-found)),
    * ha már van megadott nevű műfaj, visszatérés a 419-es HTTP válaszkóddal ([Conflict](https://httpstatusdogs.com/409-conflict)).

- `POST /api/genres`

    * új műfaj létrehozása,
    * a kontrollerfüggvény `Genre`-t vár, de csak a műfaj közvetlen tulajdonságait (azaz a nevét) lehet megadni (csak azt mentjük)
    * a műfaj nem URL-ből, hanem a kérés törzséből jön,
    * ha már van megadott nevű műfaj, visszatérés a 419-es HTTP válaszkóddal ([Conflict](https://httpstatusdogs.com/409-conflict)),
    * sikeres visszatérés 201-es HTTP válaszkóddal ([Created](https://httpstatusdogs.com/201-created)), a válasz tartalmaz egy Location header-t, aminek értéket megmutatja, hogy honnan (melyik URL-ről) kérhető le az új erőforrás. (Tipp: [CreatedAtAction függvény - ezesetben a kétparaméteres nem jó](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.createdataction?view=aspnetcore-8.0))
    
- `DELETE /api/genres/<ID>`

    * a megadott ID-jú genre objektum törlése,
    * ha az ID azonosítójú elem nem található, visszatérés 404-es HTTP válaszkóddal ([Not found](https://httpstatusdogs.com/404-not-found))
    * egyébként visszatérés 204-es HTTP válaszkóddal ([No Content](https://httpstatusdogs.com/204-no-content))

## Beadandó tesztkérések

- Beszúrás, módosítás: egy sikeres és egy tetszőleges sikertelen (szabálysértő, pl. *Not found* választ eredményező) kérés/válasz bemutatása.
- Törlés: két törlés egymás után ugyanarra az ID-ra

## Következő feladat

Folytasd a [következő feladattal](Feladat-3.md).
