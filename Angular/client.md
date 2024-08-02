# Angular kliens

Hozzunk létre egy üres könyvtárat a munkához. 

Az Angular keretrendszer egy `ng` nevű programcsomagot is tartalmaz, amely nagyban megkönnyíti az Angular projektek menedzselését. Segít létrehozni új projeketeket, fájlokat, illetve segít az Angular alkalmazások elindításában is. 

Ha saját gépen dolgozunk, és nincs még telepítve a gépünkre, telepítsük az `ng` programcsomagot (a laborgépeken erre nincs szükség): 

```cmd
$ npm install -g @angular/cli
```

Ezután adjuk ki a következő parancsot: 

```cmd
$ ng new twitter --inline-style=false --inline-template=false --interactive=false --prefix=twit --routing=true --skip-git=true --skip-install --strict=true --style=scss --no-standalone
```

Az egyes kapcsolók jelentését megtekinthetjük ha kiadjuk a következő parancsot: 

```cmd
ng new --help
```

**A jegyzőkönyvben válaszoljuk meg a következő kérdéseket:**

* Mit jelent a `--prefix=twit` beállítás?
* Mit jelent a `--style=scss` kapcsoló?

Telepítsük fel a függőségeket (ha szükséges, előbb a terminálon navigáljunk a létrehozott mappába - ```cd twitter```):

```cmd
$ npm install
```

Egyből ki is próbálhatjuk a legenerált alkalmazást, ha kiadjuk a következő parancsot: 

```cmd
$ ng serve 
```

... és megnyitjuk a böngészőben a `localhost:4200`-as címet.

## A generált kód megértése

Röviden nézzük át, mi is történik, amikor elindul az alkalmazásunk:
1. Az `ng serve` parancs, (vagy `ng build`) kiadásakor az `ng` fogja az összes TypeScript fájlunkat és generál belőlük egy darab JavaScript fájlt. Ugyanígy fogja az összes `scss` fájlt és generál belőlük egyetlen css fájlt. Ezután elhelyez az `index.html` fájlba két hivatkozást a generált ún. *bundle* fájlokra. Ezt az `index.html` fájlt fogja majd visszaküldeni a böngészőnek a webszerver. 
1. A JavaScript kódunk belépési pontja a `main.ts`-ben található. A `platformBrowserDynamic().bootstrapModule(AppModule)` sor elindítja az Angular keretrendszert és betölti az `AppModule`-t, amely az `app.module.ts` fájlban található. 
1. Az `AppModule` egy TypeScript modult definiál, amelyben összefogunk néhány komponenst, ill. service-t, amelyeket elérhetővé szeretnénk tenni az alkalmazásunkban. Az egyik ilyen komponens az `AppComponent`, amelyet az `app.component.ts` fájl definiál. 
1. A komponensek legfontosabb tulajdonságai a *selector*, a *komponens osztály* és a *HTML sablon*. 
    * A szelektor egy HTML tag neve. Miután a böngésző betöltötte az Angular keretrendszert megnézi, hogy a HTML kódban található-e olyan tag, amely egy adott komponens nevével egyezik meg. 
    * Ha igen, akkor a HTML sablonban lévő HTML tartalmat beilleszti az adott tag helyére. 
    * Közben létrehoz egy JavaScript objektumot a komponens osztály példányosításával. Ennek az objektumnak a propertyjei, illetve függvényei elérhetők lesznek a HTML sablonból. 

Ha jól megnézzük a generált `index.html` fájlt, abban megtalálható a `<twit-root></twit-root>` tag, amely pont az `AppComponent` szelektora. Ezért az alkalmazás elindításakor az `AppComponent` sablonjában leírt HTML tartalmat fogjuk látni. 

Hogyan tudunk újabb komponseket megjeleníteni, ha mindig az `AppComponent` fog megjelenni? Az Angular alkalmazás generálásakor beállítottuk, hogy szeretnénk ún. *routing*ot használni. Nézzük meg az `app.component.html` fájl legvégét, itt a következő HTML tag található: `<router-outlet></router-outlet>`. Töröljük ki az előtte lévő szöveget és legyen a következő a fájl tartlma: 

```html
<h1>Twitter</h1>

<router-outlet></router-outlet>
```

Mire való a `router-outlet` elem? Ha átnézzük a kódot, nem találunk olyan komponenst, amelynek ez lenne a szelektora, mert ez egy speciális elem. Az `app-routing.module.ts` fájlban létrejött egy `routes` nevű változó: 

```ts
const routes: Routes = [ ];
```

Ennek a feladata, hogy a böngészőbe beírt URL alapján eldöntse, hogy melyik komponenst szeretnék megjeleníteni a `router-outlet` helyén. A listában felsorolhatjuk a következő URLeket és a hozzájuk tartozó komponenst. Amint átírjuk az URLt, vagy a felhasználó egy linket átnavigál egy másik URLre, mindig a megfelelő komponens fog megjelenni. 

