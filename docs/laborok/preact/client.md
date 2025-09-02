# Preact kliens

## Projekt létrehozása

Nyissunk meg egy tetszőleges mappát (pl. a szerver projekt mellett) a projektnek VS Code-ban, majd a Terminalban (`Ctrl+Ö`) adjuk ki az alábbi parancsot:

> `npm init preact`

A parancs kiadása után több kérdésre kell válaszolnunk. Projektkönvytárnak (`Project directory`) válasszunk egy hangzatos nevet (pl. `twitter`), a projekt nyelvének (`Project language`) pedig a `TypeScript`-et. A többi kérdésre nyomjunk ENTER-t, tehát automatikusan nemmel válaszoljunk.

A parancs lefutása után navigáljunk az újonnan létrehozott mappába a Terminal-ban:

> `cd twitter`

Ezután telepítsük a függőségeket a következő parancs kiadásával:

> `npm install`

Az utolsó önálló feladatnál a `Bootstrap`-re is szükségünk lesz, ezt akár telepíthetjük most is:

> `npm install bootstrap`

Nézzük meg, hogy elindul-e az alkalmazás:

> `npm run dev`

Ha valamilyen hibába futunk a fenti parancsok lefutása során, győződjünk meg róla, hogy friss node.js-t (és npm-et) használunk a `node -v` és `npm --version` parancsok kiadásával. Amennyiben nem frissek, próbáljuk meg a [node.js-t](https://nodejs.org/en) és / vagy az npm-et (`npm install -g npm@latest` parancs) frissíteni.

Az alkalmazás alapértelmezetten (ahogy azt a konzolon is olvashatjuk) a <a href="http://localhost:5173" target="_blank">`http://localhost:5173`</a> címen fut. Hasonlóan a korábbiakhoz, a legtöbb esetben itt sem kell újraindítanunk a futást ha változtatunk a forráskódon. A háttérben a fordító automatikusan figyeli ezeket a változásokat. Indítás után az alábbihoz hasonlót kell, hogy lássunk:

```
VITE v6.3.5  ready in 555 ms

  ➜  Local:   http://localhost:5173/
  ➜  Network: use --host to expose
  ➜  press h + enter to show help
```

Opcionálisan a `package.json` fájlban (`"start": "vite"` --> `"dev": "vite"`) átírhatjuk, hogy ne az `npm run dev`, hanem az `npm start` parancs kiadásával fusson az alkalmazásunk. Ez jobban követi az ipari standardeket, de végső soron bármelyiket használhatjuk.

**A jegyzőkönyvben válaszoljuk meg a következő kérdéseket:**

* Mi a Vite szerepe a fejlesztési folyamatban?
* Miért használunk TypeScript-et a JavaScript helyett?



## A projekt struktúrája

Vizsgáljuk meg a létrejött projekt tartalmát (csak a releváns mappák és fájlok vannak kiemelve):

- **node_modules**: a Preact és függőségei, melyek a kiinduló projekthez kellenek
- **package.json**: az alkalmazás függőségeinek listája, ide tudjuk felvenni az npm csomagjainkat függőségként (vagy az `npm install [függőség_neve]` paranccsal ebbe a fájlba kerülnek be)
- **tsconfig.json**: az alkalmazás TypeScript konfigurációja
- **vite.config.ts**: a [Vite](https://vite.dev/) build tool konfigurációja, ezt használjuk a háttérben
- **index.html**: a kiinduló HTML fájl, itt tudjuk átállítani a címet és egyéb metaadatokat
  - a `div` segítségével van összekötve az `index.tsx` fájllal
- **src**: az alkalmazás forráskódja, elsősorban itt fogunk dolgozni
    - **assets**: ebben a mappában tárolhatjuk a statikus tartalmakat (pl. képek)
    - **style.css**: a globális CSS fájl, az egész alkalmazásra érvényes szabályokkal - az egyes komponenseknek külön CSS fájlokat is készíthetünk
    - **index.tsx**: a fő komponens, ami a tartalom megjelenítéséért felelős

Írjuk át az alkalmazás címét az `index.html` fájlban valami kifejezőbbre!

```HTML
<title>Preact labor - Twitter</title>
```

???+ tip "Formázás"
    A kódot formázni az `Alt+Shift+F` billentyűkombinációval tudjuk.

**A jegyzőkönyvben válaszoljuk meg a következő kérdéseket**: 
* Mi az a komponens Preact környezetben?
* Hogyan tudunk egy komponenshez személyre szabott formázást beállítani? 

## Modell típusok

Hozzuk létre az `src/models.ts` fájlt a következő tartalommal: 

```ts
export type Tweet = {
    text : string,
    userName: string,
    tags?: string[]
}

export type TweetWithId = Tweet & {
    id?: string
}
```

Ezeket a típusokat fogjuk használni a tweetek leírására. 

## HTTP kommunikáció a backenddel

A HTTP kérések kezelését érdemes külön service osztályba szervezni, hogy:
- Központosítsuk az API hívásokat
- Egységes hibakezelést valósítsunk meg
- Könnyen módosítható legyen az API alapútvonala

Hozzunk létre egy `src/services/api.ts` fájlt:

```typescript
import { Tweet, TweetWithId } from '../models';

const API_BASE = '/api';

export class ApiService {
    private async request<T>(endpoint: string, options?: RequestInit): Promise<T> {
        const response = await fetch(`${API_BASE}${endpoint}`, {
            headers: {
                'Content-Type': 'application/json',
                ...options?.headers
            },
            ...options
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        return response.json() as Promise<T>;
    }

    // Tweetek lekérése
    public async getTweets(): Promise<TweetWithId[]> {
        return this.request<TweetWithId[]>('/tweets');
    }

    // Új tweet létrehozása
    public async createTweet(tweet: Omit<Tweet, 'id'>): Promise<void> {
        return this.request<void>('/tweets', {
            method: 'POST',
            body: JSON.stringify(tweet)
        });
    }

    // Tweet törlése
    public async deleteTweet(id: string): Promise<void> {
        return this.request<void>(`/tweets/${id}`, {
            method: 'DELETE'
        });
    }
}

// Singleton példány
export const apiService = new ApiService();
```

## Proxy beállítás

Ahhoz, hogy a beérkező kérések továbbítódjanak a backend felé (amennyiben a kliens nem tudja kiszolgálni azokat), proxyzást kell beállítanunk, vagyis a `localhost:3000` felé kell továbbítanunk a `/api` útvonalú kéréseket. Ezek azok a kérések, amiket az előzőleg létrehozott service osztály kezel. Módosítsuk a `vite.config.ts` fájl tartalmát az alábbi módon:

import { defineConfig } from 'vite';
import preact from '@preact/preset-vite';

```typescript
export default defineConfig({
    plugins: [preact()],
    server: {
        proxy: {
            '/api': {
                target: 'http://localhost:3000',
                changeOrigin: true,
                secure: false
            }
        }
    }
});
```

## Tweetek listája

Hozzunk létre egy `src/components/TweetsList.tsx` komponenst:

```typescript
import { useState, useEffect } from 'preact/hooks';
import { apiService } from '../services/api';
import { TweetWithId } from '../models';

export function TweetsList() {
    const [tweets, setTweets] = useState<TweetWithId[]>([]);
    const [error, setError] = useState<string | null>(null);

    const loadTweets = async () => {
        try {
            const data = await apiService.getTweets();
            setTweets(data);
            setError(null);
        } catch (err) {
            setError('Hiba történt a tweetek betöltésekor!');
            console.error(err);
        }
    };

    useEffect(() => {
        loadTweets();
    }, []);

    const getTagsString = (tags?: string[]) => (tags || []).join(', ');

    return (
        <div>
            {error && <div class="alert alert-danger">{error}</div>}

            <button onClick={loadTweets} class="btn btn-primary">
                Frissítés
            </button>

            {tweets.length > 0 && (
                <table>
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Text</th>
                            <th>Tags</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tweets.map(tweet => (
                            <tr key={tweet.id}>
                                <td>{tweet.id}</td>
                                <td>{tweet.text}</td>
                                <td>{getTagsString(tweet.tags)}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}
```

A fenti komponens a korábban elkészített service segítségével megjeleníti a tweet-eket egy táblázatban, illetve, ha hibába fut, akkor egy hibaüzenetet jelenít meg az oldalon. A `Frissítés` gomb megnyomásával újból lekérhetjük az aktuális tweeteket a backendtől.

Frissítsük az index.tsx `App` függvényét, hogy az új komponensünk jelenjen meg az oldal betöltésekor:

```typescript
export function App() {
	return (
	  <div>
		  <TweetsList />
	  </div>
	);
}
```

**Készítsen képernyőképet működés közben a felületről és illessze be ezt a jegyzőkönyvbe!**

**A jegyzőkönyvben válaszoljuk meg a következő kérdéseket**: 
* Mi a useState szerepe egy komponensben?
* Mi a useEffect szerepe egy komponensben?


## Új Tweet hozzáadása

Az előzőekhez nagyon hasonló módon egy újabb oldalt szeretnénk létrehozni, amellyel új tweeteket tudunk majd létrehozni. Hozzunk létre egy `src/components/NewTweet.tsx` komponenst:

```typescript
import { useState } from 'preact/hooks';
import { apiService } from '../services/api';

export function NewTweet() {
    const [text, setText] = useState('');
    const [tagsStr, setTagsStr] = useState('');
    const [userName, setUserName] = useState('');

    const send = async () => {
        if (!text || !userName) return;

        apiService.createTweet({
            text,
            userName,
            tags: tagsStr ? tagsStr.split(',') : undefined
        });

        // route('/tweets');
        window.location.href = '/tweets';
    };

    return (
        <div>
            <label>
                Username:
                <input type="text" value={userName}
                    onInput={(e) => setUserName(e.currentTarget.value)} />
            </label>
            <br />
            <label>
                Text:
                <textarea value={text}
                    onInput={(e) => setText(e.currentTarget.value)} />
            </label>
            <br />
            <label>
                Tags:
                <input type="text" value={tagsStr}
                    onInput={(e) => setTagsStr(e.currentTarget.value)} />
            </label>
            <br />
            <button onClick={send}>Küldés</button>
        </div>
    );
}
```

A komponens egy form és a korábban létrehozott service segítségével támogatja az új tweet létrehozását. Új tweet felvétele után a komponens automatikusan visszairányít a tweetek listájára. A `window.location.href` beállítás helyett routing-ot érdemes inkább használni (`route` függvényhívás, ld. a kikommentezett részt), a következő feladat elvégzése után használjuk is ezt!

## Routing

 Annak érdekében, hogy az új tweet hozzáadó oldalra szépen el tudjunk navigálni, vezessünk be routing-ot az alkalmazásban! Ehhez először is telepítsük a `preact-router` csomagot. Előtte érdemes leállítani az alkalmazás futtatását is.

 > `npm install preact-router`

 Ezután módosítsuk az `index.tsx` fájlt, felvéve a következő útvonalakat és egy új `Header` komponenst, mely a navigációt fogja végezni. Az importálásokról se feledkezzünk meg:

```typescript
export function App() {
    return (
        <div>
            <Header />
            <Router>
                <Route path="/tweets" component={TweetsList} />
                <Route path="/new" component={NewTweet} />
            </Router>
        </div>
    );
}
```

Ezután hozzunk létre egy `src/components/Header.tsx` komponenst és importáljuk be az `index.tsx` fájlba:

```typescript
import { Link } from 'preact-router';

export function Header() {
    return (
        <header>
            <h1>Twitter</h1>
            <nav>
                <Link href="/tweets">Tweets</Link>
                <Link href="/new">New Tweet</Link>
            </nav>
        </header>
    );
}
```

A végén ne felejtsük el a végén átírni a `NewTweet.tsx` komponens kódját, hogy az újonnan bevezetett routing-ot használja az automatikus visszairányításnál!

 **Készítsen képernyőképet működés közben az új tweet hozzáadásának felületéről, valamint a tweetek listájában látható új tweetről és illessze be ezeket a jegyzőkönyvbe!**

# Önálló feladatok

## Bootstrap stílusozás

A Bootstrap keretrendszer segítségével adjon hozzá néhány stílust az oldalhoz, hogy szebben nézzen ki. Különösen ügyeljen a Header komponens formázására, itt használja a [navbar, nav-link](https://getbootstrap.com/docs/5.3/components/navbar/) stb. Bootstrap osztályokat! Ha korábban nem tette meg, előbb telepítse a bootstrap csomagot:

> `npm install bootstrap`

**Illesszen be a tweets és az új tweet oldalakról egy-egy képernyőképet a jegyzőkönyvbe!**

## Tweetek törlése

Egészítse ki az oldalt úgy, hogy a tweetek listájában minden tweet mellett megjelenjen egy törlés gomb is, amely megnyomására kitöröljük az adott tweetet. 

**Az idevágó kódrészleteket és egy képernyőképet illesszen be a jegyzőkönybe!**

