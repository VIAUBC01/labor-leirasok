# HTML + CSS + JavaScript/TypeScript-mérés

Ennek a labornak a célja a három alapvető webes technológia, a HTML, a CSS és a JavaScript (TypeScript) nyelvek
gyakorlása egy összetett példán keresztül. A feladat egy Twitterhez hasonló, rövid üzenetek publikálására és
lekérdezésére alkalmas webalkalmazás és egy azt kiszolgáló szerver elkészítése lesz.

A megvalósítandó alkalmazás két részből áll:

1. A backend a [Node.js](https://nodejs.org/en/) JavaScript-motoron
   futtatott [ExpressJS keretrendszert](https://expressjs.com/) használó alkalmazás, amit TypeScript nyelven kell
   elkészíteni.
2. A frontend egy egyszerű HTML-oldal és hozzá tartozó JavaScript-kód, amelyet szintén TypeScript nyelven fogunk
   megírni. Ebben a példában még nem használunk semmilyen frontend keretrendszert.

A feladathoz semmilyen kezdő kódot nem használunk fel, mindent mi fogunk megírni, a projekteket is mindenki maga készíti
el. Fejlesztéshez és futtatáshoz mindössze a [Node.js](https://nodejs.org/en/)-re lesz szükség, továbbá egy
szövegszerkesztőre. Javasuljuk a [Visual Studio Code (`vscode`)](https://code.visualstudio.com/) használatát. (Egyik
feladathoz szükség lesz egy ingyenes segédlkalmazásra, de erről majd később részletesen.)

## Beadandó

1. Forráskód: mind a kliens, mind a szerver forráskódja, a **`node_modules` mappa nélkül!**
2. Jegyzőkönyv:
    * PDF-ben
    * Tartalmazza a hallgató adatait (név, Neptun-kód) és a dátumot
    * A feladatleírásban pontosan szerepel, hogy miről kell írni, vagy képernyőképet készíteni a jegyzőkönyvben. Amit
      külön nem említünk, arról nem kell írni

## Értékelés

A vezetett rész elvégzésével az elégségest lehet megszerezni. Ezután minden önálló feladat elkészítése +1 jegy.

## Architektúra

Mielőtt nekilátunk a kód elkészítéséhez, tekintsük át, hogy az alkalmazás milyen komponensekből fog állni:

![Architektúra](architektura.png)

* A backendalkalmazás egy szerver, amely egy adott hálózati címen (IP-cím és portszám) figyel és várja a bejövő
  kéréseket. HTTP-végpontokat biztosít majd, amelyen keresztül az üzenetek publikálása és lekérdezése megvalósítható.
  Ezt nevezzük REST API-nak. Tegyük fel, hogy a backendalkalmazást a `localhost:3000` hálózati címen érjük el. Ekkor
  például a `localhost:3000/tweets` URL-re küldött HTTP `GET` kérés visszaadhatja az összes tweetet, vagy az ugyanerre a
  címre küldött `POST` kéréssel küldhetünk új üzenetet.
    * A backend JavaScript-kódját a Node.js futtatja.
    * A JavaScript helyett TypeScript nyelven kódolunk, majd a TypeScript-fordítóval lefordítjuk a TypeScript-fájlokat
      JavaScript-fájlokká.
* A böngészőben futó webalkalmazás egy egyszerű HTML-fájlból és egyetlen hozzátartozó JavaScript-fájlból fog állni.
    * A JavaScript helyett TypeScript nyelven kódolunk, majd a TypeScript-fordítóval lefordítjuk a TypeScript-fájlt
      JavaScript nyelvű kódra.
* Ahhoz, hogy a böngészőben futtatni tudjuk a webalkalmazást, kell egy webszerver, ami kiszolgálja a statikus fájlokat.
  Például, tegyük fel, hogy a `localhost:8080` címen figyel a webszerver. A böngészőben futó webalkalmazásnak a
  backenddel is kommunikálnia kell. Ez megtehető úgy, hogy a webalkalamzás közvetlenül a backend hálózati címére küldi a
  kéréseit, vagy úgy, hogy a webszervert felhasználjuk proxynak. Ez azt jelenti, hogy ha olyan kérés érkezik a
  webszerverhez, amely nem egy statikus fájlra vonatkozik, akkor a kérést közvetlenül továbbítja a backendnek. Tehát ha
  a böngésző a webszervernek a `localhost:8080/index.html` címére küld egy HTTP `GET` kérést, akkor visszaadja az
  eltárolt `index.html` fájlt, de ha a `localhost:8080/tweets` címére küld egy kérést, akkor azt továbbítja a
  `localhost:3000/tweets` URL-re. Így a böngészőben futó webalkalmazás nem is tudja, hogy a háttérben egy másik
  backendalkalmazás van, nem kell tudnia annak hálózati címét. Ennek a megközelítésnek pl. biztonság szempontjából is
  több előnye van.

[Kezdjük a szerveralkalmazással.](feladat1.md)
