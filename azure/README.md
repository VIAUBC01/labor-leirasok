# Azure webhoszting

A labor során .NET Core alapú, adatbázist használó webalkalmazásokat kell Azure-ba telepíteni [Azure SQL](https://azure.microsoft.com/en-us/products/azure-sql/), illetve [Azure App Service](https://azure.microsoft.com/en-us/products/app-service/) szolgáltatásokra építve.

## Előkészületek

A mérés Windows és Linux rendszeren is teljesíthető. A méréshez böngészőn kívül nem szükséges semmilyen egyéb eszköz.

### Azure előfizetés beüzemelése

A mérés hallgatói Azure előfizetéssel teljesítendő, [itt aktiváld](https://azure.microsoft.com/en-us/free/students/).

Bővebb leírás a [Felhő alapú szoftverfejlesztés tárgy honlapjáról](https://www.aut.bme.hu/Course/felho)
> [Azure for Students](https://azure.microsoft.com/en-us/free/students/) - ez az újabb program, 100$ kredittel egy évre (de évente megújítható). Bankkártya regisztráció nem szükséges. Ezt próbáljtátok meg aktiválni! Edu.bme.hu-s fiókot érdemes használni az aktivációhoz. Fontos, hogy 10-11 hónap után indul egy megújítási időablak és ha abban nem újítjátok meg, akkor ki kell várni míg egy több hónapos inaktív időszak után teljesen törődik az előfizetés és utána tudtok (ugyanazzal az email címmel) egy teljesen új előfizetést igényelni.

## Feladatok

### Általános szabályok

:warning: A következő típusú Azure erőforrások nevének **kötelezően** tartalmaznia kell a mérést végző neptun kódját: 

  - Azure SQL Server
  - Azure App Service

Ha névütközés miatt nem lehet simán a neptun kód, akkor kerüljön elé és/vagy mögé pár extra karakter.


### Feladat 1

Végzed el a következő hivatalos Microsoft oktatóanyagot: [magyarul (gépi fordítású)](https://learn.microsoft.com/hu-hu/azure/app-service/tutorial-dotnetcore-sqldb-app) vagy [angolul](https://learn.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app). Az angol az ajánlott, mivel az az eredeti, a magyar gépi fordítás sem rossz, követhető, de néhány helyen rosszul ragoz vagy furán fogalmaz. Mielőtt nekiállnál, mindenképp olvasd el az eltérések részt, a tippeket és a beadandók leírását is!

A feladat hasonló a [Háttéralkalmazások tárgy Azure-os gyakorlatához](https://github.com/BMEVIAUBB04/gyakorlat-azure), de jóval komolyabb, komplexebb annál. 

A jelentősebb eltérések:
- Minden létrejövő erőforrás alapból csak egymást éri el (privát elérés), így mi sem tudunk az internet felől csatlakozni ezekhez vagy belépni ezekbe (kivéve Azure portál meghatározott funkcióin keresztül). Ez egy biztonsági ökölszabály (_secure-by-default_), így ne kapcsold be az internet felőli elérés(eke)t. A privát elérés alapja egy virtuális hálózat. Ebbe a hálózatba kerülnek be az Azure erőforrásaink. Platformszolgáltatásaink lesznek, ezek esetén az ajánlott integrációs módszer a [privát hálózati végpontok](https://www.fugue.co/blog/cloud-network-security-101-azure-private-link-private-endpoints) alkalmazása. Kifelé a virtuális hálózat zárt, a publikus DNS névfeloldás sem működik, ezért jön létre privát DNS, így továbbra is hálózati névvel tudunk hivatkozni az egyes szolgáltatásokra, pl. adatbázis szerver.
- Az alkalmazás [Azure Redis Cache](https://azure.microsoft.com/en-us/products/cache)-t használ gyorsítótárként
- Az Azure Web App felől az Azure SQL adatbázis, valamint a Redis cache felé [Service Connector](https://learn.microsoft.com/en-us/azure/service-connector/overview) reprezentálja a kapcsolatot.
- Az összes előbb említett erőforrást egy füst alatt a **Web app + Database** varázslóval/sablonnal hozzuk létre.
- Az adatbázis séma inicializáláshoz egy futtatható állományt ([EF Core migration bundle](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#bundles)) hozunk létre, amit az alkalmazás egyéb fájljaival együtt publikálunk és így azokkal együtt fel is kerül a futtatókörnyezetbe. A futtatható fájlt külön kell futtatnunk a Web App futtatókörnyezetébe belépve. Ez nem egy szép megoldás, de mivel az adatbázist csak a többi erőforrásból érjük el, nincs túl sok választásunk.

#### Tippek és hasznos tudnivalók

:bulb: Az Azure portálra történő első belépéskor érdemes [beállítani a portál nyelvét](https://learn.microsoft.com/en-us/azure/azure-portal/set-preferences#language--region). Ajánlott, hogy a nyelv egyezzen meg az oktatóanyag nyelvével, míg a formátum mindig legyen magyar. Jelen leírásban a portál angol nyelvű menüpontjaira hivatkozunk.

:bulb: ha az Azure-ban futó webalkalmazással gond van, érdemes előrevenni a [diagnosztikai naplók lekérdezéséről szóló részt](https://learn.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app#6-stream-diagnostic-logs). A diagnosztikai naplókat már az App Service létrehozása után be lehet kapcsolni.

:bulb: Régióként ajánlott a nyugat- vagy észak-európait használni (West Europe, North Europe). Az elején válaszd ki az egyiket és azt használd végig.

:bulb: Azure erőforrások létrehozásakor az űrlap utolsó oldalának alján ne felejtsük el a `Create` gombot megnyomni, különben nem indul el a létrehozási folyamat!

:bulb: Éles környezetben általában a connection string-ben olyan felhasználót adunk meg, akinek nincs is joga a migrációs műveletek elvégzésére. Ilyenkor viszont külön a migrációt egy erőteljesebb jogú felhasználót tartalmazó connection string-gel kellene [futtatni](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#efbundle).

:warning: Alapesetben publikus git repoban dolgozol. Ne pusholj bele semmilyen szenzitív adatot, jelszót!


### Eltérések

- Az első lépés előtt említett klónozást nem kell elvégezni, nincs rá szükség.
- Az 1.1 lépés előtt értelmes elvégezni a **Microsoft.Sql** resource provider regisztrációját. Minden Azure műveletet valamelyik ARM resource provider hajtja végre. A legtöbb szükséges resource provider eleve be van kapcsolva vagy a varázsló be tudja kapcsolni, amikor szükséges. Az Azure SQL (**Microsoft.Sql** azonosítójú) provider alapból általában nincs bekapcsolva (regisztrálva) és az első lépés varázslója hibát adhat (_SQLAzure is not available for your selection of subscription and location_). [Segédlet](https://learn.microsoft.com/en-us/azure/azure-resource-manager/management/resource-providers-and-types#register-resource-provider-1) egy resource provider regisztrálásához.
- Az 1.2 lépésben az app nevében (Name) az XYZ rész a neptun kódod legyen. A logikai SQL Server neve (Server name), az adatbázis neve (Database name), a cache neve (Cache name) szintén az _123_ rész helyett a neptun kódod legyen.
- Az 1.2.7-es allépésben lehet, hogy nem az SQLAzure van kiválasztva, ilyenkor válasszuk ki mi.
- Az 1.3-as lépésnél a létrehozás több percig eltarthat, addig a 3.1-es lépéssel lehet haladni.
- A 3.1-es lépésnél a fork létrehozásánál hagyjuk meg az alapértelmezett beállításokat.
- A 3.7-es lépésben a YAML fájlt nagyon nagy körültekintéssel szerkesszük. Egyetlen hiányzó vagy extra szóköz is hibás YAML fájlt eredményezhet!
- A 6.1.2-es allépést követően is mentsünk (Save) a felső sávban lévő gombbal.
- A 6.2-es lépésben a napló nézetben az üzenetek több (2,3,5!) perces késéssel jelennek meg, különösen a bekapcsolást követően. A lefuttatott SQL parancsoknak meg kellene jelenni (ha nem gyorsítótár szolgálja ki a kérést) idővel.
- A 7. lépést (erőforrások törlése) csak akkor hajtsd végre, ha a másik feladatot is megoldottad és mindent begyűjtöttél a beadandókhoz.

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

| Parancssor esetén (terminálparancs)|Azure portál esetén (böngésző) | Fájlnév (kiterjesztés nélkül) |
| -----------------|--------------------| -----------------------------------------|
| Azure CLI belépés | Azure portál főoldal, belépés után | `f1_azlogin` |
| App Service létrehozás | Az új App Service áttekintő oldala (Overview)     | `f1_app` |
| Adatbázis létrehozása | Az új adatbázis áttekintő oldala (Overview)      | `f1_sqldb` |
| Service Connector létrehozása (`az webapp connection create`) | Az App Service *Service Connector* oldala | `f1_svcconn` |
| Adatbázis inicializálása (`dotnet ef database update`) | Adatbázis inicializálása (`dotnet ef database update`) (ez kivételesen terminálparancs kimenet) | `f1_efmigr` |
| :cloud: Diagnosztikai naplók lekérdezése egy elem módosítása után | :cloud: Az App Service *Log stream* oldala egy elem módosítása után | `f1_log` |

A lekérdezett naplóüzenetek tartalmával kapcsolatban nincs elvárás, nem kell pl. a létrehozás kérésnek látszania.

#### Képernyőképek a böngészőben futó webalkalmazás **főoldaláról**

| Képernyőkép böngészőről | Fájlnév (kiterjesztés nélkül) |
| -----------------|--------------------------------------------------------------|
| első indítás után     | `f1_v1` |
| :cloud: új elem/teendő felvétele után. Az új elem/teendő leírása a neptun kódod legyen    | `f1_v2` |

:cloud:: az így jelölt képek nem szükségesek az elégséges szint eléréséhez. Összesen 5+1 kép kell az elégégeshez.
    
### Beadandó - Feladat 2

#### Képernyőképek az alábbi lépésekről

| Parancssor esetén (terminálparancs)|Azure portál esetén (böngésző) | Fájlnév (kiterjesztés nélkül) |
| -----------------|--------------------| -----------------------------------------|
| App Service létrehozás | Az új App Service áttekintő oldala (Overview)     | `f2_app` |
| Adatbázis létrehozása | Az új adatbázis áttekintő oldala (Overview)      | `f2_sqldb` |
| Service Connector létrehozása (`az webapp connection create`) | Az App Service *Service Connector* oldala | `f2_svcconn` |
| :cloud: Diagnosztikai naplók lekérdezése a főoldal betöltődése után | :cloud: Az App Service *Log stream* oldala a főoldal betöltődése után | `f2_log` |

A lekérdezett naplóüzenetek tartalmával kapcsolatban itt sincs elvárás.

#### Képernyőképek a böngészőben futó MovieCatalog webalkalmazás **főoldaláról**

| Képernyőkép böngészőről | Fájlnév (kiterjesztés nélkül) |
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

