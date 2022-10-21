# Azure webhoszting

A labor során .NET Core alapú, adatbázist használó webalkalmazásokat kell Azure-ba telepíteni Azure SQL, illetve Azure App Service szolgáltatásokra építve. A műveleteket parancssorban vagy webes felületen is végezhetjük.

## Előkészületek

A mérés Windows és Linux rendszeren is teljesíthető. Telepítés előtt ajánlott ellenőrizni a lenti ellenőrző parancsokkal, hogy mi van már eleve feltelepítve.

Telepítendő parancssoros eszközök (ha még nincsenek telepítve):    
 - [.NET Core 6.0 SDK](https://docs.microsoft.com/hu-hu/dotnet/core/install/) (Visual Studio telepítő is feltelepíti)


 Egyéb kellékek:
 - [Windows Terminal](https://www.microsoft.com/hu-hu/p/windows-terminal/9n0dx20hk701?rtc=1&activetab=pivot:overviewtab) (opcionális, csak Windows-on telepíthető)
 - valamilyen szövegszerkesztő, pl. jegyzettömb, [Visual Studio Code](https://code.visualstudio.com/)
 - *Visual Studio 2022* (opcionális): az Azure-ba való telepítés lépése ezzel is végezhető parancssor vagy Visual Studio Code helyett.

 A legjobb, ha terminálként Windows Terminal-t tudunk használni, de jó a [Visual Studio Code terminálja](https://code.visualstudio.com/docs/terminal/basics) is.


#### Telepítés ellenőrzése

.NET Core SDK ellenőrzése, legalább v6.0 legyen
```bash
dotnet --version
```

### Ha parancssorban dolgoznál

Telepítendő további parancssoros eszközök (ha még nincsenek telepítve):    
 - Azure CLI - [Windows](https://aka.ms/installazurecliwindows) [Linux](https://docs.microsoft.com/hu-hu/cli/azure/install-azure-cli) 
 - [git](https://git-scm.com/downloads) (Visual Studio telepítő is feltelepíti)


#### Telepítés ellenőrzése

Azure CLI ellenőrzése, ajánlott verzió legalább v2.13
```bash
az --version
```
git ellenőrzése, ajánlott verzió legalább v2.28 (Windows), v2.17 (Linux)
```bash
git --version
```

### Ha böngészőben dolgoznál

Elsődlegesen böngésző kell csak. Pár parancsot itt is parancssorban kell futtatni. 

### Azure előfizetés beüzemelése

A mérés hallgatói Azure előfizetéssel teljesítendő, [itt aktiváld](https://azure.microsoft.com/en-us/free/students/).

Bővebb leírás a [Felhő alapú szoftverfejlesztés tárgy honlapjáról](https://www.aut.bme.hu/Course/felho)
> [Azure for Students](https://azure.microsoft.com/en-us/free/students/) - ez az újabb program, 100$ kredittel egy évre (de évente megújítható). Bankkártya regisztráció nem szükséges. Ezt próbáljtátok meg aktiválni! Edu.bme.hu-s fiókot érdemes használni az aktivációhoz. Fontos, hogy 10-11 hónap után indul egy megújítási időablak és ha abban nem újítjátok meg, akkor ki kell várni míg egy több hónapos inaktív időszak után teljesen törődik az előfizetés és utána tudtok (ugyanazzal az email címmel) egy teljesen új előfizetést igényelni.

## Feladatok

### Általános szabályok

:warning: A következő típusú Azure erőforrások nevének **kötelezően** tartalmaznia kell a mérést végző neptun kódját: 

  - Azure SQL Server
  - Azure App Service

Ha névütközés miatt nem lehet simán a neptun kód, akkor kerüljön elé és/vagy mögé pár extra karakter. ABC123 neptun kód esetén az Azure SQL Server neve lehet például `abc123srv`.


### Feladat 1

Végzed el a következő hivatalos Microsoft oktatóanyagot: [magyarul (gépi fordítású)](https://docs.microsoft.com/hu-hu/azure/app-service/tutorial-dotnetcore-sqldb-app) vagy [angolul](https://docs.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app). Az angol az ajánlott, mivel az az eredeti, a magyar gépi fordítás sem rossz, követhető, de néhány helyen rosszul ragoz vagy furán fogalmaz. Mielőtt nekiállnál, mindenképp olvasd el az eltérések részt, a tippeket és a beadandók leírását is!

Az útmutató külön füleken megmutatja, hogy az egyes lépéseket hogyan lehet végrehajtani többfajta eszközzel (Azure CLI, Azure portal, Visual Studio, stb.). Szabadon választhattok, hogy milyen eszközzel dolgoztok, melyik fület választjátok, de az elején érdemes eldönteni és végig ahhoz tartani magatokat.

#### Tippek és hasznos tudnivalók mindenkinek

:bulb: ha az Azure-ban futó webalkalmazással gond van, érdemes előrevenni a [diagnosztikai naplók lekérdezéséről szóló részt](https://learn.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?tabs=azure-portal%2Cvisualstudio-deploy%2Cdeploy-instructions-azure-portal%2Cazure-portal-logs%2Cazure-portal-resources#8---configure-and-stream-application-logs). A diagnosztikai naplókat már az App Service létrehozása után be lehet kapcsolni.

:warning: ha Visual Studio Code-ot használunk szövegszerkesztőként, akkor minden kódfájl módosítás után explicit mentsük el a fájlt (CTRL+S), különben a `git`, `dotnet ef` parancsok nem fogják érzékelni a változásokat.

:bulb: Régióként ajánlott a nyugat- vagy észak-európait használni (West Europe, North Europe). Az elején válaszd ki az egyiket és azt használd végig.

:warning: Éles környezetben általában a connection string-ben olyan felhasználót adunk meg, akinek nincs is joga a migrációs műveletek elvégzésére. Ilyenkor viszont külön kellene a migrációt lefuttatni egy erőteljesebb jogú felhasználót tartalmazó connection string-gel.

#### Tippek és hasznos tudnivalók parancssort használóknak

:bulb: érdemes legalább két konzolablakot használni, mindkettőben ugyanabban a könyvtárban állni, de az egyikben csak az Azure CLI (`az` kezdetű) parancsokat futtatni, a másikban minden mást

:bulb: a munkakönyvtár (ahol a parancssorunk áll) legyen egyszerű, pl. c:\work\neptunkód (Windows) vagy ~/src (Linux)

:bulb: érdemes egy jegyzettömböt is nyitni és a különböző többször használatos értékeket ment közben feljegyezni (connection string, erőforráscsoport neve, stb.)

#### Tippek és hasznos tudnivalók Azure portált használóknak

:bulb: Az Azure portálra történő első belépéskor érdemes [beállítani a portál nyelvét](https://learn.microsoft.com/en-us/azure/azure-portal/set-preferences#language--region). Ajánlott, hogy a nyelv egyezzen meg az oktatóanyag nyelvével, míg a formátum mindig legyen magyar. Jelen leírásban a portál angol nyelvű menüpontjaira hivatkozunk.

:bulb: Azure erőforrások létrehozásakor az űrlap utolsó oldalának alján ne felejtsük el a `Create` gombot megnyomni, különben nem indul el a létrehozási folyamat!

:bulb: Nem kell mindig megvárni, míg egy létrehozási folyamat elkészül, például az SQL Server létrehozásához nem kell megvárni amíg az App Service elkészül. Nyilván a Service Connector létrehozásához viszont már kell, hogy kész legyen az adatbázisunk.

### Eltérések

- Az App Service létrehozásakor az App neve a neptun kódod (vagy a neptun kódodból képzett név) legyen
- Az SQL Server létrehozásakor az szerver neve a neptun kódod (vagy a neptun kódodból képzett név) legyen
- Az SQL Database létrehozásakor ún. *Serverless* adatbázist hozzunk létre 
    - ha Azure portálon dolgozol, ehhez elég ha a *Development* leehetőséget választod a *Workload environment* beállításnál.
    - ha parancssorban dolgozol, [plusz paramétereket kell átadni](https://learn.microsoft.com/en-us/azure/azure-sql/database/serverless-tier-overview?view=azuresql#create-a-new-database-in-the-serverless-compute-tier) az `az sql database create`-nek
- Ha Azure portálon dolgozol, akkor az SQL Server tűzfal és hálózati beállítás menüpontja megváltozott! *Networking* a menüpont új neve és a *Public access* fülön az *Add your client IPv4 address (x.y.w.z)* opcióval tudod a saját géped címét hozzáadni a tűzfalszabályokhoz. Ne felejtsd el alul a *Save* gombot megnyomni!
- Az appsettings.json fájlban ne írd át magát a connection string-et, csak a connection string **nevét**. Ne kerüljön a konfigurációs fájlba jelszó! Tehát ehelyett
    ```
    "MyDbConnection": "Server=(localdb)\\mssqllocaldb;Trusted_Connection=True;MultipleActiveResultSets=true"
    ```
    az alábbi legyen:
    ```
    "AZURE_SQL_CONNECTIONSTRING": "Server=(localdb)\\mssqllocaldb;Trusted_Connection=True;MultipleActiveResultSets=true"
    ```
- A `dotnet ef database update` parancsnak add meg az Azure adatbázisod connection stringjét:

    ```
    dotnet ef database update --connection "<Azure SQL DB connection string>"
    ```
- Ha Azure portálon dolgozol, akkor a naplózó (*Application Logging*) funkciónak nem kell megadni megtartási időszakot (*retention period*), helyette a naplózási szintet (*Level*) állítsuk *Information*-re.
- A feladat végén **ne töröld az Azure erőforrásokat**! Majd csak akkor, ha a másik feladatot is megoldottad és mindent begyűjtöttél a beadandókhoz.

### Feladat 2

Az első feladatban megismert módszerekkel telepítsd az ASP.NET Core Razor Pages mérésen készített *MovieCatalog.Web* projektedet Azure-ba. Használd ki, hogy egy SQL Server alatt több adatbázis és egy App Service Plan alatt több App Service App / Web App lehet, tehát nem kell új SQL Servert vagy App Service Plan-t létrehoznod.

Ehhez a feladathoz már nincs részletes leírás, csak a főbb lépéseket adjuk meg:

- új SQL adatbázis létrehozása az előző feladat szervere alá
- adatbázis feltöltése. Használd az [Azure SQL-lel kompatibilis DACPAC](./data/imdbtitles_sample_azure.dacpac) csomagot. Ebben ugyanazok az adatok vannak, mint a Razor mérésen. Majdnem ugyanúgy tudod használni, mint a korábbi DACPAC-ot, csak itt most egy 
    - [Azure adatbázishoz kell csatlakoznod](https://stackoverflow.com/a/66015950) a Visual Studio-s SQL Server Object Explorer-ből
    - és csatlakozás után nem a *Databases* feliratra kell jobbklikkelned, hanem a *Databases*-t kibontva a konkrét adatbázisra (ott kell lennie a már ismerős *Publish Data-tier Application* menüpontnak)
- új App Service létrehozása. A neve most is tartalmazza a neptun kódodat. App Service Plan-nek add meg a már meglévő Plan-t.
- új *Service Connector* létrehozása az új App Service-hez
- a projekteden belül az *appsettings.json*-ben és a *Program.cs*-ben írd át a connection string *nevét* (és csak a nevét!) *DBneptunkód*-ról *AZURE_SQL_CONNECTIONSTRING*-re
    - adatbázis migrációt, `dotnet ef` parancsokat most nem kell futtatni, mert az adatbázis már fel van töltve
- végül mehet a telepítés!

### Végeztél

:godmode: Végeztél a feladatokkal. :godmode:

## Beadandó

### Általános elvek

Beadandó egy összecsomagolt állomány, melyben képernyőképek vannak *jpg* vagy *png* formátumban. 

Terminálparancsok kimenetéről készült képernyőképeknél: 

- a terminál ablaka teljes méretű (maximalizált) legyen
- ha a parancs kimenete olyan hosszú, hogy nem férne rá egy képernyőre, akkor görgessetek föl, hogy a parancs a képernyő tetején legyen és így csináljátok a képernyőképet. (Ha így sem fér rá, nem baj, ilyenkor már nem kell a teljes kimenetnek látszani)
- bár elsődleges a kimenet, látszódjon a futtatott parancs és alatta a kimenet is

Böngészőről készült képernyőképeknél:

- a böngészőablak teljes méretű (maximalizált) legyen 
- látszódjon a címsor és a weboldal megjelenő része
- Azure portálnál látszódjon a jobb felső sarokban az Azure portálra belépett felhasználó a jobb felső sarokban

A maximális feltöltési méret 15 MB. Ha a túl nagy képek miatt a feltöltendő tömörített fájl ennél nagyobb lenne, át lehet méretezni (nem levágni!) a képeket, de a szövegeknek olvashatónak kell maradni.

### Beadandó - Feladat 1

#### Képernyőképek az alábbi lépésekről

| Parancssor esetén (terminálparancs)|Azure portál esetén (böngésző) | Képernyőkép fájlneve (kiterjesztés nélkül) |
| -----------------|--------------------| -----------------------------------------|
| Azure CLI belépés | Azure portál főoldal, belépés után | `f1_azlogin` |
| App Service létrehozás | Az új App Service áttekintő oldala (Overview)     | `f1_app` |
| Adatbázis létrehozása | Az új adatbázis áttekintő oldala (Overview)      | `f1_sqldb` |
| Service Connector létrehozása (`az webapp connection create`) | Az App Service *Service Connector* oldala | `f1_svcconn` |
| Adatbázis inicializálása (`dotnet ef database update`) | Adatbázis inicializálása (`dotnet ef database update`) (ez kivételesen terminálparancs kimenet) | `f1_efmigr` |
| :cloud: Diagnosztikai naplók lekérdezése egy elem módosítása után | :cloud: Az App Service *Log stream* oldala egy elem módosítása után | `f1_log` |

A lekérdezett naplóüzenetek tartalmával kapcsolatban nincs elvárás, nem kell pl. a létrehozás kérésnek látszania.

#### Képernyőképek a böngészőben futó webalkalmazás **főoldaláról**

| Képernyőkép böngészőről | Képernyőkép fájlneve (kiterjesztés nélkül) |
| -----------------|--------------------------------------------------------------|
| első indítás után     | `f1_v1` |
| :cloud: új elem/teendő felvétele után. Az új elem/teendő leírása a neptun kódod legyen    | `f1_v2` |

:cloud:: az így jelölt képek nem szükségesek az elégséges szint eléréséhez. Összesen 5+1 kép kell az elégégeshez.
    
### Beadandó - Feladat 2

#### Képernyőképek az alábbi lépésekről

| Parancssor esetén (terminálparancs)|Azure portál esetén (böngésző) | Képernyőkép fájlneve (kiterjesztés nélkül) |
| -----------------|--------------------| -----------------------------------------|
| App Service létrehozás | Az új App Service áttekintő oldala (Overview)     | `f2_app` |
| Adatbázis létrehozása | Az új adatbázis áttekintő oldala (Overview)      | `f2_sqldb` |
| Service Connector létrehozása (`az webapp connection create`) | Az App Service *Service Connector* oldala | `f2_svcconn` |
| :cloud: Diagnosztikai naplók lekérdezése a főoldal betöltődése után | :cloud: Az App Service *Log stream* oldala a főoldal betöltődése után | `f2_log` |

A lekérdezett naplóüzenetek tartalmával kapcsolatban itt sincs elvárás.

#### Képernyőképek a böngészőben futó MovieCatalog webalkalmazás **főoldaláról**

| Képernyőkép böngészőről | Képernyőkép fájlneve (kiterjesztés nélkül) |
| -----------------|--------------------------------------------------------------|
| első indítás után     | `f2_v1` |

:cloud:: az így jelölt képek nem szükségesek a négyes szint eléréséhez.

:warning: A beadás után érdemes törölni minden Azure erőforrást.

## Értékelési irányelvek

- ha **nem** csak jelölt (:cloud:) képek hiányoznak az első feladatból :arrow_right: 1
- ha csak jelölt (:cloud:) kép hiányzik az első feladatból:arrow_right: 2
- ha minden elkészül az első feladatból :arrow_right: 3
- ha csak jelölt (:cloud:) kép hiányzik a második feladatból:arrow_right: 4
- ha minden elkészül az első és a második feladatból is :arrow_right: 5

