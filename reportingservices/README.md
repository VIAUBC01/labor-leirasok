# Microsoft SQL Server Reporting Services használata

A labor során egy új eszközzel, a *Microsoft SQL Server Reporting Services*zel ismerkedünk meg.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Windows
- Microsoft SQL Server: az Express változat ingyenesen használható, de a Visual Studio mellett feltelepülő _localdb_
  változat is megfelelő
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Az adatbázist létrehozó script: [adventure-works-2014-oltp-script.zip](adventure-works-2014-oltp-script.zip)
- Microsoft Visual Studio 2019 (a [Community](https://archive.org/download/vs_Community/vs_Community.exe) verzió is
  megfelelő)
- Report server projekt-támogatás Visual
  Studióhoz: [Microsoft Reporting Services Projects extension](https://marketplace.visualstudio.com/items?itemName=ProBITools.MicrosoftReportProjectsforVisualStudio)

Felkészülési segédlet:

- SQL Server Reporting
  Services [hivatalos tutorial](https://docs.microsoft.com/en-us/sql/reporting-services/create-a-basic-table-report-ssrs-tutorial)

### Adventure Works 2014 adatbázis létrehozása

A feladatok során az _Adventure Works_ mintaadatbázissal dolgozunk. Az adatbázis egy kereskedelmi cég értékesítéseit
tartalmazza, amelyből mi a teljes adatbázis megértése helyett csak előre definiált lekérdezésekkel dolgozunk, melyek
termékeladások adatait tartalmazzák.

1. Töltsd le és csomagold ki az [adventure-works-2014-oltp-script.zip](adventure-works-2014-oltp-script.zip) fájlt a
   `C:\work\Adventure Works 2014 OLTP Script` könyvtárba.

   Mindenképpen ez a mappa legyen, különben az SQL-fájlban az alábbi helyen ki kell javítani a könyvtár elérési
   útvonalát:

   ```sql
   -- NOTE: Change this path if you copied the script source to another path
   :setvar SqlSamplesSourceDataPath "C:\work\Adventure Works 2014 OLTP Script\"
   ```

   Ha beleszerkesztesz az elérési útvonlaba, ügyelj arra, hogy a **végén maradjon perjel**!

1. Kapcsolódj a Microsoft SQL Serverhez az SQL Server Management Studio segítségével. Az alábbi adatokkal kapcsolódj:

    - Server name: `(localdb)\mssqllocaldb`
    - Authentication: `Windows Authentication`

1. A _File / Open / File…_ menüpont használatával nyisd meg az előbbi mappából az `instawdb.sql` fájlt. **Még ne
   futtasd!** Előbb kapcsold be az SQLCMD módot: a _Query_ menüben _SQLCMD Mode_. Csak ezt követően válasszuk az
   _Execute_ lehetőséget.

   ![SQLCMD mód](images/sql-management-sqlcmd-mode.png)

1. Ellenőrizd, hogy létrejött-e az adatbázis és a táblák. Ha a baloldali fában a _Databases_en _Refresh_-t nyomsz, meg
   kell jelenjen az _AdventureWorks2014_ adatbázis a listában, és alatta számtalan tábla.

   ![AdventureWorks adatbázistáblák](images/rs-adventureworks-tablak.png).

## Beadandó

A labor elvégzése után az alábbi tartalmat kérjük beadni a laborvezető által meghatározott módon:

- A Visual Studio-projekt könyvtára
- Minden feladat végeredményéről egy képernyőkép a riport _Preview_ ablakában a releváns részekről

## Értékelés

A laborban négy feladatrész van (az A és B feladatrészek kettőnek számítanak). Jeles osztályzat az összes feladatrész
elvégzésével kapható. Minden hiányzó, avagy hiányos feladatrész mínusz egy jegy.

## Feladatok

Összesen 3 feladat van. [Itt kezdd](Feladat-1.md) az első feladattal.
