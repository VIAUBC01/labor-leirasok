# Adatréteg készítése Entity Frameworkkel

A labor során egy adatréteget fogunk megvalósítani, amely a következő 2 labor ([Web API](../webapi/README.md), [Razor](../razor/README.md)) alapjául szolgál majd.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök (az [Entity Framework](../ef/README.md), [Web API](../webapi/README.md) és [Razor](../razor/README.md) laborok igényei megegyeznek):

- Windows
  - A feladatok megoldhatóak Linux és macOS operációs rendszereken is, ekkor értelemszerűen az OS-nek megfelelő eszközöket (Visual Studio Code, JetBrains Rider stb.) kell használni. 
  Ezeket a lehetőségeket viszont nem tárgyaljuk.
- Microsoft Visual Studio 2022 (a _Community_ verzió is megfelelő)
  - Az _Operating system not supported_ hibaüzenetre _OK_-ot lehet nyomni
  - Minimálisan szükséges workloadok:
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
- [Hivatalos jegyzetek a _Háttéralkalmazások_ tárgy honlapján (belépés után jelennek meg)](https://www.aut.bme.hu/Course/VIAUBB04)
- [Entity Framework Core (_Háttéralkalmazások_ gyakorlat)](https://github.com/BMEVIAUBB04/gyakorlat-ef)
- [ASP.NET Core Web API/Razor (_Háttéralkalmazások_ gyakorlat)](https://github.com/BMEVIAUBB04/gyakorlat-rest-web-api)

Hivatalos dokumentációk, amelyek jó kiindulásként szolgálnak a részletes megértéshez vagy elakadás esetén elő kell venni:
- EF Core:
  - [EF Core tools reference (.NET CLI) - EF Core | Microsoft Docs](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet)
  - [Migrations Overview - EF Core | Microsoft Docs](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
  - [Querying Data - EF Core | Microsoft Docs](https://docs.microsoft.com/en-us/ef/core/querying/)
  - [Saving Data - EF Core | Microsoft Docs](https://docs.microsoft.com/en-us/ef/core/saving/)
- ASP.NET:
  - [Introduction to ASP.NET Core | Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
  - [Introduction to Razor Pages in ASP.NET Core | Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/)
  - [Create web APIs with ASP.NET Core | Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/web-api/)

## Beadandó

A labor elvégzése után az alábbi tartalmat kérjük beadni a Moodle-re történő feltöltéssel:
- **PDF** formátumban (`.docx` nem elfogadott!) az egyes feladatoknál megnevezett: 
  - konkrét kódrészletek bemásolva, esetleg az azokról készített képernyőkép(ek)
  - 1 mondatos magyarázat
  - 1 vagy több ábra (jellemzően képernyőkép), ami a helyes működést hivatott bizonyítani. 

## Értékelés

A laborban négy feladatrész van. Jeles osztályzat az összes feladatrész elvégzésével kapható. 
Minden hiányzó, avagy hiányos feladatrész mínusz egy jegy.

A feladatok megoldásának akár részleges közzétételéért vagy másolásáért vagy a gyanú felmerülése esetén az aktuális szabályzatok értelmében fegyelmi eljárást indítunk, amelynek eredményeképp a hallgató eltiltásban részesülhet! 
A feladatok megoldása minden esetben teljesen **önálló**.

## Feladatok

Összesen 4 feladat van. [Itt kezdd](Feladat-1.md) az elsővel.
