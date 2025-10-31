# Azure

A labor során .NET Core alapú, adatbázist használó webalkalmazásokat kell Azure-ba telepíteni [Azure SQL](https://azure.microsoft.com/en-us/products/azure-sql/), illetve [Azure App Service](https://azure.microsoft.com/en-us/products/app-service/) szolgáltatásokra építve.

## Előkészületek

A mérés Windows és Linux rendszeren is teljesíthető. A méréshez böngészőn kívül nem szükséges semmilyen egyéb eszköz.

### Git repository létrehozása és letöltése

1. Moodle-ben keresd meg a laborhoz tartozó meghívó URL-jét és annak segítségével hozd létre a saját repository-dat.

1. Várd meg, míg elkészül a repository, majd checkout-old ki.

    !!! warning "Checkout"
        Egyetemi laborokban, ha a checkout során nem kér a rendszer felhasználónevet és jelszót, és nem sikerül a checkout, akkor valószínűleg a gépen korábban megjegyzett felhasználónévvel próbálkozott a rendszer. Először töröld ki a mentett belépési adatokat (lásd [itt](../../tudnivalok/github/GitHub-credentials.md)), és próbáld újra.

1. Hozz létre egy új ágat `megoldas` néven, és ezen az ágon dolgozz. 

1. A `neptun.txt` fájlba írd bele a Neptun kódodat. A fájlban semmi más ne szerepeljen, csak egyetlen sorban a Neptun kód 6 karaktere.

### Azure előfizetés beüzemelése

A mérés hallgatói Azure előfizetéssel teljesítendő, [itt aktiváld](https://azure.microsoft.com/en-us/free/students/).

Bővebb leírás a [Felhő alapú szoftverfejlesztés tárgy honlapjáról](https://www.aut.bme.hu/Course/felho)
> [Azure for Students](https://azure.microsoft.com/en-us/free/students/) - ez az újabb program, 100$ kredittel egy évre (de évente megújítható). Bankkártya regisztráció nem szükséges. Ezt próbáljtátok meg aktiválni! Edu.bme.hu-s fiókot érdemes használni az aktivációhoz. Fontos, hogy 10-11 hónap után indul egy megújítási időablak és ha abban nem újítjátok meg, akkor ki kell várni míg egy több hónapos inaktív időszak után teljesen törődik az előfizetés és utána tudtok (ugyanazzal az email címmel) egy teljesen új előfizetést igényelni.

## Feladatok

### Általános szabályok

:warning: A következő típusú Azure erőforrások nevének **kötelezően** tartalmaznia kell a mérést végző neptun kódját: 

  - Azure SQL Server
  - Azure App Service

### Feladat 1

Végzed el a következő hivatalos Microsoft oktatóanyagot: [magyarul (gépi fordítású)](https://learn.microsoft.com/hu-hu/azure/app-service/tutorial-dotnetcore-sqldb-app) vagy [angolul](https://learn.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app). Az angol az ajánlott, mivel az az eredeti, a magyar gépi fordítás sem rossz, követhető, de néhány helyen rosszul ragoz vagy furán fogalmaz. Mielőtt nekiállnál, mindenképp olvasd el az eltérések részt, a tippeket és a beadandók leírását is!

A feladat hasonló a [Háttéralkalmazások tárgy Azure-os gyakorlatához](https://github.com/BMEVIAUBB04/gyakorlat-azure), de jóval komolyabb, komplexebb annál. 

A jelentősebb eltérések:

- Minden létrejövő erőforrás alapból csak egymást éri el (privát elérés), így mi sem tudunk az internet felől csatlakozni ezekhez vagy belépni ezekbe (kivéve Azure portál meghatározott funkcióin keresztül). Ez egy biztonsági ökölszabály (_secure-by-default_), így ne kapcsold be az internet felőli elérés(eke)t. A privát elérés alapja egy virtuális hálózat. Ebbe a hálózatba kerülnek be az Azure erőforrásaink. Platformszolgáltatásaink lesznek, ezek esetén az ajánlott integrációs módszer a [privát hálózati végpontok](https://www.fugue.co/blog/cloud-network-security-101-azure-private-link-private-endpoints) alkalmazása. Kifelé a virtuális hálózat zárt, a publikus DNS névfeloldás sem működik, ezért jön létre privát DNS, így továbbra is hálózati névvel tudunk hivatkozni az egyes szolgáltatásokra, pl. adatbázis szerver.
- Az alkalmazás [Azure Redis Cache](https://azure.microsoft.com/en-us/products/cache)-t használ gyorsítótárként
- Az Azure Web App felől az Azure SQL adatbázis, valamint a Redis cache felé [Service Connector](https://learn.microsoft.com/en-us/azure/service-connector/overview) reprezentálja a kapcsolatot.
- Az adatbázis séma inicializáláshoz egy futtatható állományt ([EF Core migration bundle](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#bundles)) hozunk létre, amit az alkalmazás egyéb fájljaival együtt publikálunk és így azokkal együtt fel is kerül a futtatókörnyezetbe. A futtatható fájlt külön kell futtatnunk a Web App futtatókörnyezetébe belépve. Ez nem egy szép megoldás, de mivel az adatbázist csak a többi erőforrásból érjük el, nincs túl sok választásunk.

#### Tippek és hasznos tudnivalók

:bulb: Az Azure portálra történő első belépéskor érdemes [beállítani a portál nyelvét](https://learn.microsoft.com/en-us/azure/azure-portal/set-preferences#language--region). Ajánlott, hogy a nyelv egyezzen meg az oktatóanyag nyelvével, míg a formátum mindig legyen magyar. Jelen leírásban a portál angol nyelvű menüpontjaira hivatkozunk.

:bulb: ha az Azure-ban futó webalkalmazással gond van, érdemes előrevenni a [diagnosztikai naplók lekérdezéséről szóló részt](https://learn.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app#6-stream-diagnostic-logs). A diagnosztikai naplókat már az App Service létrehozása után be lehet kapcsolni.

:bulb: Régióként ajánlott a nyugat- vagy észak-európait használni (West Europe, North Europe). Az elején válaszd ki az egyiket és azt használd végig.

:bulb: Azure erőforrások létrehozásakor az űrlap utolsó oldalának alján ne felejtsük el a `Create` gombot megnyomni, különben nem indul el a létrehozási folyamat!

:bulb: Érdemes folyamatosan ellenőrizni, hogy van-e olyan bedandó képernyőkép, amit épp el tudsz készíteni.

:bulb: Az SSH terminált érdemes egy külön böngészőfülön megnyitni. Ezen a fülön ne navigálj el a terminál oldaláról.

:bulb: Éles környezetben általában a connection string-ben olyan felhasználót adunk meg, akinek nincs is joga a migrációs műveletek elvégzésére. Ilyenkor viszont külön a migrációt egy erőteljesebb jogú felhasználót tartalmazó connection string-gel kellene [futtatni](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#efbundle).

:warning: Alapesetben publikus git repoban dolgozol. Ne pusholj bele semmilyen szenzitív adatot, jelszót!

:warning: Ha kitörölsz egy (elrontott) Azure erőforrást, annak neve nem mindig szabadul fel azonnal, ilyenkor nem hozhatsz létre azonnal ugyanolyan névvel másik erőforrást.

### Eltérések

- Kezdheted a 2-es lépéssel és utána csinálhatod az 1-est nyugodtan, mivel egymástól függetlenek. A 2-es lépés végén több (tíz)perces várakozásra is szükség lehet, így az alatt lesz időd megcsinálni az 1-est.
- Az 2.1 lépés előtt értelmes elvégezni a **Microsoft.Sql** resource provider regisztrációját. Minden Azure műveletet valamelyik ARM resource provider hajtja végre. A legtöbb szükséges resource provider eleve be van kapcsolva vagy a varázsló be tudja kapcsolni, amikor szükséges. Az Azure SQL (**Microsoft.Sql** azonosítójú) provider alapból általában nincs bekapcsolva (regisztrálva) és az első lépés varázslója hibát adhat (_SQLAzure is not available for your selection of subscription and location_). [Segédlet](https://learn.microsoft.com/en-us/azure/azure-resource-manager/management/resource-providers-and-types#register-resource-provider-1) egy resource provider regisztrálásához.
- Az 2.2 lépésben az app nevében (Name) az XYZ rész a neptun kódod legyen. A logikai SQL Server neve (Server name), az adatbázis neve (Database name), a cache neve (Cache name) szintén az _123_ rész helyett a neptun kódod legyen.
- A 2.2.5-es allépésben lehet, hogy nem az SQLAzure van kiválasztva, ilyenkor válasszuk ki mi.
- :warning: A 2.4.7-es lépésnél (Review + create) sajnos jelenleg hibára fut a deploy egy hibás JSON miatt.. A hiba oka, hogy egy `tags` értéket rak a JSON-be a template, de ilyen változója nem lehet. Workaround, amíg nem javítják: 
    - **Review + create** lépésnél kattints a *Download a template for automation* linkre
    - Itt nyomd meg a *Deploy* gombot
    - Az új nézetben az *Edit Template* gombot, amiután szerkeszthetővé válik a json
    - Keresd meg a subenetet (egyedi neve van, de ehhez hasonló: *"vnet-mtpsrpsr/subnet-vbbsuxu"*), keress arran, hogy *'subnet'*
    - töröld ki az a sort utána, ami így néz ki: `"tags": {},` (vesszőstül, mindenestül)
    - Nyomd meg a *Save* gombot, ami után visszanavigál a deployment nézetre.
    - Itt állítsd vissza a resource groupot arra, amit létrehoztál
    - Adj megfelelő jelszót az *Sql Server Admin Pwd* mezőbe (figyelj, hogy jó legyen, nem validálja itt, de elszáll a deploy, ha nem megfelelő, példa egy jóra: *m8$Tl34zmhsdhOdu* jó)
    - *Next* gomnyomásra kattintás után validálja a beállíátsokat
    - Ha minden rendben, *Create* gombot nyomd meg (elnavigálás után megkérdezi, hogy elmented-e a json változtatásokat, ezt nem kell)
- 4-es pont: co-pilot nélkül teszteltük, saját felelősségre kipróbálhatod vele is
- A 4.5-ös lépésben a YAML fájlt nagyon nagy körültekintéssel szerkesszük. Egyetlen hiányzó vagy extra szóköz is hibás YAML fájlt eredményezhet!
- Az 5-ös lépésnél ha *A connection was successfully established with the server, but then an error occurred during the login process* hibát kapunk, akkor a key vaulthoz nem fér hozzá, javítsd ki a hálózati beállításait.
- A 7.1.2-es allépést követően is mentsünk (Save) a felső sávban lévő gombbal.
- A 7.2-es lépésben a napló nézetben az üzenetek több (2,3,5!) perces késéssel jelennek meg, különösen a bekapcsolást követően. A lefuttatott SQL parancsoknak meg kellene jelenni (ha nem gyorsítótár szolgálja ki a kérést) idővel.
- A 8. lépést (erőforrások törlése) majd csak akkor hajtsd végre, ha a lentebbi feladatot is megoldottad és mindent begyűjtöttél a beadandókhoz.

### Feladat 2

Az első feladatban megismert módszerekkel telepítsd az ASP.NET Core Razor Pages mérésen készített *MovieCatalog.Web* projektedet Azure-ba. A feladathoz tartozó erőforrások egy új erőforráscsoportba (resource group) kerüljenek.

Ehhez a feladathoz már nincs részletes leírás, csak néhány extra tippet adunk:

:bulb: Ne kövesd szolgaian az első feladat elnevezéseit, mert névütközések lesznek. Találj ki saját neveket, de ahol kell, ott legyen benne a neptun kódod

:bulb: A Runtime stack-et a MovieCatalog projekt .NET verziójához igazítsd (megtalálod a projekt fájlban)

:bulb: Redis Cache-t nem kell létrehozni

:bulb: EF Migration bundle-t nem kell csinálni, az ehhez szükséges EF eszközt sem kell feltenni a GitHub Actions YAML-ben.

:bulb: Az adatbázis feltöltéséhez használd a [mellékelt Azure SQL-lel kompatibilis DACPAC](./data/imdbtitles_sample_azure.dacpac) csomagot. Ehhez:   

1. töltsd fel a csomagot a projekted repojába, például a projekt fájl mellé.
1. módosítsd a GitHub Actions YAML fájlt (.github alkönyvtárban), hogy a kimeneti könyvtárba másolja a csomagot. A másolásra egy módja az alábbi lépés beszúrása a feltöltési lépés elé. Ellenőrizd, hogy az elérési út megfelelő-e: a repo gyökeréhez képest kell az elérési útvonalat megadni.
    ```yaml
     - name: Copy dacpac
       run: |
        cp MovieCatalog.Web/    imdbtitles_sample_azure.dacpac ${{env.  DOTNET_ROOT}}/myapp
    ```
1. az App Service-be SSH-zva ellenőrizd, hogy a git push után induló telepítési folyamat feltöltötte-e a `/home/site/wwwroot` mappába a DACPAC csomagot.
1. DACPAC csomagot parancssorból az [sqlpackage](https://learn.microsoft.com/en-us/sql/tools/sqlpackage/sqlpackage-download?view=sql-server-ver16) eszközzel telepíthetünk. Ehhez azonban le kell tölteni (`wget`) az eszközt csomagolva, kicsomagolni (`unzip`), futtathatóvá tenni (`chmod`) majd futtatni (`sqlpackage`). A [parancsnak](https://learn.microsoft.com/en-us/sql/tools/sqlpackage/sqlpackage-publish?view=sql-server-ver16) szüksége van a
connection string-re, ami környezeti változóként rendelkezésre áll. Mindezen műveletekre egy példát ad a [mellékelt parancsfájl](./data/sqlpackage-appservice.sh). A legtöbb esetben elég csak a fájl tartalmát lefuttatni az SSH terminálon.

### Végeztél

Végeztél a feladatokkal.

:warning: A beadás után érdemes törölni minden Azure erőforrást.

## Beadandó

### Általános elvek

Beadandó a git repoba rakott képernyőkegyépek *png* formátumban.

Képernyőképekkel kapcsolatos elvárások:

- a böngészőablak teljes méretű (maximalizált) legyen 
- látszódjon a címsor és a weboldal megjelenő része
- Azure portálnál látszódjon a jobb felső sarokban az Azure portálra belépett felhasználó a jobb felső sarokban

SSH terminálparancsok kimenetéről készült képernyőképeknél: 

- ha a parancs kimenete olyan hosszú, hogy nem férne rá egy képernyőre, akkor görgessetek föl, hogy a parancs a képernyő tetején legyen és így csináljátok a képernyőképet. (Ha így sem fér rá, nem baj, ilyenkor már nem kell a teljes kimenetnek látszani)
- bár elsődleges a kimenet, látszódjon a futtatott parancs és alatta a kimenet is

### Beadandó - Feladat 1

#### Képernyőképek az alábbi lépésekről

| Kép tartalma                                                                            | Típus        | Fájlnév (kiterjesztés nélkül) |
|-----------------------------------------------------------------------------------------|--------------|-------------------------------|
| Azure portál főoldal, belépés után                                                      | Böngésző     | `f1_azlogin`                  |
| Az új App Service áttekintő oldala (Overview)                                           | Böngésző     | `f1_app`                      |
| Az új adatbázis áttekintő oldala (Overview)                                             | Böngésző     | `f1_sqldb`                    |
| EF Migration bundle futtatása                                                           | SSH terminál | `f1_efmigr`                   |
| Az App Service-en belül a Service Connector aloldalon minden kapcsolat le van validálva | Böngésző     | `f1_svcconn`                  |
| :cloud: Az App Service *Log stream* oldala egy elem módosítása után                     | Böngésző     | `f1_log`                      |

:bulb: Service Connector-ok validálása az App Service [Service Connector lapján](https://learn.microsoft.com/en-us/azure/service-connector/quickstart-portal-container-apps): jelölj ki minden kapcsolatot és felül nyomd meg a _Validate_ gombot. Várd meg, amíg az ellenőrzés lefut.

A lekérdezett naplóüzenetek tartalmával kapcsolatban nincs elvárás, nem kell pl. a létrehozás kérésnek látszania.

#### Képernyőképek a böngészőben futó webalkalmazás **főoldaláról**

| Kép tartalma | Fájlnév (kiterjesztés nélkül) |
| -----------------|--------------------------------------------------------------|
| első indítás után     | `f1_v1` |
| :cloud: új elem/teendő felvétele után. Az új elem/teendő leírása a neptun kódod legyen    | `f1_v2` |

:cloud:: az így jelölt képek nem szükségesek az elégséges szint eléréséhez. Összesen 5+1 kép kell az elégégeshez.
    
### Beadandó - Feladat 2

#### Képernyőképek az alábbi lépésekről

| Kép tartalma                                                                                    | Típus        | Fájlnév (kiterjesztés nélkül) |
|-------------------------------------------------------------------------------------------------|--------------|-------------------------------|
| Az új App Service áttekintő oldala (Overview)                                                   | Böngésző     | `f2_app`                      |
| Az új adatbázis áttekintő oldala (Overview)                                                     | Böngésző     | `f2_sqldb`                    |
| sqlpackage futtatása                                                                            | SSH terminál | `f2_dbmigr`                   |
| :cloud: Az App Service-en belül a Service Connector aloldalon minden kapcsolat le van validálva | Böngésző     | `f2_svcconn`                  |                 |

#### Képernyőképek a böngészőben futó MovieCatalog webalkalmazás **főoldaláról**

| Kép tartalma | Fájlnév (kiterjesztés nélkül) |
| -----------------|--------------------------------------------------------------|
| első indítás után     | `f2_v1` |

:cloud:: az így jelölt képek nem szükségesek a jeles szint eléréséhez.

## Értékelési irányelvek

- ha **nem** csak jelölt (:cloud:) képek hiányoznak az első feladatból :arrow_right: 1
- ha csak jelölt (:cloud:) kép hiányzik az első feladatból:arrow_right: 2
- ha minden elkészül az első feladatból :arrow_right: 3
- ha csak jelölt (:cloud:) kép hiányzik a második feladatból:arrow_right: 4
- ha minden elkészül az első és a második feladatból is :arrow_right: 5

