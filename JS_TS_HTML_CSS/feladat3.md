# További feladatok

## Új tweet elküldése

Egészítse ki az webalkalmazást úgy, hogy új tweetet is lehessen küldeni! A felhasználó egy szövegdobozba beírhatja a
nevét, egy másikba a szöveget, egy harmadikba pedig vesszővel elválasztva a tageket. Egy gomb megnyomására küldjük el az
üzenetet.

Segítség a megvalósításhoz:

* A gomb megnyomására egy JavaScript-függvényt kell meghívni, amely egy HTTP `POST` kérést küld el. A `POST` kérésnek a
  törzse megfelelő formátumban (ahogyan azt az API várja) kell, hogy tartalmazza a tweet objektumot. Erre ugyanazt a
  `fetch()` függvényt használhatjuk, mint a `GET` esetén, csak máshogyan kell paraméterezni.

```ts
fetch('/tweets', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json' // ez közli a szerverrel, hogy JSON formátumú a HTTP POST törzse
    },
    body: JSON.stringify(tweet) // a tweet tartalmazza az elküldendő adatatot
}).then(/*... válasz feldolgozása...*/)
```

**Készítsen képernyőképet a felületről és másolja be a jegyzőkönyvbe a TypeScript-függvényt!**

## Tweet törlése

Legyen lehetőség tweetek törlésére:

1. Egészítse ki a backendet olyan végponttal, amely egy adott azonosítójú tweetet kitöröl az adatbázisból. Figyelem:
   legyen hibakezelés, ha nem létező azonosítót küldünk! A törlést mindenképpen HTTP `DELETE` kéréssel valósítsa meg. (
   Segítség: a törléshez a tweet azonosítója kell, ezt érdemes az URL-be beleírni.)
2. Készítsen a HTML-oldalra felületet, amin keresztül meghívható a törlés függvény! A HTML-táblázatot egészítsünk ki egy
   újabb oszloppal. Minden egyes sorban jelenítsünk meg egy _Törlés_ gombot. Még a gomb létrehozásakor feliratkozhatunk
   annak `click` eseményére, pl.:
    ```ts
    let button = document.createElement('button');
    button.addEventListener('click', () => {
        //törlés
    });
    ```
   Törlés után automatikusan frissítsük a listát!

**Készítsünk képernyőképet a frissített felületről a jegyzőkönyvbe! Ugyanide illesszük be az új vagy megváltozott
TypeScript-kódokat!**

## Bootstrap-téma

A felület kinézete nagyon fapados, használjuk a [Bootstrap CSS-könyvtárt](https://getbootstrap.com/), hogy szebben
nézzen ki:

* Érdemes a [BootstrapCDN](https://www.bootstrapcdn.com/)-ről hivatkozni a CSS-fájlt, így azt nem kell külön letölteni.
  Például ezt lehet beilleszteni a HTML `<head>` elemének a belsejébe:
    ```html 
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css"
            integrity="sha384-JcKb8q3iqJ61gNV9KGb8thSsNjpSL0n8PARn9HuZOnIxN0hoP+VmmDGMN5t9UJ0Z" crossorigin="anonymous">
    ```
* Alkalmazzunk néhány egyszerű stílust:
    * Adjunk [konténer `<div>`-eket](https://getbootstrap.com/docs/5.3/layout/containers/) az oldalhoz!
    * Lássuk el a gombokat a [megfelelő CSS-osztályokkal](https://getbootstrap.com/docs/5.3/components/buttons/)!
    * Használjunk [Bootstrap-stílust](https://getbootstrap.com/docs/5.3/content/tables/) a táblázatban!

A kész felületről kerüljön képernyőkép a jegyzőkönyvbe!

_Végeztél a laborral! 🎉_
