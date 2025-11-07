# Szerver alkalmazás

## Könyvtárak létrehozása

Egy üres könyvtárban hozzunk létre egy `client` és egy `server` könyvtárat.

## Szerver projektek előkészítése

Lépjünk be a szerver könyvtárba (mostantól végig ebben dolgozunk) és hozzunk létre egy NodeJS projektet a következő parancs kiadásával: 

```cmd
$ npm init
```

Ez létrehoz egy `package.json` fájlt, amiben a projektünk függőségeit (más programkönyvtárakat (= `npm` package)) leírjuk. 

Ellenőrizzük, hogy a TypeScript fordító telepítve van a gépünkre! Ezt érdemes a `-g` kapcsolóval telepíteni, így nem csak az adott program számára lesz elérhető, hanem globálisan az egész számítógépen. 

```cmd
$ npm install typescript -g
```

Mivel TypeScript nyelven szeretnénk programozni, ezért létrehozunk egy TypeScript projektet is, amellyel azt konfiguráljuk, hogy ha a könyvtárban kiadjuk a `tsc` (TypeScript Compiler) parancsot, akkor mely fájlokat és hogyan fordítsuk le. 

```cmd
$ tsc --init
```

A generált `tsconfig.json` fájl tartalmazza a TypeScript projekt és fordító beállításait. Írjuk át az alapértelmezett beállításokat az alábbiaknak megfelelően: 

```json
{
  "compilerOptions": {
    "target": "es6",                          
    "module": "commonjs",                     
    "outDir": "./build/",                     
    "strict": true,                           
    "esModuleInterop": true,                  
    "skipLibCheck": true,                     
    "forceConsistentCasingInFileNames": true  
  }
}
```

Amin változtattunk:
* `outDir`: ebbe a könyvtárba fogja generálni a TypeScript fordító (`tsc`) a JavaScript fájlokat. 
* `strict`: "szigorú" mód, ilyenkor több hibára hívja fel a figyelmet a fordító

## REST API programozása

A feladat során egy REST APIt szeretnénk megvalósítani. Vagyis az a cél, hogy a szerveralkalmazásunkra érkező HTTP kérésekre megfelelő választ tudjunk adni. Ehhez az `Express` nevű keretrendszert fogjuk használni. Ezt betöltve csak fel kell konfigurálnunk, azaz megadnunk, hogy milyen végpontokat (urleket) kell figyeljen és az egyes végpontokra érkező HTTP kérésekre milyen HTTP választ akarunk küldeni. 

Adjuk hozzá az `express` csomagot a projektünkhöz!

```cmd
$ npm install express -s
```

Mivel TypeScript nyelven dolgozunk, adjuk hozzá a `@types/express` csomagot is. Ez TypeScript nyelvű típusannotációkat tartalmaz az eredetileg JavaScript nyelven megírt `express` könyvtárhoz, így a fordító ellenőrizni tudja, hogy megfelelően használjuk-e a könyvtárat. 

```cmd 
$ npm install @types/express -s
```

Hozzunk létre egy `app` könyvtárat, abban pedig egy `main.ts` fájlt, amiben helyezzük el a következő kódot:

```ts
console.log('hello world!');
```
## Fordítás, futtatás

A TypeScript fájlokat a `tsc` paranccsal tudjuk lefordítani. 

```cmd
$ tsc
```

Ennek eredménye egy `main.js` fájl lesz a `build` mappában. Ezt az egyszerű js fájlt a `node` segítségével futtathatjuk:

```cmd
$ node .\build\main.js
```

*Linuxon és macOS-en a backslash helyett forward slash-t kell használni: node ./build/main.js*

Ha mindent jól csináltunk, a konzolon megjelenik a "hello world!" szöveg.

A `node` projektekben lehetőség van szkriptek definiálására. Írjuk be a következő részeket a `package.json` fájlba:

```json
/*...*/
"scripts": {
    "build" : "tsc",
    "start" : "node ./build/main.js",
    "build-and-start": "tsc && node ./build/main.js"
},
/*...*/
```

Ha ki szeretnénk listázni, hogy milyen szkriptek érhetők el az adott projekthez, akkor futtassuk az `npm run` parancsot, ha pedig egy konkrét szkriptet szeretnénk futtatni, adjuk ki az `npm run <szkript neve>` parancsot. 

