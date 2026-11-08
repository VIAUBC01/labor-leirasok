# Feladat 1: Razor alkalmazás

Ebben a feladatban az Azure portálon, kézzel hozod létre az erőforrásokat, és parancssorból telepíted a Razor alkalmazást. A feladat végére a `MovieCatalog.Web` egy App Service-ben fut, és egy Azure SQL adatbázisból dolgozik. A 2. feladatban a Web API ugyanerre az infrastruktúrára kerül.

!!! info "Fogalmak"
    - **Előfizetés** (*subscription*): a számlázási egység, nálunk az *Azure for Students*.
    - **Erőforráscsoport** (*resource group*): logikai mappa az összetartozó erőforrásoknak. Együtt kezelhetők, és együtt is törölhetők.
    - **Régió**: az adatközpont földrajzi helye. Minden erőforrás egy régióban jön létre.
    - **App Service csomag** (*App Service plan*): a számítási kapacitás, ezért fizetnénk. Nálunk a *Free F1* szint, ami ingyenes.
    - **Web App** (App Service alkalmazás): maga a futó alkalmazás. Egy csomagon több alkalmazás is futhat.
    - **Logikai SQL szerver** és **adatbázis**: a szerver a belépési pont (név, tűzfal, adminisztrátorok), az adatbázis a tényleges adattároló.

## 1. Erőforráscsoport létrehozása

