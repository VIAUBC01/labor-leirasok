# Angular Mérés

Ennek a labornak a célja az Angular keretrendszer használatának gyakorlása. 

Az alkalmazás a múlt laboron elkészített Twitter szervert fogja használni, amivel rövid szöveges üzeneteket (tweeteket) tudunk elküldeni, lekérdezni. 

Mit kell tudni ehhez a laborhoz?
* JavaScript és TypeScript alapok.
* Az Angular keretrendszer működése és az Angular alapfogalmainak (modulok, komponensek, service-ek) ismerete.

A twitter szerver, amely a REST API-t biztosítja a `server` alkönyvtárban található. A [`server.md`](server.md) fájl leírja, hogyan tudjuk a szervert futtatni. Készítsük elő a szervert, majd indítsuk el, mielőtt elkezdjük a kliens fejlesztését. 

## Beadandó
1. A kliens forráskódja, a `node_modules` mappa nélkül!!!
1. Jegyzőkönyv PDF formátumban
  * Tartalmazza a hallgató adatait (név, neptun kód), dátumot
  * A feladatleírásban pontosan szerepel, hogy miről kell írni, vagy képernyőképet készíteni a jegyzőkönyvben. Amit külön nem említünk, arról nem kell írni.

## Architektúra

Mielőtt nekilátunk a kód elkészítéséhez, tekintsük át, hogy az alkalmazás hogyan fog működni.

![Architektúra](architektura.png)

* A backend alkalmazás a twitter szerver, amely egy adott hálózati címen (IP cím és portszám) figyel és várja a bejövő kéréseket. HTTP végpontokat biztosít majd, amelyen keresztül az üzenetek publikálása és lekérdezése megvalósítható. Ezt nevezzük REST API-nak. Tegyük fel, hogy a backend alkalmazást a `localhost:3000` hálózati címen érjük el. Ekkor például a `localhost:3000/api-tweets` URL-re küldött HTTP GET kérés visszaadhatja az összes tweetet, vagy az ugyanerre a címre küldött `POST` kéréssel küldhetünk új tweetet. (Részleteket lásd a [szerver alkalmazás leírásában](server.md).)
* A backend JavaScript kódját NodeJS futtatja.
* A böngészőben futó webalkalmazást Angular keretrendszer segítségével készítjük el. Angular fejlesztés során HTML sablonokat és TypeScript fájlokat írunk. Az Angular fordító (`ng`) a *build*elés során ezekből legenerálja a statikus HTML, CSS és JavaScript fájlokat, amiket a böngésző futtat. 
* Ahhoz, hogy a böngészőben futtatni tudjuk a webalkalmazást, kell egy webszerver, ami kiszolgálja a statikus fájlokat. Például, tegyük fel, hogy a `localhost:4200` címen figyel a webszerver. A böngészőben futó webalkalmazásnak a backenddel is kommunikálnia kell. Ez megtehető úgy, hogy a webalkalmazás közvetlenül a backend hálózati címére küldi a kéréseit, vagy úgy, hogy a webszervert felhasználjuk proxy-nak. Ez azt jelenti, hogy ha olyan kérés érkezik a webszerverhez, amely nem egy statikus fájlra vonatkozik, akkor a kérést közvetlenül továbbítja a backendnek. Tehát ha a böngésző a webszervernek a `localhost:4200/index.html` címére küld egy HTTP GET kérést, akkor az visszaadja az eltárolt `index.html` fájlt, de ha a `localhost:4200/api/tweets` címére küld egy kérést, akkor azt továbbítja a `localhost:3000/api/tweets` URL-re. Így a böngészőben futó webalkalmazás nem is tudja, hogy a háttérben egy másik backend alkalmazás van, nem kell tudnia annak hálózati címét. Ennek a megközelítésnek pl. biztonság szempontjából is több előnye van. 

Szerencsére az Angular keretrendszer biztosít nekünk egy teszt webszervert, amellyel futtathatjuk az Angular alkalmazásunkat és amely proxyzásra is képes. 


Ha sikerült [elindítani a szerver alkalmazás](server.md)t, [kezdjük a kliens alkalmazással](client.md).


