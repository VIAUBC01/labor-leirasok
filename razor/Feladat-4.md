# Feladat 4.

Készítsük el a szűrőjét a műveknek!

A szűrő egy beviteli űrlap lesz, amely GET kérést intéz az Index oldalhoz (hasonlóan a [Lapozás és rendezés](Feladat-3.md) feladatban látottakhoz).

Előszöris, a bal oldali "műfajfelhőben" levő egyes műfajokra kattintva szűrjük le az aktuális találati listát úgy, hogy csak a műfajnak megfelelő elemek jelenjenek meg. Ha a műfaj "ki van választva", kapjon egy kiemelést. Ha újra kiválasztjuk, az aláhúzás eltűnik. Ha a lapozással már foglalkoztunk, akkor az oldalszámot 1-re kell állítanunk (vagy törölnünk, ugyanis 1 az alapérték).

Az Index oldalra kössük ki az URL-ből érkező MoviesFilter objektumot:

```C#
[BindPropery(SupportsGet = true)]
public MoviesFilter Filter { get; set; }
```

Ezt a szűrőt az esetleges [lapozási és rendezési paraméterekkel](Feladat-3.md) adjuk át a lekérdezést végrehajtó `GetTitlesAsync` metódusnak. Hogyan tudjuk megadni ezt a komplex objektumot GET paraméterként? Például: `https://localhost:44332/?Filter.Genres=Sci-Fi`. Ezzel láthatjuk, hogy kitöltődik megfelelően a Genres lista egyetlen elemmel, ami a Sci-Fi. Ha több elemet szeretnénk megadni, akkor többször soroljuk fel ugyanazt a kulcsot (a query paraméterben szabvány szerint nem történik "sorosítás", tehát nem használunk tömböket vagy JSON objektumokat, csak egyszerű értékeket string kulccsal): `https://localhost:44332/?Filter.Genres=Sci-Fi&Filter.Genres=Action`. Egyelőre ezzel a problémával nem foglalkozunk, de ezért értelmezhető a string tömböt fogadó `Filter.Genres` tulajdonság számára egyetlen normál string átadása.

Egy lehetséges megoldás váza az alábbi:

``` HTML
@foreach (var genre in Model.Genres) // Az összes műfajt kiírjuk.
{
    var selected = Model.Filter?.Genres?.Any(g => g == genre.Name) == true; // Jelzi, hogy az aktuális műfajra épp szűrünk-e.
    <a class="text-nowrap @(selected ? "text-dark border border-dark" : "")" 
       asp-all-route-data="@(Request.Query.ToDictionary(kv => kv.Key, kv => kv.Value.ToString()))" @* Az aktuális query string paramétereket megtartjuk. *@
       asp-route-Filter.Genres="@(selected ? null : genre.Name)" @* A Filter.Genres-t értelemszerűen töröljük vagy kitöltjük. *@
       style="font-size: @genre.SizeInEm" 
       asp-route-PageNumber="@null"> @* A PageNumbert töröljük. *@
        @genre.Name 
        <small class="text-muted">@genre.TitleCount</small>
    </a>
}
```

Ezzel a szűrés "már működik is". Ami hátravan, az az űrlap elkészítése a szűréshez.

Az alábbi ábrán látható paramétereket kérd be a felhasználótól, értelemszerűen a szűréseknek megfelelően működnie kell!

![Feladat 4.](images/feladat-4.png)


## Következő feladatok

A korábbi feladatokat tetszőleges sorrendben elvégezheted, ha még nem tetted:

- [Mű szerkesztő oldala](Feladat-2.md)

- [Lapozás és rendezés](Feladat-3.md)
