# Adatréteg és REST API fejlesztése Spring Boottal

A labor során egy adatréteget és egy hozzá kapcsolódó REST API-t fogunk megvalósítani Spring Boot platformon. Az
adatmodellt JPA-entitásokkal valósítjuk meg, amelyek kényelmes használatához a Spring Data JPA által nyújtott
lehetőségeket használjuk fel. Néhány adatrétegen keresztül elérhető funkciót REST API formájában publikálunk, a Spring
MVC-re támaszkodva.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- JDK 17, pl. innen: https://adoptium.net/
- Tetszőleges Java alapú IDE, pl. Spring Tools 4 for Eclipse: https://spring.io/tools
- HTTP-kérések egyszerű összeállítását lehetővé tevő fejlesztői eszköz,
  pl.: [Postman](https://www.postman.com/downloads/)

<hr />

A laborok elvégzéséhez használható segédanyagok és felkészülési anyagok:

- A Háttéralkalmazások tárgy kapcsolódó előadásai:
    - https://www.aut.bme.hu/Upload/Course/VIAUBB04/hallgatoi_jegyzetek/06-JPA.pdf
        - https://web.microsoftstream.com/video/69badb22-08d5-4b2c-9a7f-25c70d137612
    - https://www.aut.bme.hu/Upload/Course/VIAUBB04/hallgatoi_jegyzetek/07A-Spring.pdf
        - https://web.microsoftstream.com/video/0c3e6a60-3b89-4e6e-b605-2eeacb9f2e5e
- A Háttéralkalmazások tárgy kapcsolódó gyakorlatai:
    - https://github.com/BMEVIAUBB04/gyakorlat-jpa
        - https://web.microsoftstream.com/video/dc68cfd1-837f-41b2-b0c8-e437ccb50410
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

- **PDF** formátumban (`.docx` nem elfogadott!) az egyes feladatoknál megnevezett:
    - konkrét kódrészletekről készített képernyőkép(ek),
    - 1 mondatos magyarázat
    - 1 vagy több ábra (jellemzően képernyőkép), ami a helyes működést hivatott bizonyítani. A helyes működést igazoló
      képernyőképen szerepeljen a Neptun-kódod. (pl. beszúrt példaadatban, az API eredményét mutató ablakban, konzol
      kimeneten, a böngészőben megjelenő adatok között stb.; **NEM** külön Jegyzettömb-ablakban)!
- A projekt forrását (az `src\main` mappa elegendő)

## Értékelés

A laborban négy feladatrész van. Jeles osztályzat az összes feladatrész elvégzésével kapható. Minden hiányzó, avagy
hiányos feladatrész mínusz egy jegy.

## Feladatok

Összesen 4 feladat van. [Itt kezdd](Feladat-1.md) az elsővel.
