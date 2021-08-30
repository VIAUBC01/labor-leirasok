# Világjáró Android alkalmazás fejlesztése 
### Android 1 labor

Készítsen Kotlin nyelven Világjáró Android alkalmazást! Az alkalmazás az adatait a https://restcountries.eu/ oldalon részletezett API hívások segítségével töltse be.
Tanulmányozza és a böngészőben próbálja ki az oldalon lévő API végpont példákat.

* https://restcountries.eu/rest/v2/all 
* https://restcountries.eu/rest/v2/name/Hungary
* stb.

Érdemes először végigolvasni az összes feladatot, hogy aki minden funkciót szeretne megvalósítani, már az elején úgy tervezhessen, és ne közben kelljen módosítani az adatmodellen pl.
Az alkalmazás elkészítése során törekedjen a strukturált felépítésre, package-k szervezésére. Készítsen letisztult, ergonomikus felhasználói felületeket, figyeljen a vissza gomb megfelelő működésére, továbbá a hibák kezelésére és a felhasználó számára történő releváns visszajelzésekre.

## Értékelés
Az Alapok rész hiánytalan megvalósítása esetén sikeres (elégséges) a labor. Az Alapok részen kívül megoldott minden további részfeladat hiánytalan megvalósítása plusz egy jegyet jelent. A feladatok részben egymásra épülnek, ezért az ebből következő hiányok – feladat kihagyás esetén – szükség szerint áthidalhatók pl. dummy adatok vagy nem perzisztált adatok használatával.

## Feltöltés
* A megoldást (teljes projekt) egy ZIP file formájában kell feltölteni a https://www.aut.bme.hu/Members/MyResults.aspx oldalon.
* A ZIP-ből az \app\build\intermediates mappa kerüljön törlésre, ugyanakkor az \app\build\outputs\apk\debug\ mappában lévő APK fájl mindenképpen legyen benne. 
* A feltöltött ZIP file-ba kerüljön egy egyszerű, név-neptun-kóddal ellátott PDF dokumentáció, melyben szerepeljenek az egyes részfeladatok nevei, mint alfejezetek, és ezekbe kerüljön 1-2 képernyőkép az elkészült funkcióról. Szükség szerint a dokumentáció tartalmazhat szöveges kiegészítéseket, rövid magyarázatokat.

## Tippek
* Törekedjen a rövid osztályokra és függvényekre, valamint az átlátható forráskódra 
* Ügyeljen a megfelelően hierarchikus package szervezésre, a kódolás során tartsa szem előtt a Clean Code elveket.
* A felhasználói felület a lehet egyszerűbb, nem elvárás látványos felületet készíteni.
* JSON to Kotlin konverter: https://http4k-data-class-gen.herokuapp.com/json 
* Kezelje megfelelően a készülék elforgatása során bekövetkező életciklus változásokat.
* Perzisztencia és hálózati hívásoknál figyeljen a megfelelő szálkezelésre!.
* Gondoljon az internetkapcsolat hiányára, a távoli kiszolgálók hibáira és ezek megfelelő lekezelésére, illetve a felhasználó megfelelő tájékoztatására.
* A Google Térkép használatához, valamint a szükséges API kulcs elkészítéséhez az Android Studio új projekt Google Maps Activity opciója jó példát mutat.
* A tesztelés Nexus 5X API 29, Android 10.0 (Google APIs) x86 emulátoron fog történni.
* Érdemes az előző féléves előadáson és a laborokon tanult ismereteket és projekteket alapul venni a feladatok megoldásához.
    * https://www.aut.bme.hu/Course/VIAUBB03 
    * https://github.com/bmeaut/VIAUBB03/tree/master/Mobil/El%C5%91ad%C3%A1s%20p%C3%A9ld%C3%A1k 
    * https://github.com/bmeaut/VIAUBB03/tree/master/Mobil/Labor 
* A feladat megvalósítása és beadása során önálló, egyedi munkákat várunk.

