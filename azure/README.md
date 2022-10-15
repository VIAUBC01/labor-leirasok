# Azure webhoszting

A labor során .NET Core alapú, adatbázist használó webalkalmazásokat kell Azure-ba telepíteni Azure SQL, illetve Azure App Service szolgáltatásokra építve. A műveleteket parancssorban vagy webes felületen is végezhetjük.

## Előkészületek

### Ha parancssorban dolgoznál
A mérés Windows és Linux rendszeren is teljesíthető. Telepítés előtt ajánlott ellenőrizni a lenti parancsokkal, hogy mi van már eleve feltelepítve.

Telepítendő parancssoros eszközök (ha még nincsenek telepítv):    
 - Azure CLI - [Windows](https://aka.ms/installazurecliwindows) [Linux](https://docs.microsoft.com/hu-hu/cli/azure/install-azure-cli) 
 - [git](https://git-scm.com/downloads) (Visual Studio telepítő is feltelepíti)
 - [.NET Core 6.0 SDK](https://docs.microsoft.com/hu-hu/dotnet/core/install/) (Visual Studio telepítő is feltelepíti)

Egyéb kellékek:
 - [Windows Terminal](https://www.microsoft.com/hu-hu/p/windows-terminal/9n0dx20hk701?rtc=1&activetab=pivot:overviewtab) (opcionális, csak Windows-on telepíthető)
 - valamilyen szövegszerkesztő, pl. jegyzettömb, [Visual Studio Code](https://code.visualstudio.com/)

A legjobb, ha Windows Terminal-t tudunk használni, de jó a [Visual Studio Code terminálja](https://code.visualstudio.com/docs/terminal/basics) is.

#### Telepítés ellenőrzése

Azure CLI ellenőrzése, ajánlott verzió legalább v2.13
```bash
az --version
```
git ellenőrzése, ajánlott verzió legalább v2.28 (Windows), v2.17 (Linux)
```bash
git --version
```
.NET Core SDK ellenőrzése, legalább v6.0 legyen
```bash
dotnet --version
```

### Ha böngészőben dolgoznál

Elsődlegesen böngésző kell csak.

Egyéb kellékek:
 - valamilyen szövegszerkesztő, pl. jegyzettömb, [Visual Studio Code](https://code.visualstudio.com/)

### Azure előfizetés beüzemelése

A mérés hallgatói Azure előfizetéssel teljesítendő, [itt aktiváld](https://azure.microsoft.com/en-us/free/students/).

Bővebb leírás a [Felhő alapú szoftverfejlesztés tárgy honlapjáról](https://www.aut.bme.hu/Course/felho)
> [Azure for Students](https://azure.microsoft.com/en-us/free/students/) - ez az újabb program, 100$ kredittel egy évre (de évente megújítható). Bankkártya regisztráció nem szükséges. Ezt próbáljtátok meg aktiválni! Edu.bme.hu-s fiókot érdemes használni az aktivációhoz. Fontos, hogy 10-11 hónap után indul egy megújítási időablak és ha abban nem újítjátok meg, akkor ki kell várni míg egy több hónapos inaktív időszak után teljesen törődik az előfizetés és utána tudtok (ugyanazzal az email címmel) egy teljesen új előfizetést igényelni.

## Feladatok

### Általános szabályok

:warning: A következő Azure erőforrások nevének *kötelezően* tartalmaznia kell a mérést végző neptun kódját: 

  - Azure SQL Server (`az sql server create` parancs hozza létre) 
  - Azure App Service (`az webapp create` parancs hozza létre). 

Ha névütközés miatt nem lehet simán a neptun kód, akkor kerüljön elé és/vagy mögé pár extra karakter. ABC123 neptun kód esetén az Azure SQL Server neve lehet például `abc123srv`.


### Feladat 1

Végzed el a következő hivatalos Microsoft oktatóanyagot: [magyarul (gépi fordítású)](https://docs.microsoft.com/hu-hu/azure/app-service/tutorial-dotnetcore-sqldb-app) vagy [angolul](https://docs.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app). Az angol az ajánlott, mivel az az eredeti, a magyar gépi fordítás sem rossz, követhető, de néhány helyen rosszul ragoz vagy furán fogalmaz. Mielőtt nekiállnál, mindenképp olvasd el az eltérések részt, a tippeket és a beadandók leírását!

Az útmutató külön füleken megmutatja, hogy az egyes lépéseket hogyan lehet végrehajtani többfajta eszközzel (Azure CLI, Azure portal, stb.). Szabadon választhattok, hogy milyen eszközzel dolgoztok, melyik fület választjátok, de az elején érdemes eldönteni és végig ahhoz tartni magatokat.

#### :bulb: Tippek és hasznos tudnivalók parancssort használóknak:

:bulb: érdemes legalább két konzolablakot használni, mindkettőben ugyanabban a könyvtárban állni, de az egyikben csak az Azure CLI (`az` kezdetű) parancsokat futtatni, a másikban minden mást

:bulb: a munkakönyvtár (ahol a parancssorunk áll) legyen egyszerű, pl. c:\work (Windows) vagy ~/src (Linux)

:bulb: érdemes egy jegyzettömböt is nyitni és a különböző többször használatos értékeket ment közben feljegyezni (connection string, erőforráscsoport neve, stb.)

:warning: ha Visual Studio Code-ot használunk szövegszerkesztőként, akkor minden kódfájl módosítás után explicit mentsük el a fájlt (CTRL+S), különben a `git`, `dotnet ef` parancsok nem fogják érzékelni a változásokat

#### :bulb: Tippek és hasznos tudnivalók mindenkinek:

:bulb: ha az Azure-ban futó webalkalmazással gond van, érdemes előrevenni a [diagnosztikai naplók lekérdezéséről szóló részt](https://learn.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?tabs=azure-portal%2Cvisualstudio-deploy%2Cdeploy-instructions-azure-portal%2Cazure-portal-logs%2Cazure-portal-resources#8---configure-and-stream-application-logs). A diagnosztikai naplókat már az App Service létrehozása után be lehet kapcsolni.

### [Feladat 2](Feladat-2.md)

Ez már nem Microsoft anyag, minden infó a [feladat oldalán](Feladat-2.md).

## Beadandó

### Általános elvek

Beadandó egy összecsomagolt állomány, melyben képernyőképek vannak *jpg* vagy *png* formátumban. 

Terminálparancsok kimenetéről készült képernyőképeknél: 

- a terminál ablaka teljes méretű (maximalizált) legyen
- ha a parancs kimenete olyan hosszú, hogy nem férne rá egy képernyőre, akkor görgessetek föl, hogy a parancs a képernyő tetején legyen és így csináljátok a képernyőképet. (Ha így sem fér rá, nem baj, ilyenkor már nem kell a teljes kimenetnek látszani) [Példa](media/sql_create.png)
- bár elsődleges a kimenet, látszódjon a futtatott parancs és alatta a kimenet is

Böngészőről készült képernyőképeknél:

- a böngészőablak teljes méretű (maximalizált legyen) 
- látszódjon a címsor és a weboldal megjelenő része
- Azure portálnál látszódjon a jobb felső sarokban az Azure portálra belépett felhasználó a jobb felső sarokban

A maximális feltöltési méret 15 MB. Ha a túl nagy képek miatt a feltöltendő tömörített fájl ennél nagyobb lenne, át lehet méretezni (nem levágni!) a képeket, de a szövegeknek olvashatónak kell maradni.

### Beadandó - Feladat 1

- képernyőképek az alábbi parancsok kimenetéről:
    - **Azure CLI belépés** (`az login`); képernyőkép fájlneve kiterjesztés nélkül: `f1_azlogin`
    - **Azure SQL szerver létrehozása;** `f1_sqlsrv`
    - **Azure SQL adatbázis létrehozása;** `f1_sqldb`
    - **Azure App Service Plan létrehozása;** `f1_appplan`
    - **Azure Web App létrehozása;** `f1_app`
    - **git push Azure-ba - az eredeti, első push**; `f1_push1`
    - git push Azure-ba - a módosított (`Done` property hozzáadása után); `f1_push2`
    - **diagnosztikai napló lekérdezése (`az webapp log tail`) egy új teendő létrehozása után** (ha a módosítás nem készült el, akkor lehet az eredeti változatról is). A lekérdezett naplóüzenetek tartalmával kapcsolatban nincs elvárás, nem kell pl. a létrehozás kérésnek látszania; `f1_log`

- képernyőképek a böngészőben futó Azure Web App főoldaláról:
    - **az eredeti változat futása** ([példa](media/tutorial-dotnetcore-sqldb-app/azure-app-in-browser.png)); `f1_v1`
    - a módosított változat futása ([példa](media/tutorial-dotnetcore-sqldb-app/this-one-is-done.png)); `f1_v2`

- képernyőképek az Azure portálról:
    - **az Azure Web App áttekintő oldala** ([példa](media/tutorial-dotnetcore-sqldb-app/web-app-blade.png)); `f1_portal`

### Beadandó - Feladat 2

- képernyőképek az alábbi parancsok kimenetéről:
    - Azure CLI belépés (`az login`); `f2_azlogin` (csak ha nem ugyanaz a sandbox előfizetés, mint az első feladatnál)
    - Azure SQL adatbázis létrehozása; `f2_sqldb`
    - Azure Web App létrehozása; `f2_app`
    - git push Azure-ba - az eredeti, első push; `f2_push`
    - diagnosztikai napló lekérdezése egy film módosítása után (`az webapp log tail`) . A lekérdezett naplóüzenetek tartalmával kapcsolatban nincs elvárás, nem kell pl. a módosítás kérésnek látszania; `f2_log`

- képernyőképek a böngészőben futó Azure Web App-ról:
    - főoldalról; `f2_index`
    - valamely film szerkesztő oldaláról; `f2_detail`

- képernyőképek az Azure portálról:
    - az Azure Web App áttekintő oldala; `f2_portal`

## Értékelési irányelvek

- ha nincs meg minden **kiemelt** kép az első feladatból :arrow_right: 1
- ha csak a **kiemelt** képek készülnek el az első feladatból :arrow_right: 2
- ha minden elkészül az első feladatból :arrow_right: 3
- ha minden elkészül az első és a második feladatból is :arrow_right: 5

