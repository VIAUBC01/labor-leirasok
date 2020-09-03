# Microsoft SQL Server Reporting Services használata

A labor során egy új eszközzel, a Microsoft SQL Server Reporting Services-zel ismerkedünk meg, így a labor részben vezetett. Az első feladat laborvezetővel együtt megoldott, a továbbiak önálló feladatok.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Windows
- Microsoft SQL Server: Express változat ingyenesen használható, avagy Visual Studio mellett feltelepülő _localdb_ változat is megfelelő
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Az adatbázist létrehozó script: [adventure-works-2014-oltp-script.zip](adventure-works-2014-oltp-script.zip)
- Microsoft Visual Studio 2019 (Community verzió is megfelelő)
- Report Server Projekt támogatás Visual Studio-hoz: [Microsoft Reporting Services Projects extension](https://marketplace.visualstudio.com/items?itemName=ProBITools.MicrosoftReportProjectsforVisualStudio)

Felkészülési segédlet:

- SQL Reporting Services [hivatalos tutorial](https://docs.microsoft.com/en-us/sql/reporting-services/create-a-basic-table-report-ssrs-tutorial)

### Adventure Works 2014 adatbázis létrehozása

A feladatok során az _Adventure Works_ minta adatbázissal dolgozunk. Az adatbázis egy kereskedelmi cég értékesítéseit tartalmazza, amelyből mi a teljes adatbázis megértése helyett előre definiált lekérdezésekkel dolgozunk csak, melyek termékek eladásainak adatait tartalmazza.

1. Töltsd le és csomagold ki az [adventure-works-2014-oltp-script.zip](adventure-works-2014-oltp-script.zip) fájlt a `c:\work\Adventure Works 2014 OLTP Script` könyvtárba.

   Mindenképpen ez a mappa legyen, különben az sql fájlban az alábbi helyen ki kell javítani a könyvtár elérési útvonalát:

   ```sql
   -- NOTE: Change this path if you copied the script source to another path
   :setvar SqlSamplesSourceDataPath "C:\work\Adventure Works 2014 OLTP Script\"
   ```

   Ha beleszerkesztesz az elérési útvonlaba, ügyelj hogy a **végén maradjon perjel**!

1. Kapcsolódj Microsoft SQL Serverhez SQL Server Management Studio segítségével. Az alábbi adatokkal kapcsolódj.

   - Server name: `(localdb)\mssqllocaldb`
   - Authentication: `Windows authentication`

1. A _File / Open / File..._ menüpont használatával nyisd meg az előbbi mappából az `instawdb.sql` fájlt. **Még ne futtasd!** Előbb kapcsold be az SQLCMD módot: a _Query_ menüben _SQLCMD Mode_, és csak ezt követően válasszuk az _Execute_ lehetőséget.

   ![SQLCMD mód](images/sql-management-sqlcmd-mode.png)

1. Ellenőrizd, hogy létrejött-e az adatbázis és a táblák. Ha a baloldali fában a _Databases_-en _Refresh_-t nyomsz, meg kell jelenjen az _AdventureWorks2014_ adatbázis a listában, és alatta számtalan tábla.

   ![AdventureWorks adatbázis táblák](images/rs-adventureworks-tablak.png).

## Beadandó

A labor elvégzése után az alábbi tartalmat kérjük beadni a laborvezető által meghatározott módon:

- A Visual Studio projekt könyvtára,
- Minden feladat végeredményéről egy képernyőkép a riport _preview_ ablakáról a releváns részekről.

## Feladatok

Összesen 3 feladat van. [Itt kezdd](Feladat-1.md) az első feladattal.
