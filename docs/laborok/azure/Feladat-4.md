# Feladat 4: Web API CI/CD

A 3. feladatban a Razor alkalmazás telepítését automatizáltad. Most ugyanezt a Web API-hoz is elkészíted, végül egy kódmódosítással, már a CI/CD-n keresztül, bekapcsolod a Swagger felületet az Azure-ban futó Web API-n.

## 1. Jogosultság a Web API-ra

Új felügyelt identitásra és új federált hitelesítő adatra nincs szükség: a 3. feladatban beállított federált bizalom a repository-hoz és a `megoldas` ághoz kötött, nem egy adott alkalmazáshoz. Az identitásnak viszont eddig csak a Razor alkalmazásra volt joga, ezért a Web API-ra is meg kell adni:

1. Nyisd meg a Web API-t (`mc-api-<neptun>`), majd *Access control (IAM)* » *Add* » *Add role assignment*.
1. *Role*: *Website Contributor*, *Next*.
1. *Assign access to*: *Managed identity*, *Select members* » *User-assigned managed identity* » `id-github-<neptun>`.
1. *Review + assign*.

!!! question "Gondolatébresztőként"
    Kényelmesebb lett volna a szerepkört egyszer, az egész erőforráscsoportra megadni. Mit tudna ekkor tenni valaki, aki megszerzi a repository feletti irányítást? Miért jobb a két, alkalmazásszintű hozzárendelés?

## 2. A Web API workflow-ja

Másold le a `.github/workflows/deploy-web.yml` fájlt `.github/workflows/deploy-api.yml` néven, és írd át benne az alábbiakat:

``` YAML
name: Deploy MovieCatalog.Api

on:
  push:
    branches: [ megoldas ]
    paths:
      - 'MovieCatalog.Api/**'
      - '.github/workflows/deploy-api.yml'

# ...

env:
  AZURE_WEBAPP_NAME: mc-api-<neptun>
  PROJECT_PATH: MovieCatalog.Api
  # a többi érték (DOTNET_VERSION, AZURE_CLIENT_ID, AZURE_TENANT_ID, AZURE_SUBSCRIPTION_ID) változatlan
```

A `jobs` rész változatlan maradhat. A két workflow a `paths` szűrő miatt egymástól függetlenül fut: ha csak a Razor projekt változik, csak a Razor alkalmazás települ újra, és fordítva.

## 3. Kipróbálás

1. Commitold és pushold a `deploy-api.yml` fájlt a `megoldas` ágra.
1. A GitHubon az *Actions* fülön elindul a *Deploy MovieCatalog.Api* futás. Várd meg, amíg zöld lesz.
1. Ellenőrizd böngészőben, hogy a Web API továbbra is válaszol: `https://<a Web API default domainje>/api/genres`.

A hibák és megoldásuk ugyanazok, mint a 3. feladatban. Ha a telepítésnél `AuthorizationFailed` hibát kapsz, a szerepkör-hozzárendelés még nem érvényesült: várj néhány percet, és indítsd újra a futást (*Re-run jobs*).

## 4. A Swagger felület bekapcsolása

A Web API *Program.cs* fájljában a Swagger UI jelenleg csak fejlesztői környezetben indul el:

``` C#
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

1. Vedd ki a két hívást a feltételből, hogy minden környezetben bekapcsoljanak:

    ``` C#
    app.UseSwagger();
    app.UseSwaggerUI();
    ```

1. Commitold és pushold. A *Deploy MovieCatalog.Api* workflow magától elindul, kézi telepítésre nincs szükség.
1. A futás után nyisd meg: `https://<a Web API default domainje>/swagger`. Próbáld ki a felületről a `GET /api/genres` műveletet (*Try it out* » *Execute*).

!!! warning "Swagger éles környezetben"
    A Swagger felület bárkinek megmutatja az API összes végpontját, és ki is próbálhatók róla. A Web API-nk hitelesítés nélküli, így a módosító végpontok (`PUT`, `POST`, `DELETE`) ettől függetlenül is bárki számára hívhatók. A Swagger ezt a hiányosságot nem okozza, csak láthatóvá teszi. Éles rendszerben a Swagger felületet általában csak teszt környezetben vagy bejelentkezés után teszik elérhetővé.

??? tip "Szebb megoldás: konfigurációból kapcsolható Swagger"
    A feltételt nem kell teljesen elhagyni, konfigurációhoz is köthető:

    ``` C#
    if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("EnableSwagger"))
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    ```

    Ekkor Azure-ban a Web API *App settings* beállításai között egy `EnableSwagger` = `true` bejegyzés kapcsolja be, a kód minden környezetben ugyanaz marad.

## Beadandó

| Kép tartalma | Fájlnév (kiterjesztés nélkül) |
|--------------|-------------------------------|
| A Web API *Access control (IAM)* » *Role assignments* oldala, látszik az identitás *Website Contributor* szerepköre | `f4_role` |
| A GitHub *Actions* fülön a *Deploy MovieCatalog.Api* workflow egy sikeres futása (látszanak a lépések) | `f4_run` |
| Az Azure-ban futó Web API Swagger felülete, a lefuttatott `GET /api/genres` kérés válaszával (az Azure-os cím látszik a címsorban) | `f4_swagger` |

# Végeztél

Végeztél a kötelező feladatokkal. Ha van még időd, folytasd a [bónusz feladattal](Bonusz.md).

Ha nem tartod meg az erőforrásokat, a beadás után törölheted az `rg-<neptun>` erőforráscsoportot. Ez minden benne lévő erőforrást töröl.
