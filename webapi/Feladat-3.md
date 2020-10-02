# Feladat 3.

A korábbiakkal koncepcionálisan analóg módon készítsd el a művek (Title) műveleteit is (kivéve az összes mű lekérdezését):
- `GET api/titles/<ID>`
- `PUT api/titles/<ID>`
- `POST api/titles`
- `DELETE api/titles/<ID>`

A `Title` entitás nem utazhat a dróton, azaz nem jöhet paraméterben és nem szabad vele visszatérni az API végpontokon! Saját modell objektum létrehozásával kell megoldani a kezelését. A transzformációkat minden irányban neked kell megvalósítanod.

Eltérés, hogy műveknek lehet ugyanazon neve, tehát nem lesz konfliktus, ha ugyanazt próbáljuk meg többször beszúrni.

A modellben legyen benne minden információ, ami az entitásban is megtalálható! Az egyetlen eltérés a műfajokban van: nem `TitleGenre` entitások, hanem csak a hozzá tartozó műfajok ID-ja szerepeljen a modellben! Törekedj rá, hogy a transzformációs logikát ne duplikáld sehol a kódban, próbáld kiszervezni (például saját szolgáltatással, a Controlleren definiált segédfüggvénnyel, copy constructor használatával stb.)!

Tipp: a GET/PUT/POST hívásokhoz mind használható egyazon modell objektum!

## Következő feladat

Folytasd a [következő feladattal](Feladat-4.md).
