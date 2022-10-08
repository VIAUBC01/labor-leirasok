# Feladat 4.

A korábbiakkal koncepcionálisan analóg módon készítsd el a művek (`Title`) alábbi műveleteit is:
- `DELETE api/titles/<ID>`
  - Ne felejtsd el az ID ellenőrzést!
- `GET api/titles?pageSize=<oldalméret>`
  - Adja vissza a legfrissebb filmeket (maximális *StartYear*) `StartYear` szerint rendezve 
  - *oldalméret* darab eredményt adjon vissza, de legfeljebb ezret
  - ha nincs megadva *oldalméret*, százat adjon vissza
  - az oldalmérettel kapcsolatos fenti logikát a kontrollerbe tedd, a `TitleService.GetTitlesAsync` már a szabályoknak megfelelő paramétert kapjon

## Beadandó tesztkérések

- Két törlés egymás után ugyanarra az ID-ra
- Két listázás különböző oldalmérettel, ami különböző szabályozás alá esik

# Végeztél

:godmode: Végeztél a feladatokkal. :godmode:
