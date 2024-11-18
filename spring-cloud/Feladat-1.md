# 1. feladat: Config server

Indítsd el a `CurrencyApplication`-t, majd a `BonusApplication`-t (Pl. jobb klikk > _Run as..._ > _Spring Boot App_).
Azt tapasztalod, hogy az egyik nem fog elindulni, mert mindkettő a default `8080`-as portot szeretné használni, de a
korábban elinduló már elfoglalta.

Az egy gépen történő futtatáshoz mindenképp külön portot kell beállítani az alkalmazásoknak. A `booking` projektben lévő
`src\main\application.proprerties`-ben látható is, hogy `server.port=9080`, így ez nem akadna össze a többivel.

De a légitársaság a belső szolgáltatásait nem az egyes alkalmazások belső konfigfájljaiban akarja konfigurálni, hanem
egy központosított helyen, ezért egy konfigszervert fog bevezetni. A konfigszerver egy külön alkalmazás lesz (a neve pl.
`config`), amelyeknek a `bonus`, `currency` és `flights` alkalmazások a kliensei lesznek, vagyis ettől fogják lekérdezni
induláskor a saját konfigurációjukat. A Spring Cloud Config projekt beépítve tartalmaz támogatást a szerver- és a
kliensoldalhoz is, amit az alábbi módon vehetünk igénybe:

1. Hozz létre egy új projektet `config` néven. Ha STS-t használsz, a leggyorsabb ezt a _File_ > _New_ > _Spring Starter
   Project_ menüből indítani. 17-es Javát és Mavent válassz a varázsló első oldalán, majd a második oldalon a
   választható függőségek közül a Spring Cloud Config csoport alatt a Config Servert.

2. Vizsgáld meg a keletkező `pom.xml`-t. Látható, hogy a parent projekt ugyanúgy a `spring-boot(-starter)-parent`, ahogy
   azt megszoktuk. Viszont létrejött egy `dependencyManagement` tag, amely a spring cloud-os függőségeket húzza be. (Az
   aktuális Spring Cloud verzió a 2023.0.3, ami propertybe van kiszervezve):

   ```xml
   <dependencyManagement>
    <dependencies>
        <dependency>
            <groupId>org.springframework.cloud</groupId>
            <artifactId>spring-cloud-dependencies</artifactId>
            <version>${spring-cloud.version}</version>
            <type>pom</type>
            <scope>import</scope>
        </dependency>
    </dependencies>
   </dependencyManagement>
   ```

   A `dependencyManagement` tag nem összekeverendő a `dependencies` taggel! Ha egy konkrét Spring cloud-os függőséget
   ténylegesen használni akarunk, akkor azt a `dependencies` tagbe kell tenni, persze a verziót elhagyhatjuk, mert azt
   megkapjuk a `dependencyManagement` tagből:

   ```xml
   <dependencies>
   		<dependency>
   			<groupId>org.springframework.cloud</groupId>
   			<artifactId>spring-cloud-config-server</artifactId>
   		</dependency>
   		...
   </dependencies>
   ```


3. A varázsló létrehozott egy `application.properties` fájlt, de mi YAML formátumban konfiguráljuk az alkalmazásunkat,
   ezért hozzunk létre egy `application.yaml` fájlt az `src\main\resources` alatt. Ebbe az alábbi tartalom kerüljön:

   ```yaml
   server:
     port: 8081
   spring:
     application: 
       name: config
     profiles:
       active: native  
     cloud:
       config:
         server:
           native:
             search-locations: classpath:/config
   ```

   A konfig eleje értelemszerűen a config szerver portját állítja `8081`-re. Az alkalmazás neve az lesz, hogy `config`,
   majd bekapcsoljuk a `native` profile-t. Ennek hatására a config szerver nem egy Git repóból próbálja majd olvasni a
   konfigfájlokat (ami amúgy valós környezetben hasznos lehetne), hanem lokális fájlrendszerből. Hogy pontosan honnan,
   azt a következő konfig adja meg: a `classpath` `config` könyvtárából.

4. Hozzuk létre az `src\main\resources` alatt a `config` könyvtárat, abban pedig 3 YAML fájlt: `bonus.yaml`,
   `flights.yaml`, `currency.yaml`. Ezekben konfigoljunk nem ütköző portokat a három alkalmazásnak:
   ```yaml
   server:
     port: 8083
   ```

    - A `currency.yaml`-ben pedig ezen felül még az átváltási jutalékot is állítsuk 1,5%-ra:

      ```yaml
      currency:
          exchangePremium: 0.015
      ```

