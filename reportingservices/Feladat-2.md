# Feladat 2: Vizualizáció

A feladat **önálló munka**.

A táblázatos megjelenítés részletesen mutatja az eladási adatokat. Egy diagram azonban gyorsabban áttekinthető.
Készítsünk egy diagramot, ami az egyes termékkategóriák eladásait mutatja.

## Diagram beszúrása

1. Váltsunk _Design_ nézetre, és húzzunk be egy *Chart*ot a *Toolbox*ról a táblázat mellé. Ennek hatására elég sokáig
   fog tölteni a diagramvarázsló, de egy idő után megnyílik. Válasszuk ki az első oszlopdiagramtípust.

1. A _Report Data_ panelről húzzuk a _LineTotal_ mezőt a diagramra. **Még ne engedjük fel a bal egérgombot!** Meg fog
   jelenni a diagram mellett a _Chart Data_ ablak – itt a "∑ Values" mező (a fehér téglalap) fölé vigyük az egeret. Most
   már elengedhetjük.

   ![Chart hozzáadása](images/rs-chart-data.png)

   Ezzel azt állítottuk be, hogy az eladás értékeiének összegét szeretnénk függőleges tengelynek.

1. Húzzuk a _Chart data_ alatt a _Category Groups_ mezőbe a _Subcat_ mezőt, a _Series Groups_-ba pedig a _Date_-et.

   ![Diagram értékei](images/rs-chart-values.png)

   Ezzel azt érjük el, hogy a vízszintes tengelyen az alkategória szerint külön oszlopcsoportokat kapunk, dátum szerint
   pedig külön oszlopsorozatokat.

1. A `[Date]` feliraton jobb klikkeljünk, és válasszuk a _Series Groups Properties…_-t. Itt nyomjuk meg a _Group
   expressions_ csoportban az **_fx_** gombot.

   ![Expression megadása](images/rs-chart-group-expression.png)

   A megjelenő ablakba írjuk be: `=Year(Fields!Date.Value)`

   ![Expression értéke](images/rs-chart-group-expression2.png)

   Ezzel a dátum éve szerinti oszlopokat fogunk kapni.

1. Nyomjunk OK-t mindkét ablakban. Mielőtt megnéznénk a _Preview_-t, növeljük meg a diagram magasságát bőséggel,
   különben a jelmagyarázat nem fog kiférni:

   ![Diagram átnéretezése](images/rs-chart-resize.png)

1. Ha most megnézzük a *Preview*-t, az egyes kategóriák termelte bevételt fogjuk látni, év szerint csoportosítva:

   ![Diagram előnézete](images/rs-chart-preview-1.png)

## A diagram formázása

A megjelenés még nem az igazi, de ezen könnyen segíthetünk.

1. _Chart Title_-re kattintva írjuk át a diagram címét, pl. "Revenue by category".

1. A _Series Groups_ mezőben az `<<Expr>>` feliratra jobb klikkelve válasszuk ki a _Series Groups Properties…_-t, és itt
   a _Label_ mező mellett nyomjuk meg az **_fx_** gombot. Értéknek adjuk meg: `=Year(Fields!Date.Value)`. Ezzel a
   felirat maga is csak az évet fogja mutatni.

1. Jobb klikkeljünk a függőleges tengely címkéire, és válasszuk a _Vertical Axis Properties…_ lehetőséget.

   ![Tengely formázása](images/rs-y-axis-properties.png)

   Itt a _Number_ fülön válasszuk a _Currency_ lehetőséget, és töltsük ki a már ismert módon:

   ![Tengely formázása](images/rs-y-axis-properties-currency.png)

1. Ha most megnézzük a _Preview_ fület, már szép lesz a diagramunk:

   ![Előnézet](images/rs-chart-preview-2.png)

## Következő feladat

Folytasd a [következő feladattal](Feladat-3.md).
