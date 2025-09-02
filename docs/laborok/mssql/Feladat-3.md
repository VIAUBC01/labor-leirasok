# Feladat 3: Számla tábla denormalizálása

## Új oszlop

Módosítsd a `Szamla` táblát: vegyél fel egy új oszlopot `TetelSzam` néven a számlához tartozó összes tétel darabszámának tárolásához.

Készíts T-SQL programblokkot, amely kitölti az újonnan felvett oszlopot az aktuális adatok alapján. Ha például egy számlán megrendeltek 2 darab piros pöttyös labdát és 1 darab tollasütőt, akkor 3 tétel szerepel a számlán.

Figyelem, itt a **számlához** tartozó tételeket kell nézni (és nem a megrendelés tételeket)!

## Oszlop karbantartása triggerrel

Készíts triggert `SzamlaTetelszamKarbantart` néven, amely a számla tartalmának változásával együtt karbantartja az előzőleg felvett tételszám mezőt. Ügyelj rá, hogy hatékony legyen a trigger! A teljes újraszámolás nem elfogadható megoldás.

> Tipp: A triggert a `SzamlaTetel` táblára kell készíteni, bár a változott érték a `Szamla` táblában lesz.

Próbáld ki, jól működik-e a trigger. A teszteléshez használt utasításokat nem kell beadnod a megoldásban, de fontos, hogy ellenőrizd a viselkedést.

## Végeztél

Végeztél a feladatokkal.
