# Twitter szerver alkalmazás

Ez az alkalmazás egy REST API-t publikáló alkalmazás, amely elindítása után a 3000-es porton figyel és várja a megfelelő HTTP kéréseket a következő végpontokon: 
* `GET http://localhost:3000/api/`: visszaküldi a következő JSON objektumot: `{}name: 'Twitter server'`
* `GET http://localhost:3000/api/tweets`: lekérdezi a szerveren eltárolt összes tweetet. 
* `POST http://localhost:3000/api/tweets`: elküld egy új tweetet. Az új tweet adatai a POST kérés törzsében kell utazzanak a következő JSON formátumban: 
    ```ts
    {
        "text" : string, // a tweet szövege
        "userName": string, // a küldő felhasználó neve
        "tags"?: string[] // a tweet címkéi
    }
    ```
* `GET http://localhost:3000/api/search`: keres a szerveren eltárolt tweetek között. A keresés paramétereit az URLben a `text` és `tags` paraméterekkel lehet megadni. A `tags` paraméter a lehetséges értékeket ','-vel elválasztva kell leírnia. Például:
    * `GET http://localhost:3000/api/search?text=alma`: Visszaadja azokat a tweeteket, amelyek szövege tartalmazza az 'alma' szót. 
    * `GET http://localhost:3000/api/search?tags=a,b&text=alma`: visszaadja azokat a tweeteket, amelyek szövege tartalmazza az 'alma' szót és amelyek  tartalmazzák az 'a', vagy 'b' címkéket. 
* `GET http://localhost:3000/api/tweets/<ID>`: Visszaad egy adott azonosítójú tweetet. 
* `DELETE http://localhost:3000/api/tweets/<IDS_LIST>`: Kitörli az adott azonostójú tweeteket. Az ID-kat vesszővel elválasztva kell leírni

## Az almalmazás futtatása

1. Telepítsük fel a `package.json` fájl alapján a szükséges segédkönyvtárakat. 
    ```cmd
    $ npm install 
    ```
1. Fordítsuk le és indítsuk el az alkalmazást: 
    ```cmd
    $ npm run build-and-start
    ```