Tegyük fel például, hogy szeretnénk, hogy a `localhost:4200/tweets` URLre navigálva megjelenjen a tweetek listája. 
1. Ehhez létre kell hozni egy új komponenst
2. Ki kell egészíteni a `routes` listát
3. Ki kell tenni egy linket valahova az oldalra, amely átnavigál a `localhost:4200/tweets` oldalra. 

**A jegyzőkönyvben választoljuk meg a következő kérdéseket**: 
* Mi az az Angular modul?
* Mire való az Angular service? 

## Modell osztályok

Hozzunk létre egy `models.ts` fájlt az `src/app` mappában a következő tartalommal: 

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

## Kommunikáció a backenddel

Ahhoz, hogy kommunikálni tudjunk a backenddel, szükségünk lesz HTTP kérések küldésére. 
1. Először létrehozunk egy service-t, amelyben kiszervezzük a kommunikációjának a kódját. 
1. HTTP kérések elküldésére az Angular beépített `HttpClientModule` modulját és azon belül is a `HTTPClient` service-t fogjuk használni. 

Egészítsük ki az `app.module.ts` fájlt a `HttpClientModule` és a `FormsModule` importálásával!

```ts
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule, // importálás
    FormsModule // importálás
  ],
  providers: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

```

Generáljuk le a service-t az `ng` tool segítséségével: 

```cmd
ng g s tweets-api
```

Ez létrehoz egy `tweets-api.service.ts` fájlt. Adjuk hozzá a service-t az `app.module.ts` fájlhoz is, ha nem adta volna hozzá magától: 

```ts
@NgModule({
  declarations: [
      //...
  ],
  imports: [
    //...
  ],
  providers: [
    TweetsApiService // hozzáadás a modulhoz
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

Írjuk meg a service-be a segédfüggvényeket, amellyel le tudjuk kérdezni a tweeteket, illetve új tweetet tudunk létrehozni: 

```ts
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TweetWithId } from './models';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TweetsApiService {

  constructor(private http: HttpClient) { }

  public getTweetsAsync(): Promise<TweetWithId[]> {
    return firstValueFrom(this.http.get<TweetWithId[]>('/api/tweets'));
  }

  public createTweetAsync(text: string, userName: string, tags: string[] | undefined): Promise<any> {
    return firstValueFrom(this.http.post('/api/tweets', { text, userName, tags }));
  }
}
```

A `HttpClient` observable típusú objektumokkal tér vissza, ezekből pl. a `firstValueFrom` függvény tud Promise-t készíteni. [A Promise-ok segítségével kezeljük az aszinkronitást](https://javascript.info/async).

**A jegyzőkönyvben válaszoljuk meg a következő kérdéseket:**
 * Mit jelent, hogy a HTTP komunikáció aszinkron az alkalmazásunk és a backend között?
 * Mit jelent, hogy egy függvény visszatérési értéke `Promise`?

## Tweetek listája

Generáljunk egy új komponenst, amelyet tweetek megjelenítésére fogunk használni a következő paranccsal!

```cmd
$ ng g c tweets-list -m app
```

**A jegyzőkönyvben válaszoljuk meg a következő kérdést:**
* Az `ng g c tweets-list -m app` parancsban mik az egyes paraméterek jelentései? (Segítség kérhető az `ng g c --help` paranccsal.)

Írjuk meg a komponens osztály kódját, az importokat értelemszerűen kezelve!

```ts
@Component({
  selector: 'twit-tweets-list',
  templateUrl: './tweets-list.component.html',
  styleUrls: ['./tweets-list.component.scss']
})
export class TweetsListComponent {

  constructor(private apiSvc: TweetsApiService) { }

  tweets: TweetWithId[] | undefined;

  refresh() {
    this.apiSvc.getTweetsAsync().then(tweets => this.tweets = tweets);
  }

  getTagsString(tags: string[] | undefined): string {
    return (tags || []).join(', ');
  }
}
```

Majd írjuk meg a HTML sablont, amely megjeleníti a tweeteket!

```ts
<button (click)="refresh()">Refresh</button>

<table *ngIf="tweets">
    <thead>
        <tr>
            <th>Id</th>
            <th>Text</th>
            <th>Tags</th>
        </tr>
    </thead>
    <tbody>
        <tr *ngFor="let tweet of tweets">
            <td>{{tweet.id}}</td>
            <td>{{tweet.text}}</td>
            <td>
                {{getTagsString(tweet.tags)}}
            </td>
        </tr>
    </tbody>
