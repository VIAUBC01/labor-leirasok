# 1. feladat

Az alkalmazás keretében egy ingatlanhirdetési portál néhány alapvető funkcióját kell megvalósítani. Az első feladat több
részből áll:

- [Hozz létre](https://start.spring.io) egy Maven alapú Spring Boot-projektet, amelyben függőségként szerepel a Spring
  Web (`org.springframework:spring-webmvc`), a Spring Data JPA (`org.springframework.data:spring-data-jpa`), és a H2
  Database (`com.h2database:h2`) vagy a Hyper SQL Database (`org.hsqldb:hsqldb`) beágyazott adatbáziskezelő!
- Hozz létre egy entitást, amely képes az ingatlanok alábbi adatainak tárolására:
    - automatikusan generált azonosító
    - a hirdetés rövid, figyelemfelkeltő címe
    - a hirdetés részletes szövege
    - a létrehozás időbélyege
    - az ingatlan települése
    - az ingatlan típusa (lakás, ház, telek)
    - irányár
    - telefonszám
- Valósíts meg egy REST végpontot, amely egy JSON formátumban POST-olt ingatlant tud lementeni, a válaszban visszaadva
  azt az URL-t, amin keresztül lekérhető az újonnan létrehozott ingatlan (természetesen a telefonszám kivételével)!
- Valósíts meg egy REST végpontot, amely GET kérés esetén az URI-ben utazó azonosítóhoz visszaadja hirdetéshez tartozó
  telefonszámot!
- Valósíts meg egy REST végpontot, amely GET kérés esetén az URI-ben utazó azonosítóhoz visszaadja az ingatlan adatait,
  kivéve a telefonszámot! (Képzeljük el, hogy a telefonszámot a GUI-n majd egy külön gombbal kell lekérni, ennek
  megnyomása egy komolyabb érdeklődést feltételez.)

## Következő feladat

Folytasd a [következő feladattal](Feladat-2.md).
