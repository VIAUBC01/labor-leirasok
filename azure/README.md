# Azure webhoszting

A labor során .NET Core alapú, adatbázist használó webalkalmazásokat kell Azure-ba telepíteni Azure SQL, illetve Azure App Service szolgáltatásokra építve.

## Előkészületek

A mérés Windows és Linux rendszeren is teljesíthető.

Telepítendő:
    
 - Azure CLI - [Windows](https://aka.ms/installazurecliwindows) [Linux](https://docs.microsoft.com/hu-hu/cli/azure/install-azure-cli) 
 - [Windows Terminal](https://www.microsoft.com/hu-hu/p/windows-terminal/9n0dx20hk701?rtc=1&activetab=pivot:overviewtab) (opcionális, csak Windows-on telepíthető)
 - git
 - valamilyen szövegszerkesztő, pl. jegyzettömb, [Visual Studio Code](https://code.visualstudio.com/)


## Feladatok

https://docs.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?pivots=platform-windows

:warning: a connection string beállítása csak a parancssori ablak bezárásig él

:warning: a környezeti változóban megadott connection string csak a lokális alkalmazásban működik

:warning: log streaminghez érdemes külön konzolablakot nyitni

https://docs.microsoft.com/en-us/learn/modules/develop-app-that-queries-azure-sql/
https://docs.microsoft.com/en-us/learn/modules/develop-app-that-queries-azure-sql/3-exercise-create-tables-bulk-import-query-data

## Beadandó

- egy szöveges fájl (txt), ami tartalmazza a következő parancsok kimenetét (általában JSON formátumú):
    - Azure SQL szerver létrehozása
    - Azure SQL adatbázis létrehozása
    - App Service Plan létrehozása
    - App Service létrehozása

## Értékelés

