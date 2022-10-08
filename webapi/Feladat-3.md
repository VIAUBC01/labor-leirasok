# Feladat 3.

A korábbiakkal koncepcionálisan analóg módon készítsd el a művek (`Title`) alábbi műveleteit is:
- `GET api/titles/<ID>`
- `PUT api/titles/<ID>`
- `POST api/titles`

Eltérés, hogy műveknek lehet ugyanazon neve, tehát nem lesz konfliktus, ha ugyanazt próbáljuk meg többször beszúrni. Az azonosítónak megfelelő mű meglétét viszont továbbra is ellenőrizni kell!

A `Title` entitás nem utazhat a dróton, 
- azaz nem jöhet a klienstől paraméterben
- nem szabad vele visszatérni az API végpontokon
- tehát nem hivatkozhat rá sehol a `TitlesController`  

Helyette a [Dtos mappában](./snippets/Dtos) lévő DTO-kat kell használni:

- `TitleInsertUpdateModel` ezt várjuk a klienstől módosításkor, beszúráskor
- `TitleQueryModel` ezt vagy ilyen kollekciót adunk vissza a kliensnek lekérdezéskor

A DTO-k DTO <=> Entitás konvertáló függvényt/konstruktort is tartalmaznak.

Adateléréshez a [Services mappában](./snippets/Services) lévő `ITitleService` és `TitleService` típusokat használd.

## Beadandó tesztkérések

- A módosító és a beszúró végpontra egy-egy sikeres és egy tetszőleges sikertelen kérés/válasz bemutatása.
- egy elem sikeres lekérdezése
- nem létező elem lekérdezése

## Következő feladat

Folytasd a [következő feladattal](Feladat-4.md).