1. Az [Azure portálon](https://portal.azure.com) a felső keresőbe írd be: *Resource groups*, majd *Create*.
1. *Subscription*: *Azure for Students*. *Resource group*: `rg-<neptun>`. *Region*: a felírt, engedélyezett régiód.
1. *Review + create*, majd *Create*.

!!! tip "Létrehozó varázslók"
    Minden létrehozó varázsló utolsó oldalán a *Create* gombot is meg kell nyomni, különben nem indul el a létrehozás. A folyamatot a jobb felső sarokban lévő harang ikonnál (*Notifications*) követheted.

## 2. Azure SQL adatbázis

1. A keresőbe írd be: *SQL databases*, majd *Create* » *SQL database (Free offer)*.
1. Ellenőrizd, hogy a létrehozó oldal tetején megjelenik a *Free offer applied* sáv.

    !!! warning "Ha nincs ingyenes ajánlat"
        Ha a menüben nincs *Free offer* lehetőség, vagy a létrehozó oldalon nem jelenik meg a sáv, ne hozd létre az adatbázist, mert így az fizetős lesz. Szólj a laborvezetőnek.

1. Töltsd ki a létrehozó oldalt:
    - *Resource group*: `rg-<neptun>`
    - *Database name*: `moviecatalog`
    - *Server*: *Create new*
        - *Server name*: `sql-<neptun>`
        - *Location*: a régiód
        - *Authentication method*: *Use SQL authentication*
            - *Server admin login*: pl. `sqladmin`, és adj meg egy erős jelszót (például ilyen formájút: `m8#Tl34zmhsdhOdu`). A jelszó legalább 8 karakteres legyen, és a kisbetű, nagybetű, szám és írásjel közül legalább hármat tartalmazzon. A `$`, `"` és `\` karaktereket kerüld, mert parancssorban és JSON-ban külön kezelést igényelnek.
    - Ha a varázsló mintaadatokat (*Sample*) is felkínál, ne kérd: üres adatbázis kell, a sémát és az adatokat a 3. lépésben töltjük be.
1. *Review + create*, majd *Create*. A létrehozás néhány percig tart.
1. Állítsd be a hálózati hozzáférést a szerveren (az ingyenes ajánlat varázslójában erre nincs külön fül). Nyisd meg az `sql-<neptun>` szervert, majd *Security* » *Networking*:
    - *Public network access*: *Selected networks*
    - *Firewall rules*: *Add your client IPv4 address*
    - *Exceptions*: pipáld be az *Allow Azure services and resources to access this server* lehetőséget
    - *Save*

!!! info "Miért kell a két tűzfalszabály?"
    Az Azure SQL szerver alapértelmezetten minden kapcsolatot elutasít. Az *Allow Azure services...* beállítás az Azure-on belülről érkező kapcsolatokat engedi, így az App Service-ben futó alkalmazásaid elérik majd az adatbázist. A *client IP* szabály a saját géped (illetve a labor hálózatának) publikus IP-címét engedi, hogy helyből is tudj csatlakozni. Éles rendszerben ennél szigorúbb megoldást használnánk (privát hálózat, privát végpontok), de ez már fizetős szolgáltatásszintet igényel.

!!! warning "Az ingyenes keret és az automatikus szüneteltetés"
    A szerver nélküli (*serverless*) adatbázis addig fogyasztja a vCore-másodperceket, amíg van rá nyitott kapcsolat. Kapcsolat nélkül kb. egy óra után magától szünetel, és a következő kapcsolódáskor újraindul. Ez az első kérésnél akár egy percig is tarthat, ilyenkor egy időtúllépés után frissíts rá. Ha nem dolgozol az adatbázissal, zárd be a hozzá kapcsolódó eszközöket (pl. SQL Server Object Explorer), különben hamar elfogyhat a havi keret.

## 3. Séma és adatok betöltése (DACPAC)

A korábbi laborokon egy DACPAC fájlból töltötted fel a LocalDB adatbázist. Most ugyanezt az Azure SQL adatbázissal teszed meg, az Azure SQL-lel kompatibilis változatot használva.

1. Töltsd le a [DACPAC fájlt innen](data/imdbtitles_sample_azure.dacpac).
1. Visual Studióban nyisd meg a *View* » *SQL Server Object Explorer* ablakot, és kattints az *Add SQL Server* ikonra.
1. Csatlakozz az Azure SQL szerverhez:
    - *Server Name*: `sql-<neptun>.database.windows.net`
    - *Authentication*: *SQL Server Authentication*, *User Name*: az admin felhasználó (pl. `sqladmin`), *Password*: a jelszavad
    - *Database Name*: `<default>` maradhat
    - *Connect*
1. Nyisd le a szervert és a *Databases* mappát. A meglévő `moviecatalog` adatbázison jobb klikk » *Publish Data-tier Application*.
1. Tallózd be a DACPAC fájlt, ellenőrizd, hogy a *Database name* mezőben `moviecatalog` áll, majd *Publish*. A folyamatot a *Data Tools Operations* ablakban követheted.

    !!! warning "A céladatbázis neve"
        A *Database name* mezőben pontosan a meglévő adatbázis neve (`moviecatalog`) álljon. Ha más nevet adsz meg, a Visual Studio egy új, akár fizetős adatbázist hoz létre. A betöltés után ellenőrizd a portálon, hogy továbbra is csak egy adatbázisod van.

    !!! tip "Időtúllépés az első kapcsolódáskor"
        Ha az első kapcsolódás időtúllépéssel elszáll, akkor az adatbázis úgynevezett hidegindítása van folyamatban. Várj egy percet, és próbáld újra.

1. A betöltés után az *SQL Server Object Explorer*-ben jobb klikk a szerveren » *Disconnect*. Amíg nyitott kapcsolat van, az adatbázis nem tud szünetelni, és fogyasztja az ingyenes keretet.
1. Ellenőrzés: a portálon az adatbázis oldalán nyisd meg a *Query editor (preview)* felületet, lépj be SQL authentikációval, és futtasd az alábbi lekérdezést:

    ```sql
    SELECT COUNT(*) FROM Titles;
    ```

??? tip "Visual Studio nélkül: sqlpackage parancssorból"
    ```powershell
    dotnet tool install --global microsoft.sqlpackage
    $conn = '<ADO.NET connection string>'
    sqlpackage /Action:Publish /SourceFile:imdbtitles_sample_azure.dacpac /TargetConnectionString:$conn
    ```

    - A connection stringet a portálon az adatbázis oldalán találod: *Settings* » *Connection strings* » *ADO.NET (SQL authentication)*. A `{your_password}` részt cseréld a jelszavadra.
    - A connection stringet aposztrófok (`'...'`) közé tedd. PowerShellben az idézőjelek (`"..."`) között a `$` jellel kezdődő részt változóként értelmezi a rendszer, így egy `$` jelet tartalmazó jelszóból csak a `$` előtti rész marad meg, és *Login failed* hibát kapsz.
    - Itt is ügyelj rá, hogy az `Initial Catalog` értéke a meglévő `moviecatalog` adatbázis legyen.


??? note "Ha a hálózat blokkolja a kapcsolatot (1433-as port)"
    Az Azure SQL a 1433-as TCP porton érhető el, amit egyes hálózatok tiltanak. Ellenőrzés PowerShellből:

    ```powershell
    Test-NetConnection sql-<neptun>.database.windows.net -Port 1433
    ```

    Ha a `TcpTestSucceeded` értéke `False`, akkor helyből nem tudsz csatlakozni. Ilyenkor hagyd ki most ezt és a következő (4.) lépést, kivéve a 4. lépés első pontját (a connection string lekérését), mert arra a 6. lépésben szükséged lesz. A 7. lépés (a Razor alkalmazás telepítése) után térj vissza ide, és töltsd be az adatokat az App Service-ből, amely az Azure-on belül eléri az adatbázist:

    1. A Web App oldalán *Development Tools* » *SSH* » *Go*.
    1. Az SSH konzolban futtasd az előkészített szkriptet. Ez letölti az `sqlpackage` eszközt és a DACPAC fájlt, majd az App Service-ben beállított connection stringgel betölti az adatokat:

        ```bash
        wget -q -O /tmp/load-dacpac.sh https://viaubc01.github.io/labor-leirasok/laborok/azure/data/sqlpackage-appservice.sh
        bash /tmp/load-dacpac.sh
        ```

## 4. Lokális Razor app és Azure SQL összekapcsolása

Mielőtt bármit telepítenél, kipróbálod, hogy a helyben futó Razor alkalmazás eléri-e az Azure SQL adatbázist. Ha később a telepített alkalmazás nem működik, így már tudni fogod, hogy az adatbázis-oldal rendben van.

1. Kérd le a connection stringet: a portálon az adatbázis oldalán *Settings* » *Connection strings* » *ADO.NET (SQL authentication)*. Másold ki, és a `{your_password}` részt cseréld a jelszavadra. (A 6. lépésben is ezt használod.)
1. Visual Studióban a `MovieCatalog.Web` projekten jobb klikk » *Manage User Secrets*. Megnyílik egy `secrets.json` fájl, ami ugyanúgy működik, mint az `appsettings.Development.json`, csak a gépeden, a repository-n kívül tárolódik.
1. Írd bele a connection stringet ugyanazzal a névvel, amit a Razor laboron használtál (`DB<neptun kód>`), majd mentsd el:

    ``` JSON
    {
        "ConnectionStrings": {
            "DB<neptun kód>": "<ADO.NET connection string>"
        }
    }
    ```

    Ha a jelszavad idézőjelet (`"`) vagy fordított perjelet (`\`) tartalmaz, azt JSON-ban `\"`, illetve `\\` alakban kell írni.

    ??? tip "Visual Studio nélkül: parancssorból"
        A projekt mappájában:

        ```powershell
        dotnet user-secrets init
        dotnet user-secrets set "ConnectionStrings:DB<neptun kód>" '<ADO.NET connection string>'
        ```

        A connection stringet aposztrófok közé tedd, ne idézőjelek közé (lásd a 3. lépésnél a `$` jelről szóló megjegyzést).

1. Indítsd el az alkalmazást helyben. A főoldal most már az Azure SQL adatbázisból kapja az adatokat (az első betöltés a hidegindítás miatt lassabb lehet, ha az adatbázis éppen szünetelt).

!!! info "Miért nem az `appsettings.Development.json`-t használjuk?"
    Az `appsettings.*.json` fájlok a repository részei, a jelszó tehát bekerülne a gitbe. A *user secrets* a felhasználói profilodban tárolódik (`%APPDATA%\Microsoft\UserSecrets`), a repository-n kívül. Fejlesztői (*Development*) környezetben az ASP.NET Core automatikusan betölti, és felülírja vele az `appsettings.Development.json` azonos nevű beállításait.

## 5. App Service létrehozása

1. A keresőbe írd be: *App Services*, majd *Create* » *Web App*.
1. *Basics* fül:
    - *Resource group*: `rg-<neptun>`
    - *Name*: `mc-web-<neptun>`
        - Ha a portál felajánlja a *Secure unique default hostname* lehetőséget, maradhat bekapcsolva. Ekkor az alkalmazás címe egy véletlen résszel bővül, a pontos címet mindig az alkalmazás *Overview* oldalán, a *Default domain* mezőben találod.
    - *Publish*: *Code*
    - *Runtime stack*: a projekted .NET verziója (a `.csproj` fájlban a `TargetFramework`, pl. `net8.0` » *.NET 8 (LTS)*)
    - *Operating System*: *Linux*
    - *Region*: a régiód
    - *Linux Plan*: *Create new*, a neve `plan-<neptun>`
    - *Pricing plan*: *Free F1* (ha nem látszik, *Explore pricing plans* » *Dev/Test* » *Free F1*)
1. *Database* fül: ne hozz létre adatbázist, már van.
1. *Deployment* fül: *Continuous deployment*: *Disable*, *Basic authentication*: *Disable*.
1. *Networking* fül: *Enable public access*: *On*.
1. *Monitoring* fül: *Enable Application Insights*: *No*.
1. *Review + create*, majd *Create*.

!!! info "A Free F1 csomag korlátai"
    - Alkalmazásonként napi 60 CPU-perc.
    - Ha egy ideig nem érkezik kérés, az alkalmazás leáll, és a következő kérésnél újraindul. Az első kérés ezért akár egy percig is tarthat.
    - Nincs saját domain, TLS-tanúsítvány és *deployment slot*.

## 6. Web Appban adatbáziselérés beállítása

1. A Web App oldalán: *Settings* » *Environment variables* » *Connection strings* fül » *Add*.
1. *Name*: ugyanaz a név, amit a kód használ (`DB<neptun kód>`). *Value*: az ADO.NET connection string a jelszóval. *Type*: *SQLAzure*.
1. *Apply*, majd a lap alján újra *Apply*, és erősítsd meg. Az alkalmazás ekkor újraindul.

!!! info "Hogyan éri el a kód ezt a beállítást?"
    Az App Service a connection stringet környezeti változóként adja át az alkalmazásnak (`SQLAZURECONNSTR_DB<neptun kód>`), amit az ASP.NET Core konfigurációs rendszere `ConnectionStrings:DB<neptun kód>` kulcsként lát. Azure-ban az alkalmazás *Production* környezetben fut, ezért ott sem az `appsettings.Development.json`, sem a user secrets nem töltődik be: a connection stringet csak innen kaphatja meg.

## 7. Telepítés parancssorból

A telepítés két lépésből áll: elkészíted a futtatható csomagot (*artifact*), majd feltöltöd az App Service-be. A 3. feladatban a GitHub Actions pontosan ugyanezt fogja automatikusan elvégezni.

1. Készítsd fel az alkalmazást az átmeneti hibákra. A felhőben az adatbázis kapcsolat időnként rövid ideig nem elérhető (pl. az ingyenes adatbázis hidegindítása alatt). Ilyenkor nem hibaoldalt kell mutatni, hanem újra kell próbálkozni. Az EF Core ezt beépítetten tudja, csak be kell kapcsolni a *Program.cs*-ben, ahol a `DbContext`-et regisztráltad:

    ``` C#
    builder.Services.AddDbContext<MovieCatalogDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DB<neptun kód>"),
            sql => sql.EnableRetryOnFailure()));
    ```

1. Jelentkezz be az Azure-ba. Megnyílik egy fiókválasztó ablak, ott az egyetemi fiókodat válaszd:

    ```powershell
    az login
    ```

    A bejelentkezés után a parancs kilistázza az elérhető előfizetéseket, és megáll, választásra vár (*Select a subscription and tenant*). Ellenőrizd, hogy a csillaggal jelölt alapértelmezett előfizetés az *Azure for Students*, és nyomj Entert (vagy írd be a sorszámát). Amíg nem válaszoltál, ne írj be más parancsot, mert azt a választásnak veszi.

    Ha a lista előtt olyan üzenetet látsz, hogy egyes bérlőkben (*tenant*) nincs elérhető előfizetés, az nem hiba: a fiókod más szervezetekhez is tartozhat.

1. A repository gyökeréből, PowerShellben készítsd el és töltsd fel a csomagot:

    ```powershell
    # 1. A futtatható csomag elkészítése a publish/web mappába, majd tömörítése
    dotnet publish MovieCatalog.Web -c Release -o publish/web
    tar.exe -a -c -f publish/web.zip -C publish/web .

    # 2. A csomag feltöltése az App Service-be
    az webapp deploy --resource-group rg-<neptun> --name mc-web-<neptun> --src-path publish/web.zip --type zip
    ```

- A `dotnet publish` lefordítja az alkalmazást, és egy mappába gyűjti mindazt, ami a futtatáshoz kell.
- A `tar.exe` (Windows 10 óta a rendszer része) a `-a` kapcsoló miatt a kiterjesztés alapján zip formátumot készít, a `-C publish/web .` pedig azt jelenti, hogy a mappa *tartalmát* tömöríti, nem magát a mappát.
- Az `az webapp deploy` feltölti a zip fájlt az App Service telepítő szolgáltatásába (*Kudu*), amely kicsomagolja a `/home/site/wwwroot` mappába, majd újraindítja az alkalmazást.
- Az aktív előfizetést bármikor ellenőrizheted az `az account show --output table` paranccsal, átállítani az `az account set --subscription "Azure for Students"` paranccsal lehet.

!!! warning "A publish mappa helye"
    Ne a projekt mappáján belülre publikálj. A következő publikáláskor a csomag a korábbi kimenetet is tartalmazná, ami hibához vezet. A `publish` mappát vedd fel a `.gitignore`-ba.

??? tip "Ha nincs Azure CLI: telepítés Visual Studióból"
    1. A projekten jobb klikk » *Publish* » *Azure* » *Azure App Service (Linux)*.
    1. Jelentkezz be, válaszd ki az előfizetést és a már létrehozott `mc-web-<neptun>` alkalmazást, majd *Finish*.
    1. *Publish*.

    A Visual Studio a háttérben ugyanezt teszi: `dotnet publish`, majd a csomag feltöltése.

## 8. Ellenőrzés és naplók

1. A Web App *Overview* oldalán kattints a *Default domain* címre. Az első betöltés lassú lehet, ha a weboldal vagy az adatbázis éppen nem fut: ilyenkor meg kell várni, amíg automatikusan elindulnak.
1. Ha hibaoldalt kapsz, kapcsold be a naplózást: *Monitoring* » *App Service logs* » *Application logging*: *File System*, majd *Save*. Ezután nyisd meg a *Monitoring* » *Log stream* oldalt, és frissítsd az alkalmazást a böngészőben. A kivételek itt jelennek meg.

Gyakori hibák:

- **A connection string neve nem egyezik** a kódban használttal: a kód `null` értéket kap.
- **Hiányzik az *Allow Azure services...* tűzfalszabály**: `Cannot open server ... requested by the login. Client with IP address ... is not allowed to access the server.`
- **Az adatbázis éppen hidegindítás alatt van**: időtúllépés vagy *Database ... is not currently available* (40613-as hiba) az első kérésnél. Várj egy percet, és frissíts. Ha az `EnableRetryOnFailure` be van kapcsolva, az alkalmazás magától újrapróbálkozik.
- **Az `az webapp deploy` sokáig áll a *Starting the site...* állapotban**: a parancs megvárja, amíg az alkalmazás elindul és válaszol. Ha közben az alkalmazás a hidegindítás alatt álló adatbázisra vár, ez percekig is tarthat. A telepítés ettől sikeres, a parancsot meg is szakíthatod (++ctrl+c++). Elkerülhető, ha telepítés előtt elindítod az adatbázist, pl. egy lekérdezéssel a portál *Query editor* felületén.

## Beadandó

| Kép tartalma | Fájlnév (kiterjesztés nélkül) |
|--------------|-------------------------------|
| Az erőforráscsoport *Overview* oldala, látszik az összes erőforrás (SQL szerver, adatbázis, App Service csomag, Web App) | `f1_rg` |
| Az adatbázis *Overview* oldala, látszik az ingyenes keret | `f1_sqldb` |
| A Query editorban futtatott `SELECT COUNT(*) FROM Titles;` lekérdezés eredménye (vagy a hálózati tiltás esetén a betöltő szkript kimenete az SSH konzolban) | `f1_data` |
| A Razor alkalmazás telepítése: az `az webapp deploy` sikeres kimenete (vagy a Visual Studio sikeres publikálása) | `f1_deploy` |
| A Razor Web App *Environment variables* » *Connection strings* oldala (az érték ne látszódjon) | `f1_connstr` |
| A Razor alkalmazás főoldala az Azure-ban (az Azure-os cím látszik a címsorban) | `f1_web` |
| A Razor alkalmazás *Log stream* oldala egy oldalbetöltés után | `f1_log` |

## Következő feladat

Folytasd a [következő feladattal](Feladat-2.md).