## 1. Alapok

* Állítson be az alkalmazásnak egyedi vagy saját készítésű ikont.
    * https://romannurik.github.io/AndroidAssetStudio/icons-launcher.html
* Valósítson meg navigációt egy tetszőleges megközelítéssel (NavigationDrawer, ViewPager, BottomNavigationView, Főmenü activity három gombbal stb.), amivel összesen három felület (Activity vagy Fragment) között lehet váltani az alkalmazásban. 
* Az első felületen készítsen RecyclerView alapú görgethető listát, amihez egy beviteli mező (pl. rögzített EditText és gomb a felület telején vagy AlertDialog FloatingActionButton-ra kattintva) segítségével lehet dinamikusan hozzáadni országokat a Retrofit osztálykönyvtár és a RestCountries API felhasználásával. Az egyes lista elemek tartalmazzák az ország angol nevét, hárombetűs országkódját (alpha3Code) és zászlóját. 
* A zászló képét a GlideToVectorYou osztálykönyvtár segítségével töltse be az API által visszaadott URL (flag) felhasználásával. 
* Törekedjen a hálózati adatforgalom minimalizálására! Használja az API által biztosított szűrési lehetőséget (filter response) a szükséges adatmezőkre (előretekintve a további feladatokra is). 
* Készítsen a Toolbar-on egy menüt, ami egy tetszőlegesen ideillő ikonként látható, kiválasztás esetén pedig egy Snackbar üzenetben kiírja az Ön nevét és Neptun-kódját. 

```xml
<?xml version="1.0" encoding="utf-8"?>
<menu xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto">
    <item
        android:id="@+id/info"
        app:showAsAction="always"
        android:icon="@android:drawable/ic_menu_info_details"
        android:title="Info" />
</menu>
```

```kotlin
override fun onCreateOptionsMenu(menu: Menu?): Boolean {
    menuInflater.inflate(R.menu.menu, menu)
    return true
}

override fun onOptionsItemSelected(item: MenuItem): Boolean {
    return when(item.itemId){
        R.id.info -> {
            Snackbar.make(binding.root, "Hello", Snackbar.LENGTH_LONG).show()
            true
        }
        else -> false
    }
}
```

 
## 2. Részletes nézet és Wikipedia

* Adott listaelem kiválasztása esetén a kiválasztott ország részletes nézete kerüljön betöltésre a második felületre, szépen elrendezve, címkékkel, mértékegységekkel. A részletes nézet tartalmazza legalább az ország nevét, ország nevét a saját nyelvén, fővárosát, népességét, területét, pénznemét. 
* Adott listaelem hosszú kiválasztása esetén pedig kiválasztott ország angol Wikipedia oldala töltsön be az alapértelmezett böngésző alkalmazásban. (a szóközt a Wikipedia automatikusan redirekteli alulvonásra) Pl. https://en.wikipedia.org/wiki/Hungary, https://en.wikipedia.org/wiki/Costa_Rica 

## 3. Perzisztencia

* Az alkalmazás tegye lehetővé a listához hozzáadott országok Room alapú perzisztens elmentését, onnan való kitörlését. Ehhez bővítse ki az egyes országokhoz tartozó listaelemet egy gombbal, amivel ki lehet törölni az aktuális országot. 
* Az alkalmazás ismételt elindítása esetén ezeknek az elmentett országoknak vissza kell töltődniük. 

## 4. Térkép

Ebben a feladatban már ne használjunk Retrofit hívást. Minden a perzisztens adatbázisból kerüljön felhasználásra.

* A harmadik felületen egy térkép (Google, OpenStreetMap stb.) jelenjen meg. 
* A térképen egy választott színű jelölővel szerepeljenek a listában szereplő országok az elmentett földrajzi koordinátákkal jelölt helyükön (latlng). 
* A jelölőre kattintva írja ki az ország nevét a saját nyelvén (nativeName) és fővárosát. 
