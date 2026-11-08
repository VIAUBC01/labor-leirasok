# Feladat 2: Web API

Ebben a feladatban a Web API laboron elkészült `MovieCatalog.Api` alkalmazást telepíted az Azure-ba, az 1. feladatban létrehozott infrastruktúrára. Új adatbázis és új App Service csomag nem kell: a Web API ugyanazon az ingyenes csomagon fut, mint a Razor alkalmazás, és ugyanazt az adatbázist használja.

A lépések az 1. feladatéval analógok, ezért itt csak az eltéréseket részletezzük.

## 1. Web App létrehozása a meglévő csomagon

1. A keresőbe írd be: *App Services*, majd *Create* » *Web App*.
1. *Basics* fül:
    - *Resource group*: `rg-<neptun>`
    - *Name*: `mc-api-<neptun>`
    - *Publish*: *Code*
    - *Runtime stack*: a `MovieCatalog.Api` projekt .NET verziója (a `.csproj` fájlban a `TargetFramework`)
    - *Operating System*: *Linux*
    - *Region*: ugyanaz a régió, mint az 1. feladatban
    - *Linux Plan*: a már meglévő `plan-<neptun>` csomag. Ha nem jelenik meg a listában, ellenőrizd, hogy a régió és az operációs rendszer egyezik-e.
1. A többi fület ugyanúgy töltsd ki, mint az 1. feladatban: nincs adatbázis, nincs *Continuous deployment*, a *Basic authentication* ki van kapcsolva, nincs Application Insights.
1. *Review + create*, majd *Create*.

!!! info "Két alkalmazás közös csomagban"
    Az App Service csomag a számítási kapacitás, az alkalmazások ezen osztoznak. A Free F1 csomagon legfeljebb 10 alkalmazás futhat, a napi 60 CPU-perc alkalmazásonként értendő. Fizetős csomagnál is így szokás: több kis alkalmazás egy csomagon, amíg a terhelésük ezt elbírja.

## 2. Adatbáziselérés beállítása

1. A Web API oldalán: *Settings* » *Environment variables* » *Connection strings* fül » *Add*.
1. *Name*: az a név, amit a Web API kódja használ (a Web API laboron is `DB<neptun kód>`). *Value*: ugyanaz az ADO.NET connection string, mint az 1. feladatban. *Type*: *SQLAzure*.
1. *Apply*, majd *Apply*.

Ha helyben is ki akarod próbálni a Web API-t az Azure SQL adatbázissal, a connection stringet a `MovieCatalog.Api` projekt user secretjei közé is vedd fel, ugyanúgy, mint az 1. feladat 4. lépésében.

## 3. Felkészítés az átmeneti hibákra

A Web API *Program.cs* fájljában is kapcsold be az újrapróbálkozást, ugyanúgy, mint az 1. feladat 7. lépésében:

``` C#
builder.Services.AddDbContext<MovieCatalogDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DB<neptun kód>"),
        sql => sql.EnableRetryOnFailure()));
```

## 4. Telepítés parancssorból

A repository gyökeréből, PowerShellben (ha az `az login` óta nem zártad be a terminált, nem kell újra bejelentkezni):

```powershell
dotnet publish MovieCatalog.Api -c Release -o publish/api
tar.exe -a -c -f publish/api.zip -C publish/api .
az webapp deploy --resource-group rg-<neptun> --name mc-api-<neptun> --src-path publish/api.zip --type zip
```

Ha nincs Azure CLI, a telepítés Visual Studióból is elvégezhető, az 1. feladat 7. lépésében leírt módon.

## 5. Ellenőrzés

1. Nyisd meg böngészőben: `https://<a Web API default domainje>/api/genres`. A válasz a műfajok JSON listája.
1. Próbálj ki egy elem lekérdezését is, pl. `https://<a Web API default domainje>/api/genres/1`.
1. Ha hibát kapsz, a naplókat ugyanúgy éred el, mint az 1. feladat 8. lépésében (*App Service logs*, majd *Log stream*). A gyakori hibák is ugyanazok.

!!! note "Miért nem látszik a Swagger?"
    A Web API projektsablon a Swagger UI-t csak fejlesztői (*Development*) környezetben kapcsolja be (`if (app.Environment.IsDevelopment())`). Azure-ban *Production* fut, ezért a `/swagger` cím itt 404-es hibát ad. Addig a `GET` műveletek böngészőből próbálhatók ki, a Swagger felületet a 4. feladat végén kapcsoljuk be.

## Beadandó

| Kép tartalma | Fájlnév (kiterjesztés nélkül) |
|--------------|-------------------------------|
| Az erőforráscsoport *Overview* oldala, látszik mindkét Web App és a közös App Service csomag | `f2_rg` |
| A Web API *Environment variables* » *Connection strings* oldala (az érték ne látszódjon) | `f2_connstr` |
| A Web API telepítése: az `az webapp deploy` sikeres kimenete (vagy a Visual Studio sikeres publikálása) | `f2_deploy` |
| A `GET /api/genres` válasza böngészőben, az Azure-os címmel | `f2_api` |

## Következő feladat

Folytasd a [következő feladattal](Feladat-3.md).
