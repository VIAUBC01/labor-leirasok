# Feladat 2.

Egészítsd ki/módosítsd a GenresControllert az alábbiaknak megfelelően:
  - `PUT /api/genres/<ID>`
    - az ID azonosítójú műfaj módosítása,
    - csak a műfaj közvetlen tulajdonságait (azaz egyedül a nevét) lehet módosítani, a kapcsolódó entitásokat (`TitleGenres` elemek) nem; ez volna az ún. "overposting" támadás,
    - a név nem URL-ből, hanem a kérés törzséből jön,
    - sikeres visszatérés 204-es válasszal,
    - ha az ID azonosítójú elem nem található, visszatérés 404-gyel,
    - ha már van megadott nevű műfaj, visszatérés a Conflict HTTP státusszal.
  - `POST /api/genres`
    - új műfaj létrehozása,
    - csak a műfaj közvetlen tulajdonságait (azaz a nevét) lehet megadni (csak azt mentjük),
    - a név nem URL-ből, hanem a kérés törzséből jön,
    - ha már van megadott nevű műfaj, visszatérés a Conflict HTTP státusszal,
    - sikeres visszatérés 201-gyel, aminek válaszában megtalálható, hogy honnan (melyik URL-ről) kérhető le az új erőforrás.
  - `DELETE /api/genres/<ID>`
    - a megadott ID-jú genre objektum törlése,
    - sikeres visszatérés 204-gyel és **akkor is**, ha nem található az ID azonosítójú elem.
  - Az első feladatban elkészült két `GET`-es kérés sikeres eredményeképpen előálló objektumokban legyen benne az is egy `NumberOfTitles` nevű tulajdonságban, hogy hány mű tartozik a műfajba!

Beadandó:
- Az érintett kódok a szokásos formában.
- Mindegyik módosító végpontra egy sikeres és egy tetszőleges sikertelen kérés/válasz bemutatása.
- A GET-es kérés bemutatása egyetlen és több elem lekérdezésére.

## Következő feladat

Folytasd a [következő feladattal](Feladat-3.md).