5. A `config` projekt main osztályára (`ConfigApplication`) tegyük rá a `@EnableConfigServer` annotációt, ezzel a
   konfigszerver elkészült.

6. Most a három légitársasági alkalmazást állítjsk be, hogy a konfigszervert konfigkliensként használják. Az alábbi
   lépéseket mindháromra (`bonus`, `currency`, `flights`) végezzük el.

    - A `pom.xml`-ben:

        - A `<properties>` tagbe helyezzük el ezt a sort:

      ```xml
              <spring-cloud.version>2023.0.3</spring-cloud.version>
      ```

        - A `<properties>` tag alá helyezzük el ezeket a sorokat:

      ```xml
          <dependencyManagement>
              <dependencies>
                  <dependency>
                      <groupId>org.springframework.cloud</groupId>
                      <artifactId>spring-cloud-dependencies</artifactId>
                      <version>${spring-cloud.version}</version>
                      <type>pom</type>
                      <scope>import</scope>
                  </dependency>
              </dependencies>
          </dependencyManagement>
      ```

        - A `spring-boot-starter-web` függőség után illesszük be ezeket a sorokat:

          ```xml
                  <dependency>
                      <groupId>org.springframework.cloud</groupId>
                      <artifactId>spring-cloud-starter-config</artifactId>
                  </dependency>
          ```

    - Adjuk hozzá az `application.properties` fájlhoz (`src\main\resources`) ezeket (a `name` utáni érték értelemszerűen
      az adott projektnek megfelelő legyen):

      ```
      spring.application.name=bonus
      spring.config.import=optional:configserver:http://localhost:8081
      ```

        - A `currency` projektben `application.yml` fájlunk van, amiben az átváltási jutalékot definiáljuk. Egészítsük
          ki ezzel:
          ```yaml  
          currency:
             exchangePremium: 0.01
          spring:
             application:
               name: currency
             config:
               import: optional:configserver:http://localhost:8081
          ```
            - Az `optional:` prefix azt szolgálja, hogy az alkalmazásunk akkor is el tudjon indulni, ha a konfigszerver
              nem elérhető. Ha azonban ez előfordulna, visszajutnánk a kezdeti problémához: a default `8080`-as porton
              akarna elindulni minden alkalmazás. Tehát a helyes működéshez a konfigszervert kell majd először
              elindítani.
            - Ha nem tudjuk vagy akarjuk biztosítani, hogy a konfigszerver induljon el először, az alkalmazásokat
              beállíthatjuk úgy is, hogy induláskor többször próbálkozzanak a konfigszerver elérésével, erről itt
              olvashatóak
              részletek: https://docs.spring.io/spring-cloud-config/docs/current/reference/html/#config-client-retry
            - A Spring Boot 2.4 vezette be a `spring.config.import` propertyt, amivel a fenti módon, egyszerűen lehet
              máshonnan (jelen esetben egy konfigszerverről) konfigurációt importálni. Korábbi verziókban egy
              `bootstrap.yml` fájlra volt szükség, ott kellett megadni az alkalmazás nevét és a konfigszerver
              elérhetőségét (a `spring.cloud.config.uri` propertyben). Ha a régi módon, a `bootstrap.yml`-t akarnánk
              használni, a `spring.cloud.bootstrap.enabled=true` propertyvel, vagy a `spring-cloud-starter-bootstrap`
              függőség behúzásával tudnánk megtenni.

7. Indítsd el a `ConfigApplication`-t, majd sorban a `BonusApplication`-t, `CurrencyApplication`-t és
   `FlightsApplication`-t! Jól használható erre a célra a bal alsó sarokban lévő Boot Dashboard nézet, ahol gyorsan
   kiválasztható bármelyik alkalmazás, ami után egyszerű vagy debug módú futtatás kezdeményezhető a megfelelő ikonnal. (
   Ha már fut az alkalmazás, akkor először le is állítja azt, így nem kell portütközéstől tartani. A `*-api` projekteket
   értelemszerűen nem lehet elindítani, mert nincs bennük main osztály.)

   ![Boot Dashboard](images/boot-dashboard.png)

8. Készíts két képernyőképet a jegyzőkönyvbe:

    1. Látszódjon a `currency.yaml` megnyitva az IDE-ben, és mellette egy böngészőben az átváltó API meghívásának
       eredménye, USD -> HUF irány esetére
    2. Látszódjon a `bonus.yaml` megnyitva az IDE-ben, és mellette egy böngészőben a saját Neptun-kódodnak megfelelelő
       user bonusának lekérdezése

## Következő feladat

Folytasd a [következő feladattal](Feladat-2.md).
