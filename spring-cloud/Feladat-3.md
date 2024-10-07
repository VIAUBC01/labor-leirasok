# 3. Feladat: API gateway

A légitársaság nem akarja összes alkalmazását (sem pedig a discovery vagy a config servert) a belső hálózaton kívülről érkezők számára elérhetővé tenni, csak bizonyos jól definiált végpontokat. Ezért bevezetünk egy API gateway-t, amely kívülről elérhető lesz, és a kívülről érkező kéréseket a megfelelő microservice-hez továbbítja. Ehhez természetesen tudnia kell, hogy hol futnak ezek a szolgáltatások. Egy API gateway másik tipikus feladata az autorizáció kezelése, itt szoktak valamilyen kliens oldalról küldött token alapján user infokat (pl. JWT token formájában) beletenni a header-ök közé, vagy megfelelő kliens oldali token hiányában elutasítani a kérést. Így a gateway mögötti microservice-eknek már csak a user info kiolvasása a feladata. Jelenlegi példánkban az autorizációval nem foglalkozunk, kizárólag a kérések route-olására fókszálunk.

1. Hozzuk létre a gateway szerver projektjét, Ismét célszerű a File > New > Spring Starter Project varázslót használni. A projekt neve legyen *gateway*. A következő oldalon a lehetséges függőségek közül válasszuk ki:

   - A Spring Cloud Routing csoportból a Reactive Gateway-t, fontos, hogy nem a "sima" Gateway-t. (Így egy kész gateway implementációt kapunk, amit csak fel kell konfigolnunk.)
   - A Spring Cloud Config csoportból a Config Client-et. (Így az API gateway is a config szerverről fogja venni a saját konfigját.)
   - A Spring Cloud Discovery csoportból az Eureka Discovery Client-et. (Így az API gateway is be fog jelentkezni az eureka servernél, valamint az eureka servertől fogja megtudni a többi szolgáltatás aktuális címét.)

2. A konfig szerver használatához hozzuk létre az src\main\resources alá a application.properties fájlt, a szokásos tartalommal:

   ```
   spring.application.name=gateway
   spring.config.import=optional:configserver:http://localhost:8081
   ```
   
3. A main osztályra (GatewayApplication) tegyük rá az *@EnableDiscoveryClient* annotációt. Így fog tudni a gateway szolgáltatás címeket keresni a discovery szervernél. 

4. A config projektben az src\main\resources\config alá hozzuk létre a gateway.yml konfig fájlt, ilyen sorokkal:

   ```
   server:
     port: 8080
   eureka:
     client:
       serviceUrl:
         defaultZone: http://localhost:8085/eureka/
   spring:
     cloud:
       gateway:
         routes:
         - id: currency
           uri: lb://currency
           predicates:
           - Path=/currency/**
           filters:
           - RewritePath=/currency(?<segment>/?.*), /api$\{segment}
         - id: bonus
           uri: lb://bonus
           predicates:
           - Path=/bonus/**
           filters:
           - RewritePath=/bonus(?<segment>/?.*), /api$\{segment}
         - id: flights
           uri: lb://flights
           predicates:
           - Path=/flights/**
           filters:
           - RewritePath=/flights(?<segment>/?.*), /api$\{segment}
   ```

   A port természetesen más legyen, ha a 8080-at már elhasználtad más célra, és az eureka portját is módosítsd, ha nálad nem 8085. A következő sorokban routing szabályok láthatók. Sorban, három különböző id alatt írjuk le, hogy milyen szabályok vonatkoznak a currency, bonus és flights alkalmazásokra. Csak a currency példáját írjuk le részletesen, a másik kettő ezzel teljesen analóg. 

   - Az uri: után azt írjuk le, hova kell továbbítani a kérést. Itt beégethetnénk egy konkrét URI-t, pl. http://localhost:8083 . Mi viszont nem ezt tesszük, hanem az *lb://currency* értékkel azt kérjük, hogy a gateway a discovery szervertől a currency alkalmazáshoz tartozó címek közül adjon vissza egyet. (Az lb a **l**oad **b**alancer rövidítése, hiszen több elérhető példány esetén így terheléselosztás valósul meg közöttük.)
   - A predicates alatt azt írjuk le, milyen feltételek esetén alkalmazódjon ez a szabály. Mi a - Path=/currency/** kifejezéssel azt érjük el, hogy ha a gatewayhez /currency-vel kezdődő uri-re érkezik a kérés, akkor az továbbítódjon a currency szolgáltatáshoz
   - Az eddigiek alapján, ahhoz, ha pl. a http://localhost:8080/currency/rate/USD/HUF címre érkezik egy kérés, az a http://localhost:8083/currency/rate/USD/HUF címre fog továbbítódni. Tudjuk viszont, hogy a tényleges cím ez lenne: http://localhost:8083/api/rate/USD/HUF . Ezt az eltérést több ponton is kezelhetjük, mi most azt a megoldást választjuk, hogy a routing szabálynál beállítunk egy path újraíró filtert is.  A RewritePath filterünk két, vesszővel elválasztott reguláris kifejezéssel pont azt éri el, hogy a http://localhot:8080/currency/rate/USD/HUF kérés a http://localhost:8083/api/rate/USD/HUF címre érkezzen be. (Feltéve, hogy ezeket a portokat konfigoltuk be.)

5. Indítsd el a légitársaság összes szerverét: config, discovery, gateway, flights, currency, bonus. Készíts két képernyőképet a jegyzőkönyvbe:

   1. Az elsőn látszódjon a currency.yml megnyitva az IDE-ben, és mellette egy böngészőben az átváltó API meghívásának eredménye, USD -> HUF irány esetére, viszont **a gateway-en keresztül meghívva!**
   2. A másodikon látszódjon a bonus.yml megnyitva az IDE-ben, és mellette egy böngészőben a saját NEPTUN kódodnak megfelelelő user bonusának lekérdezése, **a gateway-en keresztül meghívva!**

## Következő feladat

Folytasd a [következő feladattal](Feladat-4.md).

