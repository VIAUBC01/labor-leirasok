# Azure webhoszting

A labor során .NET Core alapú, adatbázist használó webalkalmazásokat kell Azure-ba telepíteni Azure SQL, illetve Azure App Service szolgáltatásokra építve. A műveleteket parancssorban, cross-platform eszközökkel végezzük.

## Előkészületek

A mérés Windows és Linux rendszeren is teljesíthető. Telepítés előtt ajánlott ellenőrizni a lenti parancsokkal, hogy mi van már eleve feltelepítve.

Telepítendő parancssoros eszközök:    
 - Azure CLI - [Windows](https://aka.ms/installazurecliwindows) [Linux](https://docs.microsoft.com/hu-hu/cli/azure/install-azure-cli) 
 - [git](https://git-scm.com/downloads) (Visual Studio telepítő is feltelepíti)
 - [.NET Core 3.1 SDK](https://docs.microsoft.com/hu-hu/dotnet/core/install/) (Visual Studio telepítő is feltelepíti)
 
Egyéb kellékek:
 - [Windows Terminal](https://www.microsoft.com/hu-hu/p/windows-terminal/9n0dx20hk701?rtc=1&activetab=pivot:overviewtab) (opcionális, csak Windows-on telepíthető)
 - valamilyen szövegszerkesztő, pl. jegyzettömb, [Visual Studio Code](https://code.visualstudio.com/)

### Telepítés ellenőrzése

Azure CLI ellenőrzése, ajánlott verzió legalább v2.13
```bash
az --version
```
git ellenőrzése, ajánlott verzió legalább v2.28 (Windows), v2.17 (Linux)
```bash
git --version
```
.NET Core SDK ellenőrzése, legalább v3.1 legyen
```bash
dotnet --version
```

### Azure előfizetés beüzemelése
A mérés ún. sandbox előfizetéssel teljesítendő. Ezt az alábbi weboldalon lehet aktiválni: [link](
https://docs.microsoft.com/en-us/learn/modules/develop-app-that-queries-azure-sql/3-exercise-create-tables-bulk-import-query-data)
A `Sign in to activate sandbox` gombra nyomva. Belépéshez az edu.bme.hu fiókot kell használni. Ha még nem volt korábban ilyen előfizetés aktiválva a fiókhoz, akkor a kért engedélyt is meg kell adni a Microsoft Learn oldalnak.

![Azure Sandbox activated](media/sandbox_activated.png)

Miután a fenti üzenet megjelenik, dolgozhatunk az előfizetéssel, de **ne a weboldal jobb oldalán lévő terminálon (Azure Cloud Shell)**, hanem egy sima terminálban, Windows-on Parancssorban (cmd) vagy Windows Terminal-ban (ez az ajánlott), Linuxon pedig a beépített terminálon.

```bash
az login
```

Ez egy böngészőlapot nyit meg, ahol be kell jelentkezni szintén az edu.bme.hu-s fiókkal. Ezt követően pár másodperc múlva a paramcs lefut, kilistázva az aktív előfizetéseket.

:warning::warning::warning: Fontos tudnivalók a sandbox előfizetésről:

- 4 óra időtartamig él
- egy fiókkal naponta max. 10 db. hozható létre
- nem minden régióban enged erőforrásokat létrehozni. Ha a `West Europe` régióban nem engedi, akkor használjuk a `Central US`-t,
- az előfizetésen belül nem hozható létre erőforráscsoport. Helyette egy már eleve létre van hozva `learn-<valamilyen azonosito>` névvel. A pontos nevet a következő paranccsal lehet lekérdezni

```bash
az group list
```

## Feladatok

### [Feladat 1](Feladat-1.md)

Ez egy hivatalos [Microsoft tutorial](https://docs.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app) - a gépi magyar fordítás kézileg magyarított változata. Tehát ne az eredetit nézzétek, hanem [ezt](Feladat-1.md). Mielőtt nekiállnál olvasd el az eltérések részt és a tippeket!

Mivel sandbox előfizetést használunk, van néhány :warning: fontos :warning: eltérés az eredeti, Microsoft által megálmodott folyamathoz képest:

- hiába kéri, hogy hozzunk létre erőforráscsoportot, nem tudunk, mert nincs hozzá jogunk. Helyette **minden további parancsban, ahol meg kell adni erőforráscsoportot**, a `myResourceGroup` helyett adjuk meg a sandbox előfizetésben alapból létrehozott erőforráscsoport nevét. Ennek a névnek a lekérdezését lásd fentebb az előfizetés beüzemelésénél.

- *Az Azure App Service alkalmazás kezelése[Feladat-1.md#az-azure-app-service-alkalmazás-kezelése]* részben az előfizetés nem a BME domain-ben jön létre (ahová a fiók alapból tartozik), ezért alapból valószínűleg nem látszanak a szolgáltatások az Azure portálon. Előbb át kell váltani a könyvtárat a learn.docs.microsoft.com-ra a jobb felső sarokban lévő ikonra kattintva.

:bulb: Tippek és hasznos tudnivalók:

:bulb: érdemes legalább két konzolablakot használni, mindkettőben ugyanabban a könyvtárban állni, de az egyikben csak az Azure CLI (`az` kezdetű) parancsokat futtatni, a másikban minden mást

:bulb: érdemes egy jegyzettömböt is nyitni és a különböző többször használatos értékeket ment közben feljegyezni (connection string, erőforráscsoport neve, stb.)

:bulb: ha az Azure-ban futó változattal gond van, érdemes előrevenni a [diagnosztikai naplók lekrdezéséről szóló részt](Feladat-1.md#diagnosztikai-naplók-lekérdezése-log-streaming-funkcióval)

:bulb: ha az első feladatot túl sokáig tartott megoldani (> 2óra), akkor érdemesebb új sandbox előfizetést aktiválni (azaz hagyni lejárni az előzőt). Semmilyen erőforrásra nincs szükség a második feladathoz az elsőből

:warning: ha Visual Studio Code-ot használunk szövegszerkesztőként, akkor minden kódfájl módosítás után explicit mentsük el a fájlt (CTRL+S), különben a `git`, `dotnet ef` parancsok nem fogják érzékelni a változásokat

:warning: a connection string beállítása csak a parancssori ablak bezárásig él

:warning: a környezeti változóban megadott connection string csak a lokális alkalmazásban működik

## Beadandó

- egy szöveges fájl (txt), ami tartalmazza a következő parancsok kimenetét (általában JSON formátumú):
    - Azure SQL szerver létrehozása
    - Azure SQL adatbázis létrehozása
    - App Service Plan létrehozása
    - App Service létrehozása

## Értékelés