</table>
```

**A jegyzőkönyvben magyarázza el, hogyan működik a fenti TypeScript osztály és hogy milyen HTML kódot generál a sablon.**

A korábban elmondottak értelmében ki kell még egészíteni az `app-routing.module.ts`-ben a `routes` változó érétkét:

```ts
const routes: Routes = [
  {
    path: 'tweets',
    component: TweetsListComponent
  }
];
```

Már csak egy link kirakása van hátra. Például az `app.component.html` alkalmas erre, mert annak tartalma mindig megjelenik: 

```html
<h1>Twitter</h1>
<a [routerLink]="['/tweets']">Tweets</a>
<br>
<router-outlet></router-outlet>
```

### Futtatás 

Mint a bevezetőben szó volt róla, ahhoz, hogy működtessük az alkalmazásunkat, az Angular tesztszerverét be kell állítani úgy, hogy proxy üzemmódban működjön. Ehhez hozzunk létre egy `proxy.conf.json` fájlt a gyökérkönyvtárban (ahol az `angular.json` is van): 

```json
{
    "/api": {
      "target": "http://localhost:3000/",
      "secure": false
    }
}
```
Így megadjuk, hogy minden kérést, amely a `/api` prefixre érkezik, azt továbbítani kell a backendnek. 

Ezután már csak futtatni kell az angular tesztszervert a fenti proxy beállításokra hivatkozva: 

```cmd
$ ng serve --proxy-config .\proxy.conf.json
```
**Készítsen képernyőképet működés közben a felületről és illessze be ezt a jegyzőkönyvbe!**

## Új Tweet hozzáadása

A fentiekhez nagyon hasonló módon egy újabb oldalt szeretnénk létrehozni, amellyel új tweeteket tudunk majd elküldeni: 

Hozzunk létre egy új komponenst:

```cmd
$ ng g c new-tweet -m app
```

```ts
//new-tweet.component.ts
import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { TweetsApiService } from '../tweets-api.service';

@Component({
  selector: 'twit-new-tweet',
  templateUrl: './new-tweet.component.html',
  styleUrls: ['./new-tweet.component.scss']
})
export class NewTweetComponent implements OnInit {

  constructor(private apiSvc: TweetsApiService,
    private router: Router) { }

  ngOnInit(): void {
  }

  text: string | undefined;
  tagsStr: string | undefined;
  userName: string | undefined;

  async send() {
    if (!this.text || !this.userName) {
      console.log(this.text);
      return;
    }
    this.apiSvc.createTweetAsync(this.text!, this.userName!, this.tagsStr?.split(",")).then(() => {
      this.router.navigateByUrl("/tweets");
    });

  }
}
```

```html
<!-- new-tweet.component.html -->
<label for="userName">Username:</label>
<input type="userName" [(ngModel)]="userName">
<br>
<label for="text">Text:</label>
<textarea [(ngModel)]="text"></textarea>
<br>
<label for="tags">Tags:</label>
<input type="tags" [(ngModel)]="tagsStr">
<br>
<button (click)="send()">Send</button>
```

**A jegyzőkönyvben magyarázza el, hogyan működik a fenti TypeScript osztály és hogy milyen HTML kódot generál a sablon.**


Egészítsük ki a `routes` változót: 
```ts
const routes: Routes = [
  //...
  {
    path: 'new',
    component: NewTweetComponent
  }
];
```

...és tegyünk ki egy új linket az `app.component.html` sablonban: 

```html
<!-- ... -->
<a [routerLink]="['/new']">New</a>
<!-- ... -->
```
**Készítsen képernyőképet működés közben a felületről és illessze be ezt a jegyzőkönyvbe!**

## Bootstrap téma használata

Bár a Bootstrap témák használatához vannak kifejezetten Angularhoz fejlesztett modulok ([példa](https://ng-bootstrap.github.io/#/home)), mi most mégis az egyszerűbb módon fogjuk hivatkozni a könyvtárat. 

A Bootstrap végső soron néhány css és JavaScript fájlt biztosít. Feladatunk annyi, hogy az alkalmazás indításakor betöltjük péládul a css fájlt. Erre két lehetőségünk is van: 
1. Az `index.html` elején a `head` tagban a szokásos módon hivatkozhatunk fájlokat. 
2. Kihasználjuk az `ng` bevezetőben már említett tulajdonságát, amely összecsomagol css és JavaScript fájlokat. 

A második megoldást fogjuk használni. 
1. Telepítsük fel a Bootstrap fájljait a gépünkre: 
    ```cmd
    $ npm install -s bootstrap
    ```
    Ez a parancsa a `node_modules/bootstrap` mappába letölti a könyvtár fájljait. 
2. Konfiguráljuk az `angular.json` fájlban a keretrendszert, hogy a letöltött könyvtárból becsomagolja az egyik css (illetve scss) fájlt:
```json
{
    /*...*/
    "projects": {
        /*...*/
        "twitter": {
            /*...*/
            "architect": {
                /*...*/
                "build": {
                    /*...*/
                    "options": {
                        /*...*/
                        "styles": [
                            "node_modules/bootstrap/scss/bootstrap.scss",
                            "src/styles.scss"
                        ],
                        /*...*/
                    },
                    /*...*/
                 },
                 /*...*/
            },
            /*...*/
        },
        /*...*/
    },
    /*...*/
}
```

**A Bootstrap keretrendszer segítségével adjon hozzá néhány stílust az oldalhoz, hogy szebben nézzen ki. Illesszen be a tweets és az új tweet oldalakról egy-egy képernyőképet a jegyzőkönyvbe!**

## Tweetek törlése

Egészítse ki az oldalt úgy, hogy a tweetek listája mellett megjelenjen egy törlés gomb is, amely megnyomására kitöröljük a megfelelő tweetet. 

**Az idevágó kódrészleteket és egy képernyőképet illesszen be a jegyzőkönybe!**

