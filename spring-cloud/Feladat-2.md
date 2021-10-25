# 2. Feladat: Discovery server

Egy microservice architektúrában célszerű egy discovery szerveren nyilvántartani, hogy melyik szolgáltatás milyen címen, vagy ha több példányban futtatjuk őket, akkor milyen cím**ek**en elérhető. Így pl. a kliensekbe nem kell beégetni a szolgáltatások címeit, és ha valamelyik szolgáltatást áthelyezik másik szerverre, a kliensek automatikusan megtalálják az új címet. Vagy ha címen elérhető egy szolgáltatás, akár terheléselosztó logikát is tehetünk a discovery mechanizmusba. Most ezt fogjuk megvalósítani, szintén Spring Cloud támogatással.

1. Hozzuk létre a discovery szerver projektjét, Ismét célszerű a File > New > Spring Starter Project varázslót használni. A projekt neve legyen *discovery*. A következő oldalon a lehetséges függőségek közül válasszuk a Spring Cloud Discovery csoportból az Eureka Server-t és a Spring Cloud Config csoportból a Config Client-et. (Maga a discovery szerver is a config szerverről fogja venni a saját konfigját.)

2. A konfig szerver beállításához hozzuk létre az src\main\resources alá a application.properties fájlt, a szokásos tartalommal:

   ```
   spring.application.name=discovery
   spring.config.import=optional:configserver:http://localhost:8081
   ```
   
3. A main osztályra (DiscoveryApplication) tegyük rá az *@EnableEurekaServer* annotációt.

4. A config projektben lévő src\main\resources\config könyvtár alá hozzuk létre a neki megfelelő konfig fájlt discovery.yml néven, az alábbi tartalommal:

   ```
   server:
     port: 8085
   eureka:
     instance:
       hostname: localhost
     client:
       registerWithEureka: false
       fetchRegistry: false
       serviceUrl:
         defaultZone: http://${eureka.instance.hostname}:${server.port}/eureka/
   ```

   Ha a 8085-ös portot már elhasználtad valamelyik korábbi alkalmazásra, akkor értelemszerűen itt mást kell megadnod. A Spring Cloud több discovery megoldást is támogat, mi most az Eureka-t használjuk, amely egy Netflix fejlesztés springes adoptációja. Az Eureka a beregisztrált szolgáltatásokat nem DB-ben tárolja, hanem memóriában, de a hibatűrés érdekében az Eureka szerver példányok egymás közt replikálják az adataikat. Ezen kívül az Eureka kliensek szintén cache-elik a kereséseik eredményét, így jó hibatűrés jellemzi a megoldást. Mi most egyetlen eureka szervert fogunk futtatni, így a konfigban azt állítottuk be, hogy ne próbáljon más Eureka szerverekkel kommunikálni.

5. Azt szeretnénk, hogy a többi induló alkalmazás (*flights, bonus, currency*, és a *config* is!) a discovery szerverünknél regisztrálja be magát induláskor. Ennek érdekében:

   - Mindegyik pom.xml-jében helyezd el ezt a függőséget:

     ```
             <dependency>
                 <groupId>org.springframework.cloud</groupId>
                 <artifactId>spring-cloud-starter-netflix-eureka-client</artifactId>
             </dependency>
     ```

   - A config projektben lévő currency.yml, bonus.yml és flights.yml-be, valamint magát a config szervert konfigoló application.yml-be tedd bele ezeket a sorokat:

     ```
     eureka:
       client:
         serviceUrl:
           defaultZone: http://localhost:8085/eureka/
     ```

6. Indítsd el sorban a config, discovery, flights, currency és bonus alkalmazásokat. Készíts egy screenshotot, amelyen egy böngészőben látszik megnyitva a http://localhost:8085/ URL. Ha más portot állítottál be a discovery szervernek, akkor értelemszerűen azt a portot nyisd meg. Az elvárt eredmény, hogy a regisztrált példányok közt látszodjon a config, a flights, a currency és a bonus alkalmazás is.

## Következő feladat

Folytasd a [következő feladattal](Feladat-3.md).
