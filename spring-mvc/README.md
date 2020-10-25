# Adatréteg és REST API fejlesztése Spring Boottal

A labor során egy adatréteget és egy hozzá kapcsolódó REST API-t fogunk megvalósítani Spring Boot platformon. Az adatmodellt JPA entitásokkal valósítjuk meg, amelyek kényelmes használatához a Spring Data JPA által nyújtott lehetőségeket használjuk fel. Néhány adatrétegen keresztül elérhető funkciót REST API formájában publikálunk, a Spring MVC-re támaszkodva. 

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- JDK 11, pl. innen: https://adoptopenjdk.net/
- Tetszőleges Java alapú IDE, pl. Spring Tools 4 for Eclipse: https://spring.io/tools
- HTTP kérések egyszerű összeállítását lehetővé tevő fejlesztői eszköz, pl.: [Postman](https://www.postman.com/downloads/)

<hr />

A laborok elvégzéséhez használható segédanyagok és felkészülési anyagok:
- A Háttéralkalmazások tárgy kapcsolódó előadásai:
  - https://www.aut.bme.hu/Upload/Course/VIAUBB04/hallgatoi_jegyzetek/06-JPA.pdf
    - https://www.loom.com/share/f08dfb97a5fc4aaaa9564c20447ef72c
    - https://www.loom.com/share/e0b5742dbbef4611b0c46caa26ce1e5b
    - https://www.loom.com/share/3ceeec0dc71e4ab38e1a7de8c9786905
    - https://www.loom.com/share/e34e5e5b5be746499996f58705d9eefc
    - https://www.loom.com/share/dd70e1aee45d495fa2c77d1584433166
  - https://www.aut.bme.hu/Upload/Course/VIAUBB04/hallgatoi_jegyzetek/07A-Spring.pdf
    - https://web.microsoftstream.com/video/e320f3f1-29f9-454b-8121-0cb9d1182d84
    - https://web.microsoftstream.com/video/fa67fe60-3d78-45cd-a89a-0696198ca845
- A Háttéralkalmazások tárgy kapcsolódó gyakorlatai:
  - https://github.com/BMEVIAUBB04/gyakorlat-jpa
    - https://web.microsoftstream.com/video/c861adb9-6889-4207-92a9-2814f8cbe75a
    - https://web.microsoftstream.com/video/06b661da-6525-4297-96e5-8941844d31e6
  - https://bmeviaubb04.github.io/gyakorlat-rest-spring-mvc/
    - https://www.youtube.com/watch?v=ahNrOHgAvqo&feature=youtu.be
    - https://github.com/BMEVIAUBB04/gyakorlat-rest-spring-mvc

Hivatalos dokumentációk:

- Spring Data JPA: https://docs.spring.io/spring-data/jpa/docs/current/reference/html/#reference
- Spring MVC: https://docs.spring.io/spring-boot/docs/current/reference/htmlsingle/#boot-features-spring-mvc
- Spring Boot: https://docs.spring.io/spring-boot/docs/current/reference/htmlsingle/

Egy hasznos tutorial:

- https://www.baeldung.com/spring-boot-start

  

## Beadandó

A labor elvégzése után az alábbi tartalmat kérjük beadni a tanszéki portálra történő feltöltéssel, egy zipelt fájlban:
- **PDF** formátumban (DOCX nem elfogadott!) az egyes feladatoknál megnevezett: 
  - konkrét kódrészletekről készített képernyőkép(ek), 
  - 1 mondatos magyarázat
  - 1 vagy több ábra (jellemzően képernyőkép), ami a helyes működést hivatott bizonyítani. A helyes működést igazoló képernyőképen szerepeljen a Neptun kódod. (pl. beszúrt példaadatban, az API eredményét mutató ablakban, konzol kimeneten, a böngészőben megjelenő adatok között stb.; **NEM** külön Jegyzettömb ablakban)!
- A projekt forrását (src\main mappa elegendő)

## Értékelés

A laborban négy feladatrész van. Jeles osztályzat az összes feladatrész elvégzésével kapható. Minden hiányzó, avagy hiányos feladatrész mínusz egy jegy.

## Feladatok

Összesen 4 feladat van. [Itt kezdd](Feladat-1.md) az első feladattal.
