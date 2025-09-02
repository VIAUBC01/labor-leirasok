# Microservice architektúra Spring Cloud platformon

A labor során 3 önálló Spring Boot-os alkalmazásból indulunk ki (*bonus, flights, currency*), amelyeket fokozatosan egészítünk ki egy Microservice architektúrához szükséges elemekkel (config server, service registre, API gateway). Az architektúra tesztelésére egy negyedik alkalmazás fog szolgálni (*booking*), amely az API gateway-en keresztül hív majd meg több REST API-t egy összetett üzleti logika elvégzéséhez.

A fő cél, hogy a Spring Cloud által adott lehetőségeket egy példán keresztül illusztráljuk, így a labor legnagyobb részében a leírásban szereplő konfigurációkat kell bemásolni, csak a legvégén szerepel egy kisebb önállóan megoldandó feladat. 

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- JDK 17, pl. innen: https://adoptium.net/?variant=openjdk17
- Tetszőleges Java alapú IDE, pl. Spring Tools 4 for Eclipse: https://spring.io/tools
- HTTP kérések egyszerű összeállítását lehetővé tevő fejlesztői eszköz, pl.: [Postman](https://www.postman.com/downloads/)

<hr />

A laborok elvégzéséhez használható segédanyagok és felkészülési anyagok:
- A Háttéralkalmazások tárgy kapcsolódó előadásai:
  - https://www.aut.bme.hu/Upload/Course/VIAUBB04/hallgatoi_jegyzetek/12-Microservices.pdf
    - https://web.microsoftstream.com/video/6e6642e0-bce5-48e5-bfcd-27d27cc22875?channelId=2c22f965-126a-4fe8-bbbd-71e5cd85a7c4
    - https://web.microsoftstream.com/video/1453a714-dcd7-4946-b53d-19fe60f67f4c?channelId=2c22f965-126a-4fe8-bbbd-71e5cd85a7c4

Hivatalos dokumentációk:

- Spring Cloud Config: https://docs.spring.io/spring-cloud-config/docs/3.0.5/reference/html/
- Spring Cloud Gateway: https://docs.spring.io/spring-cloud-gateway/docs/3.0.4/reference/html/
- Spring Cloud Netflix (Eureka): https://docs.spring.io/spring-cloud-netflix/docs/3.0.4/reference/html/
- Spring Cloud OpenFeign: https://docs.spring.io/spring-cloud-openfeign/docs/3.0.4/reference/html/

## Beadandó

A labor elvégzése után az alábbi tartalmat kérjük beadni a tanszéki portálra történő feltöltéssel, egy zipelt fájlban:
- **PDF** formátumban (DOCX nem elfogadott!) az egyes feladatoknál pontosan megnevezett: 
  - konkrét kódrészletekről készített képernyőkép(ek), 
  - 1 mondatos magyarázat
  - 1 vagy több ábra (jellemzően képernyőkép), ami a helyes működést hivatott bizonyítani.
- A booking projekt forrása (src\main mappa elegendő)

## Értékelés

A laborban négy feladatrész van. Jeles osztályzat az összes feladatrész elvégzésével kapható. Minden hiányzó, avagy hiányos feladatrész mínusz egy jegy.

## A meglévő kód áttekintése

Klónozd ezt a repository-t, és a spring-cloud mappában lévő 7 projektet importáld az IDE-be maven projektként! A bonus, currency és flights alkalmazásokban külön projektbe van kiszervezve a REST API-t definiáló interfész, ami majd hasznos lesz nekünk később. Ezek a bonus-api, currency-api és flights-api projektek. Ez a három önálló alkalmazás egy-egy jól definiált funkcióért felel, ezért a légitársaság, amelynek az üzletmenetét támogatják, 3 külön microservice formájában fejlesztette őket. Vegyük sorra a projekteket:

- bonus-api: 

  - A BonusApi interfész Spring MVC annotációkkal definiál két végpontot: egy adott user eddig összegyűjtött bónuszpontjait tudjuk lekérdezni (GET), vagy egy adott usernek tudunk új pontokat adni (PUT) 

- bonus:

  - A BonusController valósítja meg a BonusApi interfészt. A felhasználók bónuszpontjait az alkalmazás relációs adatbázisban tárolja. (Az egyszerűség kedvéért ez egy beágyazott H2 adatbázis, ahogy a pom függőségek között látható.) Az egyetlen táblát a *Bonus* JPA entitásra képezzük le, amelyet a BonusRepository interfészen keresztül manipulálunk. A bónusz lekérdező végpont megvalósításában látszik, hogy nem létező felhasználó lekérdezése esetén 0 pontot adunk vissza. Bónuszpont hozzáadásakor viszont a BonusService-be kiszervezett üzleti logika létrehoz egy új bejegyzést, 0 ponttal a felhasználóhoz, ha az még nem létezett, így mindenképp egy létező pontszámot módosítunk. A pontszám negatív is lehet, ezzel kezeljük a pontszám felhasználását vásárlási célból. 400-as Bad Request választ adunk, ha a meglévő pontszámnál nagyobbat akarnak levonni.

- currency-api:

  - A CurrencyApi interfész Spring MVC annotációkkal definiál egy végpontot: egyik valutáról a másikra adja meg az átváltási árfolyamot

- currency:

  - A CurrencyController valósítja meg a CurrencyApi interfészt, a CurrencyService felhasználásával. A CurrencyService az egyszerűség kedvéért csak memóriában beégetve tartalmaz 6 árfolyamot. Ami fontos lesz, hogy ugyanazon valuta oda- és visszairányú váltása nem egyenértékű, van egy konfigurálható átváltási jutalékkal dolgozik az alkalmazás. Az átváltási jutalék mértékét a konfigból ilyen módon nyerjük ki a CurrencyService-ben:

    ```
    @Value("${currency.exchangePremium}")
    private double exchangePremium;
    ```

    Majd így használjuk fel: 

    ```
    new RateDescriptor("HUF", "EUR"), 1.0 / 370,
    new RateDescriptor("EUR", "HUF"), 370.0*(1 + exchangePremium),
    ```
    Az src\main\resources\application.yml fájlban látható, hogy jelenleg 1%-os az átváltási jutalék:

    ```
    currency:
        exchangePremium: 0.01
    ```
    

- flights-api:
  
  - A FlightsApi interfész Spring MVC annotációkkal definiál egy végpontot, amelyen járatokat lehet keresni a felszálló és leszálló város megadásával. A válaszban Airline listát ad a végpont, egy Airline-nak id-je, kiinduló- és célvárosa, ára és pénzneme van. Az Airline osztály értelmeszerűen szintén az api projektben van definiálva. 
- flights:
  
  - A FlightsController valósítja meg a FlightsApi-t, az AirlineService segítségével. Ez az egyszerűség kedvéért lineáris keresést végez egy memóriába beégetett Airline listában.

A hetedik projekt neve *booking*. Ez nem a légitársaság, hanem egy utazási iroda rendszere, amely a légitársaság fenti szolgáltatásait felhasználva biztosít repjegy vásárló funkcionalitást. A BookingController-ben látható, hogy egy TicketData-t fogad el POST törzsként, majd erre egy PurchaseData választ ad. A pontos elvárt működést majd a 4. feladat specifikálja, de a megvalósítás során mindhárom légitársasági rendszert meg kell majd hívni.

## Feladatok

Összesen 4 feladat van. [Itt kezdd](Feladat-1.md) az első feladattal.