# 2. feladat

Nyomon akarjuk követni az egyes ingatlanok iránt mutatott érdeklődést, ennek érdekében:

- Definiálj egy entitást, ami egy ingatlan iránt mutatott érdeklődést reprezentál. Ebben tárolni kell:
  - egy automatikusan generált azonosítót
  - a megfelelő ingatlanra mutató kapcsolatot
  - az érdeklődés dátumát (dátum + idő milliszekundum pontossággal, a szerver időzónájában értelmezve)
  - az érdeklődést mutató felhasználó valamilyen azonosítóját
  - az érdeklődés típusát: csak az ingatlan adatok megtekintése történt meg, vagy a telefonszámot is lekérték-e
- Bővítsd ki az előző feladat GET végponjait:
  - Ha az ingatlan alapadatait egy felhasználó először lekéri, az a megfelelő ingatlanhoz szúrjon be egy Érdeklődés entitást is. Tegyük fel, hogy a lekérő felhasználó azonosítója az `Authorization` headerben érkezik, egyszerű sztringként. Ha a lekérő felhasználónak már van egy ingatlanadat-lekérő típusú érdeklődése arra az ingatlanra, akkor nem szabad új érdeklődés sort beszúrni.
  - Ha az ingatlanhoz tartozó telefonszámot kérik le, akkor az előzőekhez hasonlóan egy új Érdeklődés bejegyzés keletkezzen, de telefonszám típusú, kivéve, ha az adott felhasználónak már volt arra az ingatlanra egy ilyen típusú érdeklődése.

## Következő feladat

Folytasd a [következő feladattal](Feladat-3.md).
