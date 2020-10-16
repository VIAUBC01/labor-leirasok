# MovieCatalog projekt telepítése Azure-ba

Az első feladatban megismert módszerekkel telepítsd az ASP.NET Core Razor Pages mérésen készített MovieCatalog projektet Azure-ba. A projekt git repository-ja [itt található](https://github.com/VIAUBC01/MovieCatalog.Azure)

A projektet nem kell lokálisan tesztelni, kipróbálni, illetve módosítani. A projekt eleve SQL Server-es adatelérésre van felkészítve így az Entity Framework alrendszert nem kell SQLite-ról átállítani, illetve a migrációt sem kell előzetesen futtatni (`dotnet ef database update`), mivel automatikus migráció be van kapcsolva.

:warning: Éles környezetben általában a connection string-ben olyan felhasználót adunk meg, akinek nincs is joga a migrációs műveletek elvégzésére. Ilyenkor viszont külön kell a migrációt lefuttatni egy erőteljesebb jogú felhasználót tartalmazó connection string-gel.

Ehhez a feladathoz már nincs részletes leírás, csak a lépéseket adjuk meg:

- SQL Server létrehozása. A neve tartalmazza a neptun kódot!
- Tűzfalszabályok beállítása 
- **SQL adatbázis létrehozása**
- **Connection string megszerzése**
- git telepítési felhasználó beállítása
- Azure App Service Plan létrehozása
- **Azure Web App létrehozása** A neve tartalmazza a neptun kódot!
- **Connection string beállítása** :warning: Figyelj rá, hogy az alkalmazás `MovieCatalog` nevű connection string-et vár! (a connection string neve a `--settings` kapcsoló után áll az `az webapp config connection-string set` parancsban)
- **a projekt git repository-jának klónozása**
- **új, azure-os git remote hozzáadása**
- **push az új git remote-ra**

A nem **kiemeltek** csak akkor kellenek, ha nem az első feladat előfizetésén dolgozol. A kiemelteket mindenképp végre kell hajtani.