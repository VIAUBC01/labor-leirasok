# Twitter webalkalmazás

A webalkalmazás a `client` könyvtárban lesz és nagyon kevés fájlból fog állni, ezeket hozzuk is létre: 
* `index.html`
* `src/main.ts`
* `tsconfig.json` (mivel a ts fájlból js fájlt szeretnénk generálni) - ezt érdemes a `tsc --init` paranccsal generálni

A `tsconfig.json` fájlban itt használjuk a következő beállításokat:

```json
{ 
    /*...*/
    "target": "es6",
    "module": "commonjs",                     
    "sourceMap": true,                     
    "outDir": "./build/",                        
    "strict": true,        
    /*...*/
}
```

A `main.ts`-be írjunk egy egyszerű kódot: 

```ts
console.log("hello world!");
```

Hozzunk létre egy minimális html fájlt, amelyben a majd lefordított `main.js` fájlt hivatkozzuk.

```html
<html>
<head>
    <script src="build/main.js"></script>
</head>
<body>
    <h1>Twitter client!<h1>
</body>
</html>
```

Ne felejtsük el, hogy a használhatoz a `main.ts`-t mindig le kell fordítani (ha változik) a `tsc` paranccsal, különben nem áll elő a `main.js` fáj.

Webszervernek egy egyszerű `npm`-es programot használunk: `http-server`, mert ez parancssorból gyorsan elindítható, hogy egy könyvtár tartalmát kiszolgálja és proxy funkciója is van. 

```cmd
$ npm install http-server -g
```

Ha el akarjuk indítani a webszervert proxy üzemmóddal, akkor az index.html könytárában az alábbi parancsot adjuk ki (a 3000 helyére azt a portszámot írjuk, amivel  a backend alkalmazásunkat konfiguráltuk).

```cmd
$ http-server --proxy http://localhost:3000/
```

Ha elindult a `http-server`, alapértelmezetten a 8080-as porton figyel (ez egyébként konfigurálható a `--port` kapcsolóval). A weboldalunkat tehát megnyithatjuk a böngészőben a `http://localhost:80808/index.html` URL-t beírva. 

Mivel a `main.js`-t betölti a HTML oldalunk és abban a konzolra kiírunk egy szöveget, ha megnyitjuk a developer toolbar-t a böngészőben (F12-vel), akkor a *Console* tabon látnunk kell ezt a szöveget. 

**A jegyzőkönyvbe illesszünk egy egy képet a böngészőben megjelenő szövegről és a konzolra kiírt szövegről.**

## Tweetek lekérdezése és megjelenítése

A böngészőben, az oldalunkon meg szeretnénk jeleníteni egy gombot, aminek a hatására lekérdezzük a tweeteket és megjelenítjük azokat egy táblázatban. Ehhez a böngésző a backendnek a proxyn keresztül a `/tweets` végpontjára fog küldeni egy HTTP kérést a háttérben, JavaScript kódból. Ezt nevezzük AJAX hívásnak. HTTP kérés elküldésére a [*fetch API*](https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API)t használjuk.

Milyen lépésekben tudjuk ezt a funkcionalitást implementálni? 
1. Elhelyezünk egy gombot a HTML kódban. 
1. Feliratkozunk a bomb megnyomására egy JavaScript függvénnyel. 
1. A JavaScript függvény elküld egy AJAX kérést és lekérdezi a tweeteket. Mivel ezek JSON formátumban érkeznek, ezért ezekhez közvetlenül JavaScript objektumokként férhetünk hozzá. (Ha például XML formátumban küldenénk vissza a tweeteket, akkor gondoskodni kellene ezek beolvasásáról.)
1. A HTML kódban megjelenítünk egy táblázatot, amelynek kitöröljük a meglévő sorait, majd minden egyes tweethez létrehozunk egy új sort, amiben beállítjuk a cellákat. A HTML struktúrát a `document` globális objektum által nyújtott APIval tudjuk módosítani. 

### Feliratkozás

Az `index.html` fájl `body` elemében helyezzük el a következő részt: 

```html
<button onclick="refreshTweets()">Refresh</button>
<table id="tableTweets">
    <thead>
        <tr>
            <th>Id</th>
            <th>Text</th>
            <th>Tags</th>
        </tr>
    </thead>
    <tbody>

    </tbody>
</table>
```

A `table` elemet és a fejléceket előkészíthetjük, ezeket nem kell mindig újragenerálni. 

A `main.ts` fájlban írjuk meg a `refreshTweets` függvényt:

```ts

function refreshTweets() {
    fetch("/tweets")
        .then(result => result.json())
        .then((tweets : {text: string, id: string, tags? : string[]}[]) => {
            let tbody = document.getElementById("tableTweets")?.getElementsByTagName("tbody")[0]!;
            tbody.innerHTML = '';

            function createTd(content: string) {
                let td = document.createElement('td');
                td.textContent = content;
                return td;
            }
            
            for (let t of tweets) {
                let tr = document.createElement('tr');
                tbody.appendChild(tr);
                tr.appendChild(createTd(t.id));
                tr.appendChild(createTd(t.text));
                tr.appendChild(createTd((t.tags|| []).join(', ')));
            }
        });
}
```

**A jegyzőkönyvben válaszolja meg a következő kérdéseket**: 
* Mire való a `document.getElementById` függvény? 
* Mire való az `appendChild` függvény? 
* Mit csinál a fenti kódban a `createTd` függvény?
* Milyen HTML elemekre van szükség, hogy egy `table`-ben sorokat és cellákat hozzunk létre? 
* Milyen formátumban jeleníti meg a táblázat egy tweet tagjeit? 

**Készítsen képernyőképet a futó böngészőről, amint a weboldal megjelenít legalább egy tweetet a táblázatban. Ezt is tegye bele a jegyzőkönyvbe!**

[Folytassa az önálló feladatokkal!](feladat3.md)


