# Adatbázist használó ASP.NET Core alkalmazás futtatása Azure App Service-ben

Az Azure app Service egy jól méretezhető, önjavító webes üzemeltetési szolgáltatást nyújt. Ez az oktatóanyag bemutatja, hogyan hozhat létre .NET Core-alkalmazást, és hogyan csatlakoztatható Azure SQL adatbázishoz. Az oktatóanyag eredménye egy, a Linux App Service-ben futó .NET Core MVC-alkalmazás lesz.


![App Service-ben futó alkalmazás](./media/tutorial-dotnetcore-sqldb-app/azure-app-in-browser.png)

Eben az oktatóanyagban az alábbiakkal fog megismerkedni:

- [x] SQL adatbázis létrehozása Azure-ban
- [x] .NET Core-alkalmazás csatlakoztatása Azure-beli adatbázishoz
- [x] Az alkalmazás üzembe helyezése az Azure-ban
- [x] Az adatmodell frissítése és az alkalmazás ismételt üzembe helyezése
- [x] Diagnosztikai naplók streamelése Azure-ból
- [x] Az alkalmazás kezelése az Azure portálon

## Előfeltételek

Az oktatóanyag elvégzéséhez:

* <a href="https://git-scm.com/" target="_blank">A Git telepítése</a>
* <a href="https://dotnet.microsoft.com/download/dotnet-core/3.1" target="_blank">A legújabb .NET Core 3.1 SDK telepítése</a>

## Helyi .NET Core-alkalmazás létrehozása

Ebben a lépésben a helyi .NET Core-projektet állíthatja be.

### A mintaalkalmazás klónozása

Egy parancssor ablakban a `cd` paranccsal lépjen be egy olyan könyvtárba, ahol a projektet tárolni szeretné.

Futtassa az alábbi parancsokat a git repository klónozásához és a gyökerére való módosításhoz.

```bash
git clone https://github.com/azure-samples/dotnetcore-sqldb-tutorial
cd dotnetcore-sqldb-tutorial
```

A mintaprojekt egy, az Entity Framework Core szolgáltatást használó, alapszintű CRUD (létrehoz-olvas-frissít-töröl) alkalmazást tartalmaz.

### Az alkalmazás futtatása

Futtassa az alábbi parancsokat a szükséges csomagok telepítéséhez, adatbázisok migrálásához és az alkalmazás elindításához.

```bash
dotnet tool install -g dotnet-ef
dotnet ef database update
dotnet run
```

Egy böngészőben nyissa meg a `http://localhost:5000` oldalt. Kattintson a **Create** hivatkozásra, és hozzon létre néhány _teendőt_.

![sikeres csatlakozás az Azure SQL-hez](./media/tutorial-dotnetcore-sqldb-app/local-app-in-browser.png)

Ha bármikor le szeretné állítani a futtatást, nyomja meg a `Ctrl+C` billentyűkombinációt a terminálon.

## Felhőbeli SQL adatbázis létrehozása

Ebben a lépésben egy Azure SQL adatbázist hozhat létre. Miután az alkalmazás üzembe lett helyezve az Azure-ban, ezt a felhőadatbázist használja.

