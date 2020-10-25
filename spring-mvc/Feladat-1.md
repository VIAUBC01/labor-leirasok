# 1. Feladat

Az alkalmazás keretében egy ingatlanhirdetési portál néhány alapvető funkcióját kell megvalósítani. Az első feladat több részből áll:

- Hozz létre egy maven alapú Spring Boot-os projektet, amelyben függőségként szerepel a spring-mvc, a spring-data-jpa, és a h2 vagy hsqldb beágyazott adatbáziskezelő!
- Hozz létre egy entitást, amely képes az ingatlanok alábbi adatainak tárolására:
  - automatikusan generált azonosító
  - a hirdetés rövid, figyelemfelkeltő címe
  - a hirdetés részletes szövege
  - a létrehozás időbélyege
  - az ingatlan települése
  - az ingatlan típusa (lakás, ház, telek)
  - irányár
  - telefonszám
- Valósíts meg egy REST végpontot, amely GET kérés esetén az URI-ben utazó azonosítóhoz visszaadja az ingatlan adatait, kivéve a telefonszámot! (Képzeljük el, hogy a telefonszámot a GUI-n majd egy külön gombbal kell lekérni, ennek megnyomása egy komolyabb érdeklődést feltételez.)
- Valósíts meg egy REST végpontot, amely GET kérés esetén az URI-ben utazó azonosítóhoz visszaadja hirdetéshez tartozó telefonszámot!
- Valósíts meg egy REST végpontot, amely egy JSON formátumban POST-olt ingatlant tud lementeni, a válaszban visszaadva azt az URL-t, amin keresztül lekérhető az újonnan létrehozott ingatlan (természetesen a telefonszám kivételével) 

## Következő feladat

Folytasd a [következő feladattal](Feladat-2.md).