A fenti szkriptek szerepe a következő: 
* `build`: lefordítja a TypeScript kódunkat (kiadja a `tsc` parancsot)
* `start`: elindítja a már lefordított alkalmazást
* `build-and-start`: lefordítja a kódot, majd elindítja a lefordított `main.js`-t. 

Innentől tehát, ha változik a kód, elég a következő parancsot kiadni az újrafordításhoz és futtatáshoz: 

```cmd
$ npm run build-and-start
```

## Twitter szerver logikájának megvalósítása

A következő komponenseket fogjuk megvalósítani:
* Típusok, amely leírják az egyes üzeneteket. Az üzeneteket a továbbiakban *tweet*eknek fogjuk nevezni.
* Egy adatbázis osztály, amely képes eltárolni a tweeteket, újat beszúrni, visszaadni a létező tweetek listáját. 
* Egy `express` alapú API, amely biztosítja a végpontokat a tweetek lekérdezéséhez és visszaadásához. 

### Modellek

Hozzuk létre az `app/models.ts` fájlt:

```ts
//app/models.ts
export type Tweet = {
    text : string,
    userName: string,
    tags?: string[]
}

export type TweetWithId = Tweet & {
    id?: string
}
```

Hozzuk létre az `app/database.ts` fájlt:

```ts
//app/database.ts
import { Tweet, TweetWithId } from "./models";

let i = 0;
function generateTweetId() : string {
    i++; 
    return i.toString();
}

export class Database {
    private tweetsByIds: Map<string, Tweet> = new Map();

    public addTweet(t: Tweet): string {
        let id = generateTweetId();
        this.tweetsByIds.set(id, t);
        return id;
    }

    public getTweetById(id: string): TweetWithId | null {
        if (!this.tweetsByIds.has(id))
            return null;
        return {
            ...this.tweetsByIds.get(id)!,
            id
        };
    }

    public getAllTweets() : TweetWithId[] {
        return Array.from(this.tweetsByIds.keys()).map(id => ({
            id,
            ...this.tweetsByIds.get(id)!
        }));
    }
}
```

Láthatjuk, hogy a tweeteket egyszerűen a memóriában fogjuk tárolni, tehát nincsen az alkalmazásunk mögött semmilyen perzisztens tároló, adatbázis. Ha a szervert újraindítjuk, az addig posztolt tweetek elvesznek. 

**Fontos, hogy a fenti kódokat ne csak bemásoljuk, hanem meg is értsük!** A jegyzőkönyvben válaszoljon az alábbi kérdésekre: 

1. JavaScriptben mire szolgál a `Map` típus? 
1. Mi a különbség a `type` és az osztály (`class`) között?
1. Magyarázza el, hogy az `app/database.ts` fájlban a `getTweetById` függvényben, a `return` utasítás utáni rész pontosan mit jelent, mi lesz a visszatérési érték!

Egészítse ki a `Database` osztály függvényeit loggolással (`console.log(...)`), amelyben a meghívott művelet neve mellett a következő információk kerüljenek kiírásra: 
* Új tweet esetén írjuk ki a tweet szövegét, beküldőjét, azonosítóját, tagjeit. 
* Tweetek lekérdezésénél írjuk ki, hány tweet található az adatbázisban. 
* Egy tweet lekérdezésénél írjuk ki a lekérdezett tweet azonosítóját. 

### Egyedi tweet azonosítók

