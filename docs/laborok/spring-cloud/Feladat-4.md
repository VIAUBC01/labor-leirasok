# 4. Feladat: OpenFeign

Most, hogy a légitársaság microservice architektúrája készen áll, valósítsuk meg az utazási iroda booking alkalamzásában szereplő repjegy vásárló végpontot. Ezt a BookingController.buyTicket metódusban kell megvalósítani, az alábbi szabályok szerint (minden végpontot az API gateway-en keresztül kell meghívni):

1. A TicketData-ban szereplő from és to mező alapján végezz egy keresést a flights API meghívásával. Ha nincs találat, egyből visszaadandó egy olyan PurchaseData, amiben a success értéke false.
2. Ha van találat, akkor a legolcsóbbal dolgozz tovább. Tegyük fel, hogy a vásárlást USD-ben fogják kifizetni, ezért minden találat árát váltsd át USD-re a currency API meghívásával, hacsak nem eleve USD-ben kaptad meg a fligts API-tól.
3. Ha a TicketData-ban a useBonus true-ra van állítva, kérdezzük le a user (szintén ott van a TicketData-ban) jelenlegi bónusz pontjait a bonus API meghívásával. Ha van bónusz pontja, azokat felhasználjuk a vásárlásnál 1 USD = 1 bónusz pont értékben vagyis:
   - Számítsuk ki mennyi bónuszpontot használhatunk fel (a fizetendő végső ár nem lehet negatív, akármennyi bónuszunk is van). Ezt az bónusz pontszámot vonjuk is le a bonus API-n keresztül a usertől, majd ezt állítsuk be a válasz purchaseDate bonusUsed mezőjébe.
   - A felhasznált bónuszpontokkal csökkentett árat állítsuk be a válasz purhcaseData price mezőjébe.
4. A ténylegesen fizetendő árnak megfelelő bonus pontszámot (az ár szorozva konfigból beolvasott bonusRate értékkel) írjuk jóva a usernek a bonus API-n keresztül, majd ezt az értéket állítsuk be a válasz purchaseDate bonusEarned mezőjébe.
5. Ha a fenti lépések hiba nélkül végigfutottak, legyen a válasz purchaseData success mezője true, ellenkező esetben false.

A fenti logika lekódolása önálló feladat, előtte viszont közösen oldjuk meg azt a problémáat, hogy a BookingController-ből meg tudjuk hívni a gateway-en keresztül elérhető API-kat, a bonus példáján keresztül, értelemszerűen ezeket a lépéseket kell majd elvégezni a másik két projektnél is. Több módon meg lehet hívni Java-ból REST API-t:

- A java.net.URLConnection osztályon keresztül (nagyon alacsony szintű megoldás)

- A Spring RestTemplate-jén keresztül: https://www.baeldung.com/rest-template

- A szintén Spring által nyújtott, kicsit szebb API-t biztosító WebClient-en keresztül: https://www.baeldung.com/spring-5-webclient

- Mi azt fogjuk kihasználni, hogy az API-nk definíciója már rendelkezésre áll Spring MVC annotációkkal elllátott interfész formájában. (Az *-api projektekben.) Ilyen esetben a [Spring Cloud OpenFeign](https://docs.spring.io/spring-cloud-openfeign/docs/2.2.5.RELEASE/reference/html/) futási időben tudja generálni az API-t meghívó klienst, vagyis az API kliens oldali meghívása úgy fog megjelenni, mint egy egyszerű metódushívás ezen az interfészen. Az alábbi lépések szükségesek ehhez:

  - A spring-cloud-starter-openfeign függőség már készen áll a bonus-api pom.xml-jében

  - Helyezd el az alábbi annotációt a BonusApi interfészen, amivel azt fejezzük ki, hogy a bonus nevű Feign API kliens majd a feign.bonus.url uri-n keresztül lesz elérhető

    ```
    @FeignClient(name = "bonus", url = "${feign.bonus.url}")
    ```

  - A booking pom.xml-jébe helyezd el függőségként a bonus-api-t (ez tranzitív módon magával hozza az openfeign startert is):

    ```
            <dependency>
              <groupId>hu.bme.aut.szoftlab</groupId>
              <artifactId>bonus-api</artifactId>
              <version>0.0.1-SNAPSHOT</version>
            </dependency>
    ```

  - A BookingApplication osztályon helyezd el ezt az annotációt:

    ```
    @EnableFeignClients(basePackageClasses = {BonusApi.class})
    ```

  - A booking projekt src\main\resources\application.properties fájlba tedd be ezt a sort:

    ```
    feign.bonus.url=http://localhost:8080/bonus
    ```

  - A Feign által generált API kliens injektálható pl. a BookingControllerbe:

    ```
    @Autowired
    BonusApi bonusApi;
    ```

Miután a másik két API klienst is bekötötted, és segítségükkel megvalósítottad a fent definiált üzleti logikát, indítsd el az összes alkalmazást: config, discovery, gateway, flights, bonus, currency, booking. Postman-en keresztül végezz el három hívást, amelyekről készíts screenshotot:

1. Jegyvásárlás Budapestről Prágába, user a saját neptun kódod, bónusz felhasználásával (frissen indítsd a bonus alkalmazást, hogy 0 bónusza legyen a userednek.)
2. Jegyvásárlás Budapestről Prágába, user a saját neptun kódod, bónusz felhasználása nélkül
3. Jegyvásárlás Budapestről Prágába, user a saját neptun kódod, bónusz felhasználásával



## Végeztél

Végeztél a feladatokkal.
