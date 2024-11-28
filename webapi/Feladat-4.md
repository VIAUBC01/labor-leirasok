# Feladat 4.

A korábbiakkal koncepcionálisan analóg módon készítsd el a művek (`Title`) alábbi műveleteit is:

- `DELETE api/titles/<ID>`
    - Ne felejtsd el az ID ellenőrzést!
- `GET api/titles?pageSize=<oldalméret>`
    - Adja vissza a legfrissebb filmeket (maximális *StartYear*) `StartYear` szerint rendezve
    - *oldalméret* darab eredményt adjon vissza, de legfeljebb százat
    - ha nincs megadva *oldalméret*, ötvenet adjon vissza
    - az oldalmérettel kapcsolatos fenti logikát a kontrollerbe tedd, a `TitleService.GetTitlesAsync` már a szabályoknak
      megfelelő paramétert kapjon

## Beadandó tesztkérések

- Két törlés egymás után ugyanarra az ID-ra
- Két listázás különböző oldalmérettel, ami különböző szabályozás alá esik

# Végeztél

:godmode: Végeztél a feladatokkal. :godmode:

# Bónusz feladat

Plusz egy jegyért: tedd lehetővé, hogy a művek módosításánál és beszúrásánál a műfajazonosítók tömbje is beküldhető
legyen a DTO-ban és érvényre is jusson, azaz az új/módosult mű kizárólag a megadott műfajokba tartozzon. Ha módosításnál
egyáltalán nem kapunk műfajtömböt (nem üres tömb), akkor ne változzon a műfaji besorolás.

## Beadandó tesztkérések

- Több műfajjal rendelkező mű módosítása, csak az egyik műfaj megváltoztatása
- Több műfajjal rendelkező mű módosítása, műfajtömb küldése nélkül
- Több műfajjal rendelkező új mű beszúrása.
