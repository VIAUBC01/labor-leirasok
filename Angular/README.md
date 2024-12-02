# Angular mérés

Ennek a labornak a célja az Angular keretrendszer használatának gyakorlása. 

Az alkalmazás a múlt laboron elkészített Twitter-szervert fogja használni, amivel rövid szöveges üzeneteket (tweeteket) tudunk elküldeni, lekérdezni. 

Mit kell tudni ehhez a laborhoz?
* JavaScript- és TypeScript-alapok.
* Az Angular keretrendszer működése és az Angular alapfogalmainak (modulok, komponensek, service-ek) ismerete.

A Twitter-szerver, amely a REST API-t biztosítja, a `server` alkönyvtárban található. A [`server.md`](server.md) fájl leírja, hogyan tudjuk a szervert futtatni. Készítsük elő a szervert, majd indítsuk el, mielőtt elkezdjük a kliens fejlesztését. 

## Beadandó
1. A kliens forráskódja **a `node_modules` mappa nélkül!**
1. A jegyzőkönyv PDF-ben.
  * Tartalmazza a hallgató adatait (név, Neptun-kód) és a dátumot.
  * A feladatleírásban pontosan szerepel, hogy miről kell írni vagy képernyőképet készíteni a jegyzőkönyvbe. Amit külön nem említünk, arról nem kell írni.

## Architektúra

Mielőtt nekilátunk a kód elkészítéséhez, tekintsük át, az alkalmazás hogyan fog működni.

![Architektúra](architektura.png)

* A backendalkalmazás a Twitter-szerver, amely egy adott hálózati címen (IP-cím és portszám) figyel és várja a bejövő kéréseket. HTTP-végpontokat biztosít majd, amelyeken keresztül az üzenetek publikálása és lekérdezése megvalósítható. Ezt nevezzük REST API-nak. Tegyük fel, hogy a backendalkalmazást a `localhost:3000` hálózati címen érjük el. Ekkor például a `http://localhost:3000/api/tweets` URL-re küldött HTTP `GET` kérés visszaadja az összes tweetet, vagy az ugyanerre a címre küldött `POST` kéréssel küldhetünk új tweetet is. (A részleteket lásd a [szerveralkalmazás leírásában.](server.md))
* A backend JavaScript-kódját a Node.js futtatja.
* A böngészőben futó webalkalmazást az Angular keretrendszer segítségével készítjük el. Angular-fejlesztés során HTML-sablonokat és TypeScript-fájlokat írunk. Az Angular fordító (`ng`) a *build*elés során ezekből legenerálja a statikus HTML-, CSS- és JavaScript-fájlokat, amiket a böngésző futtat. 
* Ahhoz, hogy a böngészőben futtatni tudjuk a webalkalmazást, kell egy webszerver, ami kiszolgálja a statikus fájlokat. Például tegyük fel, hogy a `http://localhost:4200` címen figyel a webszerver. A böngészőben futó webalkalmazásnak a backenddel is kommunikálnia kell. Ez megtehető úgy, hogy a webalkalamzás közvetlenül a backend hálózati címére küldi a kéréseit, vagy úgy, hogy a webszervert felhasználjuk proxynak. Ez azt jelenti, hogy ha olyan kérés érkezik a webszerverhez, amely nem egy statikus fájlra vonatkozik, akkor a kérést közvetlenül továbbítja a backendnek. Tehát ha a böngésző a webszervernek a `http://localhost:4200/index.html` címére küld egy HTTP `GET` kérést, akkor az visszaadja az eltárolt `index.html` fájlt, de ha a `http://localhost:4200/api/tweets` címére küld egy kérést, akkor azt továbbítja a `http://localhost:3000/api/tweets` URL-re. Így a böngészőben futó webalkalmazás nem is tudja, hogy a háttérben egy másik backendalkalmazás van, és nem kell tudnia annak hálózati címét. Ennek a megközelítésnek pl. biztonság szempontjából is több előnye van. 

Szerencsére az Angular keretrendszer biztosít nekünk egy teszt webszervert, amellyel futtathatjuk az Angular-alkalmazásunkat és amely proxyzásra is képes. 


Ha sikerült elindítani a [szerveralkalmazást](server.md), kezdjük a [kliensalkalmazással](client.md)!