Minden tweetnek lesz egy egyedi azonosítója. Ezeket képezhetjük valamilyen számláló karbantartásával (ahogyan azt a fenti kódban tettük), de ehelyett egyedi [GUID](https://hu.wikipedia.org/wiki/Glob%C3%A1lisan_egyedi_azonos%C3%ADt%C3%B3)okat fogunk generálni egy `uuid` nevű `npm package` segítségével:

```cmd
$ npm install uuid -s
$ npm install @types/uuid -s
```

Alakítsuk át a `database.ts`-t a következő módon: 

```ts
import { v4 as uuidv4 } from 'uuid';

function generateTweetId() : string {
    return uuidv4(); 
}
```

### Adatbázis tesztelése

Helyezzük el a következő részt a `main.ts` fájlban:

```ts
import { Database } from "./database";

let db = new Database();
db.addTweet({
    text: 'Hello World!',
    tags: ["init"],
    userName: "mark"
});
```

Ezután futtatva az alkalmazást meg kell jelenjen a konzolon a log üzenet az új tweetről. **Tegyünk erről egy képet a jegyzőkönyvbe!**

## Web-API

Az API programozásához az `express` csomag mellett szükség lesz annak egy pluginjára is ([`body-parser`](http://expressjs.com/en/resources/middleware/body-parser.html)), amely a bejövő HTTP kérések törzsét JSON objektummá alakítja:

```cmd
$ npm install body-parser -s
$ npm install @types/body-parser -s
```

Hozzuk létre az `app/web-api.ts` fájlt!

```ts
//app/web-api.ts
import { Database } from "./database";
import express from 'express';
import bodyParser  from 'body-parser';
import { Tweet } from "./models";

export class TwitterApi {

    constructor(private db: Database, private port: number) { }

    public startServer() {
        const app: express.Application = express();

        app.use(function (req, res, next) {
            res.header("Access-Control-Allow-Origin", "*");
            res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
            next();
        });
        app.use(bodyParser.json());

        this.configureRouting(app);

        app.listen(this.port, () => {
            console.log(`Twitter server listening on port ${this.port}!`);
        });
    }

    private configureRouting(app: express.Application) {
        app.get('/', (req, res) => {
            res.json({
                name: 'Twitter server'
            });
        });

        app.post('/tweets', (req, res) => {
            let tweet = req.body as Tweet;
            this.db.addTweet(tweet);
            res.sendStatus(204);
        });

        app.get('/tweets/:id', (req, res) => {
            let id = req.params.id;
            let tweet = this.db.getTweetById(id);
            if (!tweet) {
                res.sendStatus(404);
            } else {
                res.json(tweet);
            }
        });

        app.get('/tweets', (req, res) => {
            res.json(this.db.getAllTweets());
        });
    }
}
```

**A jegyzőkönyvben válaszoljon a következő kérdésekre:** 
* Milyen  végpontokat definiál a fenti kód. Egy táblázatban szerepeljen a végpont URL-je, a HTTP metódus, azt hogy várunk-e valamilyen paramétert és hogy milyen választ küld vissza a végpont. A válaszok státuszkódja is szerepeljen a leírásban. 
* Milyen porton fogja várni a bejövő kéréseket a szerver? Hogyan tudjuk ezt megadni a fenti kód szerint? 

Egészítsük ki a fenti függvényeket úgy, hogy egy-egy beérkező kérés esetén kikerüljön a logra, hogy milyen kérés érkezett!

## Szerver elindítása

Adjuk hozzá az alábbi kódrészletet a `main.ts` fájlhoz. Ezzel elindítjuk a szervert.

```ts
let api = new TwitterApi(db, 3000);
api.startServer();
```

## Tesztelés
A szerver alkalmazás alapvető funkcióit ezzel megvalósítottuk, most ezt szeretnénk tesztelni. Ha megnyitjuk a böngészőt és beírjuk, hogy `http://localhost:3000`, akkor a böngésző megjeleníti a "Twitter szerver" információt. Ezzel máris teszteltük az egyik végpontot. A többi végpont, például a létrehozás, teszteléséhez azonban már egy tweet adatait is el kellene küldeni . 

Egyszerű HTTP kérések összeállításához és elküldéséhez nagyon jól használható segédalkalmazás a [Postman](https://www.postman.com/). [Töltsük le](https://www.postman.com/downloads/) és indítsuk el az alkalmazást (nem kell regisztrálni hozzá). 

Ismerkedjünk meg a felületével! Az alábbi ábrán látható, hogyan tudunk elküldeni egy POST üzenetet. Meg kell adnunk az URL-t, az üzenet törzsét (*body*) a megfelelő formátumban. (Figyelem, JSON formátumban kötelező a property neveket idézőjelek közé tenni.) A *Send* gombra kattintva elküldhető az üzenet és alul látható a válasz.

![Postman](postman-post.png)

A jegyzőkönyvben készüljön képernyőkép minden végpont teszteléséről Postman segítéségével!

Ha elkészült a szerver, [folytassuk a kliens alkalmazással](Feladat-2.md)!


