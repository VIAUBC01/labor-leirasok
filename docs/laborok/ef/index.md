# Áttekintés

A labor során egy adatréteget fogunk megvalósítani, amely a következő 2 labor ([Web API](../webapi), [Razor](../razor)) alapjául szolgál majd.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök (az [Entity Framework](../ef), [Web API](../webapi) és [Razor](../razor) laborok igényei megegyeznek):

- Windows
- Microsoft Visual Studio 2022 (Community verzió is megfelelő)
    - minimálisan szükséges workload-ok: 
        - ASP.NET and web development
        - .NET desktop development
        - Data storage and processing

- Opcionálisak:
    - [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads): Express/Developer változatok ingyenesen használhatók
        - az útmutatók alapján a Visual Studio mellett feltelepülő _LocalDB_ változatot használjuk
            - ettől el lehet térni, ekkor megfelelően, értelemszerűen módosítandó a kapcsolódáshoz használt _connection string_
    - [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
        - Alternatívák:
            - Visual Studio beépített *SQL Server Object Explorer* komponense és kapcsolódó felületei 
            - Azure Data Studio (SSMS újabb verziói feltelepítik)

A laborok elvégzéséhez használható segédanyagok és felkészülési anyagok:

- [Hivatalos jegyzetek a Háttéralkalmazások tárgy honlapján (belépés után jelennek meg)](https://www.aut.bme.hu/Course/VIAUBB04)
- [Entity Framework Core (Háttéralkalmazások gyakorlat)](https://github.com/BMEVIAUBB04/gyakorlat-ef)
- [ASP.NET Core Web API/Razor (Háttéralkalmazások gyakorlat)](https://github.com/BMEVIAUBB04/gyakorlat-rest-web-api)

Hivatalos dokumentációk, amelyek jó kiindulásként szolgálnak a részletes megértéshez vagy elakadás esetén elő kell venni:

- EF Core:
    * [EF Core tools reference (.NET CLI) - EF Core | Microsoft Docs](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet)
    * [Migrations Overview - EF Core | Microsoft Docs](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
    * [Querying Data - EF Core | Microsoft Docs](https://docs.microsoft.com/en-us/ef/core/querying/)
    * [Saving Data - EF Core | Microsoft Docs](https://docs.microsoft.com/en-us/ef/core/saving/)
- ASP.NET:
    * [Introduction to ASP.NET Core | Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
    * [Introduction to Razor Pages in ASP.NET Core | Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/)
    * [Create web APIs with ASP.NET Core | Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/web-api/)

### Más OS-en:

- A feladatok megoldhatók Linux és Mac OS operációs rendszereken is, ekkor értelemszerűen az OS-nek megfelelő eszközöket (Visual Studio for Mac, Visual Studio Code, JetBrains Rider stb.) kell használni. Ezeket a lehetőségeket részletesen nem tárgyaljuk.

## Labor menete

### Git repository létrehozása és letöltése

1. Moodle-ben keresd meg a laborhoz tartozó meghívó URL-jét és annak segítségével hozd létre a saját repository-dat.

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

    !!! warning "Checkout"
        Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd [itt](../../tudnivalok/github/GitHub-credentials.md)), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz. 

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Beadandó

A labor elvégzése után az alábbi tartalmat kérjük beadni a laborvezető által meghatározott módon.

- Minden feladathoz 1 (jellemzően képernyőkép), ami a helyes működést hivatott bizonyítani. 
- A Visual Studio projekt könyvtára

### Értékelés

A laborban négy feladatrész van. Jeles osztályzat az összes feladatrész elvégzésével kapható. Minden hiányzó, avagy hiányos feladatrész mínusz egy jegy.

A feladatok megoldásának akár részleges közzétételéért vagy másolásáért vagy a gyanú felmerülése esetén az aktuális szabályzatok értelmében fegyelmi eljárást indítunk, amelynek eredményeképp a hallgató eltiltásban részesülhet! A feladatok megoldása minden esetben teljesen **önálló**.

## Feladatok

Összesen 4 feladat van. [Itt kezdd](Feladat-1.md) az első feladattal!