Ez az oktatóanyag az SQL-adatbázisokhoz az [Azure SQL Database szolgáltatást](https://azure.microsoft.com/hu-hu/services/sql-database/) használja.

### Erőforráscsoport létrehozása

Az erőforráscsoport olyan logikai tároló, amelybe a rendszer üzembe helyezi és kezeli az Azure-erőforrásokat, mint például a webalkalmazásokat, adatbázisokat és a Storage-fiókokat. Dönthet úgy is például, hogy később egyetlen egyszerű lépésben törli a teljes erőforráscsoportot.

Parancssor használatával hozzon létre egy erőforráscsoportot az `az group create` paranccsal. A következő példában létrehozunk egy *myResourceGroup* nevű erőforráscsoportot a *Nyugat-Európa* régióban. Az **Ingyenes** szintű App Service-t támogató összes régió megtekintéséhez futtassa az `az appservice list-locations --sku FREE`parancsot.

```bash
az group create --name myResourceGroup --location "West Europe"
```

Az erőforráscsoportot és az erőforrásokat általában a közelében található régiókban hozhatja létre. 

A parancs befejeződésekor a JSON-kimenet megjeleníti az erőforráscsoport tulajdonságait.

### Azure SQL szerver létrehozása

Parancssor segítségével hozzon létre egy Azure SQL szervert az `az sql server create` paranccsal.

Cserélje le a *\<server-name>* helyőrzőt egy *egyedi* Azure SQL Database névre. Ez a név része lesz a szerver hálózati nevének a következő formában: `<server-name>.database.windows.net` . Érvényes karakterek:, `a` - `z` `0` - `9` `-` . Továbbá cserélje le a *\<db-username>* és a *\<db-password>* nevet az Ön által választott felhasználónévre és jelszóra. 

```azurecli-interactive
az sql server create --name <server-name> --resource-group myResourceGroup --location "West Europe" --admin-user <db-username> --admin-password <db-password>
```

Az SQL szerver létrehozása után a parancssoron az alábbi példához hasonló információk jelennek meg:

<pre>
{
  "administratorLogin": "&lt;db-username&gt;",
  "administratorLoginPassword": null,
  "fullyQualifiedDomainName": "&lt;server-name&gt;.database.windows.net",
  "id": "/subscriptions/00000000-0000-0000-0000-000000000000/resourceGroups/myResourceGroup/providers/Microsoft.Sql/servers/&lt;server-name&gt;",
  "identity": null,
  "kind": "v12.0",
  "location": "westeurope",
  "name": "&lt;server-name&gt;",
  "resourceGroup": "myResourceGroup",
  "state": "Ready",
  "tags": null,
  "type": "Microsoft.Sql/servers",
  "version": "12.0"
}
</pre>

### Tűzfalszabály beállítása

Hozzon létre az új Azure SQL szerverre vonatkozó tűzfalszabályt az `az sql server firewall create` parancs használatával. Ha a kezdő és a záró IP-cím is 0.0.0.0 értékre van állítva, a tűzfal csak más Azure-erőforrások számára van nyitva. 

```azurecli-interactive
az sql server firewall-rule create --resource-group myResourceGroup --server <server-name> --name AllowAzureIps --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0
```

:bulb: Még szigorúbb tűzfalszabályt is megadhat, ha csak azokat a kimenő IP-címeket engedélyezi, amelyeket alkalmazása használ.

Futtassa ismét a parancsot lecserélve a *\<your-ip-address>* helyőrzőt a [saját gépének IPv4 IP-címére](https://www.whatsmyip.org/) .

```azurecli-interactive
az sql server firewall-rule create --name AllowLocalClient --server <server-name> --resource-group myResourceGroup --start-ip-address=<your-ip-address> --end-ip-address=<your-ip-address>
```

### Adatbázis létrehozása

Hozzon létre egy S0 teljesítményszintű adatbázist a kiszolgálón az `az sql db create` parancs használatával.

```bash
az sql db create --resource-group myResourceGroup --server <server-name> --name coreDB --service-objective S0
```

### Connection string megszerzése

Az alábbi parancs használatával szerezze be a connection string-et.

```bash
az sql db show-connection-string --client ado.net --server <server-name> --name coreDB
```

:warning: A parancs kimenetében lévő connection string még nincs kész. Le kell cserélni *\<username>* *\<password>* értékeket a korábban (`az sql server create` parancsban) megadott rendszergazdai hitelesítő adatokkal. 

Az így előállt végleges connection string lesz a a .NET Core-alkalmazás connection string-je.

### Az alkalmazás konfigurálása felhőbeli adatbázishoz való kapcsolódáshoz

A letöltött repository-ban nyissa meg a Startup.cs fájlt, és keresse meg a következő kódot:

```csharp
services.AddDbContext<MyDatabaseContext>(options =>
        options.UseSqlite("Data Source=localdatabase.db"));
```

Cserélje le a következő kódra.

```csharp
services.AddDbContext<MyDatabaseContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("MyDbConnection")));
```

### Adatbázis előkészítése

Eddig az alkalmazás egy helyi SQLite-adatbázishoz csatlakozott. Most viszont az új Azure SQL adatbázisban kellene létrehozni az alkalmazás futtatásához szükséges objektumokat. A projekt eddigi migrációs fájljai sajnos nem használhatók, mivel azok SQLite-specifikusak.

A repository gyökerében törölje a `Migrations` mappát, majd futtassa a következő parancsokat. Cserélje le *\<connection-string>* helyőrzőt a korábban összerakott connection string-re.

```
# Migrációk újragenerálása
dotnet ef migrations add InitialCreate

# Connection string beállítása
# PowerShell esetén
$env:ConnectionStrings:MyDbConnection="<connection-string>"
# CMD esetén (macsaköröm nem kell)
set ConnectionStrings:MyDbConnection=<connection-string>
# Bash esetén
export ConnectionStrings__MyDbConnection="<connection-string>"

# Run migrations
dotnet ef database update
```

### Alkalmazás futtatása új konfigurációval

Most már az adatbázis elő van készítve az alkalmazás futtatására. Tesztelje a működést a következő futtatásával:

```
dotnet run
```

Egy böngészőben nyissa meg a `http://localhost:5000` oldalt. Kattintson a **Create** hivatkozásra, és hozzon létre néhány _teendőt_. Az alkalmazás ezeket az adatokat már az Azure SQL adatbázisba írja.

Véglegesítse a helyi módosításokat, majd véglegesítse azt a git repository-ban. 

```bash
git add .
git commit -m "connect to SQLDB in Azure"
```

Most már készen áll a magának az alkalmazásnak a felhőbe költöztetésére.

## Alkalmazás üzembe helyezése az Azure-ban

Ebben a lépésben egy Azure SQL-t használó .NET Core alkalmazást telepítünk az App Service szolgáltatásba.

### Git repository-ból történő telepítés konfigurálása

FTP vagy git segítségével is felmásolhatjuk az alkalmazásunkat a felhőbe. Ezt a műveletet egy kifejezetten erre a célra kitalált ún. _telepítési felhasználó_ nevében végezzük. Így bár a fiókhoz kapcsolódó webalkalmazások telepítésénél használható, nem kell megadnia az _előfizetéshez tartozó fiók_ adatait.

A telepítési felhasználó konfigurálásához futtassa az `az webapp deployment user set` parancsot. Cserélje \<username> és a \<password> helyőrzőket egy megfelelő felhasználónévre, illetve jelszóra. 

- A felhasználónévnek egyedinek kell lennie az Azure-on belül. Ha git-ből történő feltöltést szeretne, akkor nem tartalmazhatja a "@" szimbólumot. 
- A jelszónak legalább nyolc karakterből kell állnia, és a következő három karaktertípus közül kettőnek kell benne lennie: betűk, számok és szimbólumok. 

```shell
az webapp deployment user set --user-name <username> --password <password>
```

A kimenet a jelszót `null` értékként jeleníti meg. `'Conflict'. Details: 409` hibaüzenet esetén változtassa meg a felhasználónevet. `'Bad Request'. Details: 400` hibaüzenet esetén használjon erősebb jelszót. 

Jegyezze meg a felhasználónevet és a jelszót a webalkalmazások üzembe helyezéséhez.

### Linux alapú App Service Plan létrehozása

Hozzon létre egy App Service Plan szolgáltatást az erőforráscsoporton belül az `az appservice plan create` paranccsal.

A következő példában létrehozunk egy `myAppServicePlan` nevű, Linux alapú ( `--is-linux` ) App Service Plant  az **ingyenes** díjszabási szinten ( `--sku F1` ).

```azurecli-interactive
az appservice plan create --name myAppServicePlan --resource-group myResourceGroup --sku F1 --is-linux --location "West Europe"
```

Az App Service-csomag létrehozása után az Azure CLI az alábbi példához hasonló információkat jelenít meg:

<pre>
{ 
  "adminSiteName": null,
  "appServicePlanName": "myAppServicePlan",
  "geoRegion": "West Europe",
  "hostingEnvironmentProfile": null,
  "id": "/subscriptions/0000-0000/resourceGroups/myResourceGroup/providers/Microsoft.Web/serverfarms/myAppServicePlan",
  "kind": "linux",
  "location": "West Europe",
  "maximumNumberOfWorkers": 1,
  "name": "myAppServicePlan",
  &lt; JSON data removed for brevity. &gt;
  "targetWorkerSizeId": 0,
  "type": "Microsoft.Web/serverfarms",
  "workerTierName": null
} 
</pre>

### Azure Web App létrehozása

Hozzon létre egy webalkalmazást a `myAppServicePlan` App Service Plan-hez. 

Használja az `az webapp create` parancsot. A lenti példában cserélje ki az `<app-name>` nevet egy globálisan egyedi névre (érvényes karakterek: `a-z`, `0-9` és `-`). A futtatókörnyezet a példában `DOTNETCORE|3.1` lesz. Az összes támogatott futtatókörnyezet megtekintéséhez futtassa a parancsot `az webapp list-runtimes --linux`. 

```bash
# Bash, cmd
az webapp create --resource-group myResourceGroup --plan myAppServicePlan --name <app-name> --runtime "DOTNETCORE|3.1" --deployment-local-git
# PowerShell
az --% webapp create --resource-group myResourceGroup --plan myAppServicePlan --name <app-name> --runtime "DOTNETCORE|3.1" --deployment-local-git
```

A webalkalmazás létrehozása után az alábbi példához hasonló eredményt kapunk:

<pre>
Local git is configured with url of 'https://<username>@<app-name>.scm.azurewebsites.net/<app-name>.git'
{
  "availabilityState": "Normal",
  "clientAffinityEnabled": true,
  "clientCertEnabled": false,
  "cloningInfo": null,
  "containerSize": 0,
  "dailyMemoryTimeQuota": 0,
  "defaultHostName": "<app-name>.azurewebsites.net",
  "deploymentLocalGitUrl": "https://<username>@<app-name>.scm.azurewebsites.net/<app-name>.git",
  "enabled": true,
  < JSON data removed for brevity. >
}
</pre>

Ezzel létrehozott egy Linux alapú webalkalmazás-futtatókörnyezetet, 

:bulb: A Git remote URL-címe a `deploymentLocalGitUrl` tulajdonságban látható, a következő formátumban: `https://<username>@<app-name>.scm.azurewebsites.net/<app-name>.git`. Jegyezze fel ezt az URL-t, mert később még szüksége lesz rá.


### Connection string beállítása

A felhős környezetben elérhető connection string beállítás megadásához használja az `az webapp config appsettings set` parancsot. A következő parancsban cserélje le az *\<app-name>* és a  *\<connection-string>* paraméterek értékeit a korábban megadott alkalmazásnévre, illetve a korábban megszerzett connection string-re.

```bash
az webapp config connection-string set --resource-group myResourceGroup --name <app-name> --settings MyDbConnection="<connection-string>" --connection-string-type SQLAzure
```

A ASP.NET Core-ben ezt a connection string-et ( `MyDbConnection` ) a szokásos módon használhatja, hasonlóan mintha az *appsettings.json*-ben adta volna meg. Jelen esetben ugyan a `MyDbConnection` *appsettings.json*-ban is meg van adva. Ha az alkalmazás az App Service-ben fut, akkor a különböző helyen, de azonos névvel megadott beállítások közül az App Service beállításként megadott érték jut érvényre.

Ha szeretné megtudni, hogyan hivatkoznak a connection string-re a kódban, tekintse meg az [*Az alkalmazás konfigurálása felhőbeli adatbázishoz való kapcsolódáshoz*](#az-alkalmazás-konfigurálása-felhőbeli-adatbázishoz-való-kapcsolódáshoz) című részt.

### Git push Azure-ba

Adjon hozzá egy git remote-ot a repository-hoz. A lenti parancsban cserélje le a *\<deploymentLocalGitUrl-from-create-step>* elemet annak a korábban megszerzett git remote URL címére.

```bash
git remote add azure <deploymentLocalGitUrl-from-create-step>
```

Push-olja a kódot a távoli Azure mappába a következő paranccsal. Amikor a git kéri a bejelentkezési adatok megadását, győződjön meg arról, hogy a korábban létrehozott **telepítési felhasználó** adatait adja meg, nem pedig az Azure portálra való bejelentkezéshez használt fiók adatait.

```bash
git push azure master
```

A parancs futtatása eltarthat néhány percig. Futás közben a következő példához hasonló sorok jelennek meg:
<pre>
Enumerating objects: 273, done.
Counting objects: 100% (273/273), done.
Delta compression using up to 4 threads
Compressing objects: 100% (175/175), done.
Writing objects: 100% (273/273), 1.19 MiB | 1.85 MiB/s, done.
Total 273 (delta 96), reused 259 (delta 88)
remote: Resolving deltas: 100% (96/96), done.
remote: Deploy Async
remote: Updating branch 'master'.
remote: Updating submodules.
remote: Preparing deployment for commit id 'cccecf86c5'.
remote: Repository path is /home/site/repository
remote: Running oryx build...
remote: Build orchestrated by Microsoft Oryx, https://github.com/Microsoft/Oryx
remote: You can report issues at https://github.com/Microsoft/Oryx/issues
remote: .
remote: .
remote: .
remote: Done.
remote: Running post deployment command(s)...
remote: Triggering recycle (preview mode disabled).
remote: Deployment successful.
remote: Deployment Logs : 'https://&lt;app-name&gt;.scm.azurewebsites.net/newui/jsonviewer?view_url=/api/deployments/cccecf86c56493ffa594e76ea1deb3abb3702d89/log'
To https://&lt;app-name&gt;.scm.azurewebsites.net/&lt;app-name&gt;.git
 * [new branch]      master -> master
</pre>

### A telepített Azure webalkalmazás böngészése

Cserélje le az *\<app-name>* helyőrzőt az App Service szolgáltatás nevére az alábbi címben, majd látogasson el a kapott címre:

```bash
http://<app-name>.azurewebsites.net
```

Vegyen fel néhány teendőt az alkalmazáson belül.

![App Service-ben futó alkalmazás](./media/tutorial-dotnetcore-sqldb-app/azure-app-in-browser.png)

**Gratulálunk!** Egy adatvezérelt .NET Core-alkalmazást futtat az App Service-ben.

## Lokális módosítás érvényesítése a felhőben

Ebben a lépésben módosítjuk az adatbázissémát, és a módosítást ki is publikáljuk Azure-ba.

### Adatmodell frissítése

Nyissa meg a _models/todo. cs_ fájlt bármilyen szövegszerkesztőben. Adja hozzá a következő tulajdonságot a `ToDo` osztályhoz:

```csharp
public bool Done { get; set; }
```

### Adatbázis migráció újrafuttatása

Futtassa az alábbi néhány parancsot az új adatbázis migrációs lépés generálásához, illetve a változások adatbázisban történő érvényesítésére.

```bash
dotnet ef migrations add AddProperty
dotnet ef database update
```

:warning: ha időközben új parancssori ablakot nyitott, akkor újra be kell állítania az felhőbeli adatbázis connection string-jét a [korábban](#connection-string-megszerzése) látott módon.

### Az új tulajdonság használata

Hajtson végre néhány módosítást a kódban a `Done` property használatához. Ebben az oktatóanyagban az egyszerűség kedvéért csak az `Index` és a `Create` nézetet módosítjuk, de így is láthatja a prroperty-t működés közben.

Nyissa meg bármilyen szövegszerkesztőben a _Controllers/TodosController.cs_ fájlt.

Keresse meg a `Create([Bind("ID,Description,CreatedDate")] Todo todo)` metódust, és adja hozzá a `Done`-t a `Bind` attribútum listájához. Ezután a `Create()` metódus szignatúrája a következő kódhoz hasonló lesz:

```csharp
public async Task<IActionResult> Create([Bind("ID,Description,CreatedDate,Done")] Todo todo)
```

Nyissa meg a _Views/Todos/Create.cshtml_ fájlt.

A Razor-kódban látnia kell a `Description` property-hez tartozó `<div class="form-group">` elemet és egy másik, szintén `<div class="form-group">` elemet a `CreatedDate`-hez. Közvetlenül ezután a két elem után adjon hozzá egy `<div class="form-group">` elemet a `Done` property számára:

```csharp
<div class="form-group">
    <label asp-for="Done" class="col-md-2 control-label"></label>
    <div class="col-md-10">
        <input asp-for="Done" class="form-control" />
        <span asp-validation-for="Done" class="text-danger"></span>
    </div>
</div>
```

Nyissa meg a _Views/Todos/Index.cshtml_ fájlt.

Keresse meg az üres `<th></th>` elemet. Az elem fölé illessze be a következő Razor-kódot:

```csharp
<th>
    @Html.DisplayNameFor(model => model.Done)
</th>
```

Keresse meg a `asp-action` attribútumot tartalmazó `<td>` elemet. Az elem fölé illessze be a következő Razor-kódot:

```csharp
<td>
    @Html.DisplayFor(modelItem => item.Done)
</td>
```

Az `Index` és a `Create` nézetekben ennyi módosítás elég az új property átvezetéséhez.

### Módosítások tesztelése lokálisan

Futtassa lokálisan az alkalmazást.

```bash
dotnet run
```

:warning: ha időközben új parancssori ablakot nyitott, akkor újra be kell állítania az felhőbeli adatbázis connection string-jét a [korábban](#Connection_string_megszerzése) látott módon.

A böngészőjében navigáljon a `http://localhost:5000/` címre. Most már nem csak új teendőket vehet fel, hanem a felvétel során bejelölheti a **Kész** jelölőnégyzetet is. Bejelölve a teendőnek a főoldalon befejezettként kell megjelennie. Ne feledje, hogy az `Edit` nézetben nem jelenik meg a `Done` mező, mivel az `Edit` nézetet nem módosította.

### Módosítások publikálása a felhőbe

A korábban látottakhoz hasonlóan történik az alábbi git parancsokkal:

```bash
git add .
git commit -m "added done field"
git push azure master
```

A `git push` lefutása után navigáljon újra az App Service alkalmazáshoz, próbálkozzon a teendők hozzáadásával úgy, hogy egyúttal készre is jelöli a teendőt.

![Azure-alkalmazás a módosítás publikálása után](./media/tutorial-dotnetcore-sqldb-app/this-one-is-done.png)

A meglévő teendők továbbra is megjelennek. ASP.NET Core alkalmazás újbóli telepítésekor az Azure SQL adatbázis meglévő adatai nem vesznek el. Továbbá az Entity Framework Core Migrations csak az adatsémát módosítja, a meglévő adatokat érintetlenül hagyja.

## Diagnosztikai naplók lekérdezése log streaming funkcióval

Még ha az ASP.NET Core alkalmazás az Azure App Service-ben fut is, a konzolra kiírt naplóüzeneteket így is meg tudjuk szerezni.

A mintaprojektben már be van kapcsolva az ASP.NET Core naplózó alrendszere két kis módosítással:

- Hivatkozza `Microsoft.Extensions.Logging.AzureAppServices` NuGet csomagot a *DotNetCoreSqlDb.csproj*-ban.
- Meghívja a `loggerFactory.AddAzureWebAppDiagnostics()` függvényt a *Program.cs*-ben.

Az ASP.NET Core naplózási szintjének az `Information` alapértelmezett szintről az `Error` szintűre történő átállításához az `az webapp log config` parancs használható. Például:

```bash
az webapp log config --name <app-name> --resource-group myResourceGroup --application-logging true --level information
```

:bulb: A mintaprojekt naplózási szintje már eleve be van állítva `Information` értékre.

A *log streaming* funkció elindításához használja az `az webapp log tail` parancsot. Az *\<app-name>* helyőrzőt cserélje le az App Service nevére.

```bash
az webapp log tail --name <app-name> --resource-group myResourceGroup
```

A log streaming elindítása után navigáljon vagy frissítsen (Ctrl+F5) az alkalmazás oldalán, hogy forgalom generálódjon. Ekkor láthatja, hogy a rendszer leküldi a naplóbejegyzéseket a lokális parancsorba. Ha nem jelennek meg azonnal a konzolnaplófájlok, próbálkozzon ismét 30 másodperc múlva.

A `Ctrl`+`C` billentyűparanccsal bármikor leállíthatja a naplóstreamelést.

## Az Azure App Service alkalmazás kezelése

A létrehozott alkalmazás megtekintéséhez lépjen be az [Azure Portal](https://portal.azure.com)-ra az előfizetéshez tartozó fiókjával, majd keresse meg és válassza a **App Services** lehetőséget.

![Azure Portal - App Services kiválasztása](./media/tutorial-dotnetcore-sqldb-app/app-services.png)

A **App Services** lapon válassza ki az App Service nevét.

![Navigálás a portálon egy Azure App Service-hez](./media/tutorial-dotnetcore-sqldb-app/access-portal.png)

Alapértelmezés szerint a portál az alkalmazás **Áttekintés** lapját jeleníti meg. Ezen az oldalon megtekintheti az alkalmazás állapotát. Itt elvégezhet olyan alapszintű felügyeleti feladatokat is, mint a böngészés, leállítás, elindítás, újraindítás és törlés. Az oldal bal oldalán lévő lapok a különböző megnyitható konfigurációs oldalakat jelenítik meg.

![Az App Service lap az Azure Portalon](./media/tutorial-dotnetcore-sqldb-app/web-app-blade.png)
