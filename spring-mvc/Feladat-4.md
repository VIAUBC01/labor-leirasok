# 4. Feladat

Egy PUT típusú kérést kezelő végpontot kell fejleszteni az ingatlanok módosítására, de olyan módon, hogy az ingatlan korábbi adatai is visszakereshetők legyenek, vagyis:

- A kérés törzsében JSON formátumban érkeznek az ingatlan új adatai, az URI-ben az ingatlan azonosítója
- Az adott azonosítóhoz tartozó oszlopértékeket át kell másolni egy másik, historikus táblába, amely az ingatlant tároló tábla oszlopait ismétli meg, valamint három új oszlopot ad hozzá: 
  - az ingatlan eredeti id-jét (egy ingatlanhoz több ilyen historikus sor keletkezhet több módosítás esetén, ezért az új táblában generált id-k nem tudják megőrizni azt az infót, hogy melyik ingatlanhoz tartozik a historikus bejegyzés)
  - a historikus sor érvényességi dátumának kezdetét, amelynek értéke két helyről származhat
    - ha az új historikus sor beszúrásakor van az adott ingatlanhoz korábbi historikus sor, akkor azok közül a legutóbbinak az érvényesség vége időbélyegét kell ide másolni
    - ha az új historikus sor beszúrásakor még nincs korábbi historikus sor az adott ingatlanhoz, akkor az eredeti ingatlan létrehozásának időbélyegét kell ide másolni 
  - a historikus sor érvényességének végét jelző időbélyeget (ez a most éppen aktuális időbélyeg lesz)
- A historikus sor beszúrása után az új adatokkal kell felülírni a meglévő sort az eredeti ingatlan táblában

Készíts egy végpontot, amelyen egy adott ingatlan id-hez lekérdezhető az adattörténete (az összes sor a historikus táblából + az aktuális érték).

Egy példa:

- T1 időpontban létrehoztak X1 irányárral egy ingatlant, melynek id-je 1.
- T2 időpontban módosították az irányárát X2-re
- T3 időpontban módosították az irányárát X3-ra

Ilyenkor, a többi, nem változó oszlop értékét elhanyagolva az 1-es id-hez tartozó ingatlan lekérdezett adattörténetében ezek fognak szerepelni (érvényesség kezdete, érvényesség vége, irányár):

- T1, T2, X1
- T2, T3, X2
- T3 , null, X3

## Végeztél

Végeztél a feladatokkal.
