# Feladat 2: Számla ellenőrzése tárolt eljárással

## A) feladatrész - Tárolt eljárás

Írj tárolt eljárást `SzamlaEllenoriz` néven, aminek bemeneti paramétere egy `int` típusú és `@szamlaid` nevű számlaazonosító.

- Az eljárás ellenőrizze, hogy a paraméterben kapott számlához kapcsolódó számlatételeken szereplő mennyiség egyezik-e a kapcsolódó megrendelés tétel mennyiségével. (A számlatétel hivatkozik a megrendeléstételre.)
- Amennyiben eltérés található a kettőben, úgy mindkettő értékét, valamint a termék nevét írd ki a standard outputra az alábbi séma szerint: `Elteres: Labda (szamlan 5 megrendelesen 6)`
- Csak akkor írjon bármit a kimenetre a tárolt eljárás, ha problémát talált. Semmiképpen se hagyj teszteléshez használt kiírást az eljárásban!
- Legyen az eljárás `int` típusú visszatérési értéke 0, ha nem kellett semmit kiírni a kimenetre, és 1, ha kellett.

A kiíráshoz használd a `print` parancsot: `PRINT 'Szoveg' + @valtozo + 'Szoveg'` Ügyelj rá, hogy a változónak char típusúnak kell lennie, egyéb típus, pl. szám konvertálása: `convert(varchar(5), @valtozo)`, pl. `PRINT 'Szoveg' + convert(varchar(5), @valtozo)`

## B) feladatrész - Minden számla ellenőrzése

Írj T-SQL kódblokkot, ami az előző feladatban megírt eljárást hívja meg egyenként az összes számlára. Érdemes ehhez egy kurzort használnod, ami a számlákon fut végig.

A kód minden számla ellenőrzése előtt írja ki a standard outputra a számla azonosítóját (pl. `Szamla: 12`), és amennyiben nem volt eltérés, írja ki a kimenetre, hogy 'Helyes számla'.

Segítség: tárolt eljárás meghívása kódból az `exec` paranccsal lehetséges, pl.

```sql
declare @eredmeny int
exec @eredmeny = SzamlaEllenoriz 123
```

Ellenőrizd a kódblokk viselkedését. Ahhoz, hogy eltérést tapasztalj, ha szükséges, változtass meg egy megrendelés tételt vagy számla tételt. Az ellenőrzéshez tartozó kódot nem kell beadni.

## Következő feladat

Folytasd a [következő feladattal](Feladat-3.md).
