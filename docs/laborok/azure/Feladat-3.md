# Feladat 3: CI/CD elkészítése

Az 1. és 2. feladatban manuálisan telepítettük ki az alkalmazásokat, ám ez a gyakorlatban körülményes. Most ezt automatizáljuk úgy, hogy minden push a `megoldas` ágra lefordítja és telepíti a Razor alkalmazást. A [GitHub Actions](../../tudnivalok/github/GitHub-Actions.md) számos dologra használható (többek között az AHK is ezzel értékeli és pontozza a laborokat), most a megoldás automatikus telepítésére használjuk.

!!! info "Hogyan hitelesíti magát a GitHub az Azure felé?"
    A workflow-nak jogosultság kell ahhoz, hogy telepítsen az Azure-ba. A régebbi megoldás egy jelszó (pl. *publish profile*) eltárolása volt a GitHubon, ami biztonsági kockázatot is rejtett.

    A laboron az OpenID Connect (OIDC) alapú, ún. *federált* azonosítást használjuk:

    1. Az Azure-ban létrehozol egy felügyelt identitást (managed identity), ami tulajdonképpen egy jelszó nélküli technikai felhasználó.
    1. Beállítod, hogy ez az identitás megbízik a GitHub által kiállított tokenekben, de csak a megadott repository `megoldas` ágáról fogadja el őket.
    1. Az identitás csak egyetlen jogot kap: a Razor Web App-ra telepíthet.
    1. A workflow futásakor a GitHub egy néhány percig érvényes tokent állít ki, ezt az Azure becseréli egy rövid életű hozzáférési tokenre.

    Ezáltal nem kell jelszót tárolni, így nincs is mit ellopni.

## 1. Felügyelt identitás létrehozása

1. A portál keresőjébe írd be: *Managed Identities*, majd *Create*.
1. *Resource group*: `rg-<neptun>`, *Region*: a régiód, *Name*: `id-github-<neptun>`.
1. *Review + create*, majd *Create*.
1. Az elkészült identitás *Overview* oldaláról jegyezd fel a Client ID értékét.

## 2. Federált hitelesítő adat (federated credential)

1. Az identitás oldalán: *Settings* » *Federated credentials* » *Add Credential*.
1. *Federated credential scenario*: *GitHub Actions deploying Azure resources*.
1. Töltsd ki a repository adatai alapján. A repository címe `https://github.com/<organization>/<repository>` alakú:
    - *Organization*: `<organization>`
    - *Organization ID*: a szervezet numerikus azonosítója (lásd lent)
    - *Repository*: `<repository>`
    - *Repository ID*: a repository numerikus azonosítója (lásd lent)
    - *Entity*: *Branch*
    - *Branch*: `megoldas`
    - *Name*: `github-megoldas`
1. *Add*.

!!! tip "Honnan tudom a numerikus azonosítókat?"
    - **Organization ID**: nyisd meg böngészőben a `https://api.github.com/orgs/<organization>` címet. A válasz elején lévő `"id"` mező értéke kell.
    - **Repository ID**: a repository privát, ezért az API bejelentkezés nélkül nem adja vissza. Nyisd meg a repository oldalát a GitHubon (bejelentkezve), nézd meg a forrását (++ctrl+u++), és keress rá: `repository_id`. Az `octolytics-dimension-repository_id` sor `content` értéke kell. (Ugyanitt az `octolytics-dimension-user_id` a szervezet azonosítója.)
    - Ha telepítve van a GitHub CLI: `gh api repos/<organization>/<repository> --jq "{repo: .id, org: .owner.id}"`

A létrejött bejegyzés *Subject identifier* értéke: `repo:<organization>@<organization ID>/<repository>@<repository ID>:ref:refs/heads/megoldas`. Az Azure csak az ilyen tárgyú tokent fogadja el, egy másik ágról, másik repository-ból vagy forkból indított workflow nem tud ezzel az identitással bejelentkezni.

!!! info "Miért kellenek a numerikus azonosítók?"
    A nevek megváltoztathatók: ha valaki átnevezi a repository-t, majd egy másik repository-t hoz létre a régi néven, az a régi név alapján jogosult lenne a telepítésre. A numerikus azonosító viszont soha nem változik és nem használható fel újra. A GitHub 2026 közepe óta az új repository-k tokenjeiben ezt a változtathatatlan (*immutable*) formátumot használja.

## 3. Jogosultság adása (RBAC)

1. Nyisd meg a Razor Web App-ot (`mc-web-<neptun>`), majd *Access control (IAM)* » *Add* » *Add role assignment*.
1. *Role*: **Website Contributor**, *Next*.
1. *Assign access to*: *Managed identity*, majd *Select members*. *Managed identity*: *User-assigned managed identity*, és válaszd ki az `id-github-<neptun>` identitást.
1. *Review + assign*.

!!! info "Legkisebb jogosultság elve"
    A jogot csak erre az egy Web App-ra adtad, nem az erőforráscsoportra vagy az előfizetésre. Ha a GitHub repository-t valaki megszerezné, akkor is csak ezt az egy alkalmazást tudná felülírni: nem tudna adatbázist törölni vagy fizetős erőforrást létrehozni.

A szerepkör-hozzárendelés érvényesülése néhány percig tarthat.

## 4. A workflow elkészítése

1. Kérd le a bérlő (*tenant*) és az előfizetés azonosítóját:

    ```powershell
    az account show --query "{tenantId: tenantId, subscriptionId: id}" --output table
    ```

    Ha nincs Azure CLI: a portálon a *Subscriptions* oldalon található a *Subscription ID*, a *Microsoft Entra ID* oldalon a *Tenant ID*.

1. Hozd létre a repository-ban a `.github/workflows/deploy-web.yml` fájlt az alábbi tartalommal, és írd át a `<...>` részeket:

    ``` YAML
    name: Deploy MovieCatalog.Web

    on:
      push:
        branches: [ megoldas ]
        paths:
          - 'MovieCatalog.Web/**'
          - '.github/workflows/deploy-web.yml'
      workflow_dispatch:

    permissions:
      id-token: write   # az OIDC tokenhez
      contents: read    # a checkouthoz

    env:
      AZURE_WEBAPP_NAME: mc-web-<neptun>
      PROJECT_PATH: MovieCatalog.Web
      DOTNET_VERSION: '8.0.x'
      AZURE_CLIENT_ID: <a felügyelt identitás Client ID-ja>
      AZURE_TENANT_ID: <tenant ID>
      AZURE_SUBSCRIPTION_ID: <subscription ID>

    jobs:
      build-and-deploy:
        runs-on: ubuntu-latest
        steps:
          - name: Checkout
            uses: actions/checkout@v4

          - name: Setup .NET
            uses: actions/setup-dotnet@v4
            with:
              dotnet-version: ${{ env.DOTNET_VERSION }}

          - name: Publish
            run: dotnet publish ${{ env.PROJECT_PATH }} -c Release -o ${{ runner.temp }}/webapp

          - name: Azure login (OIDC)
            uses: azure/login@v2
            with:
              client-id: ${{ env.AZURE_CLIENT_ID }}
              tenant-id: ${{ env.AZURE_TENANT_ID }}
              subscription-id: ${{ env.AZURE_SUBSCRIPTION_ID }}

          - name: Deploy to App Service
            uses: azure/webapps-deploy@v3
            with:
              app-name: ${{ env.AZURE_WEBAPP_NAME }}
              package: ${{ runner.temp }}/webapp
    ```

    - `on`: a workflow a `megoldas` ágra történő pushkor fut, de csak akkor, ha a Razor projekt vagy maga a workflow változott (`paths`). A `workflow_dispatch` a kézi indítást engedi, de a GitHub a *Run workflow* gombot csak akkor mutatja, ha a workflow fájl az alapértelmezett (`main`) ágon is megvan, ezért nálunk ez legfeljebb a beadás után használható.
    - `permissions`: az `id-token: write` engedi, hogy a workflow OIDC tokent kérjen a GitHubtól.
    - A *Publish* lépés ugyanaz a `dotnet publish`, amit az 1. és 2. feladatban kézzel futtattál.
    - Az `azure/login` a federált hitelesítéssel bejelentkezik, az `azure/webapps-deploy` pedig feltölti a csomagot, ugyanúgy, mint korábban az `az webapp deploy`.
    - A `DOTNET_VERSION` a projekted .NET verziójához igazodjon (`net8.0` » `8.0.x`).

    !!! warning "YAML"
        A YAML fájlban a behúzás jelentéssel bír, és csak szóköz használható, tabulátor nem. Egyetlen elcsúszott szóköz is hibás workflow-t eredményez.

    !!! note "Miért nem titkos (secret) változóban vannak az azonosítók?"
        A Client ID, a tenant ID és a subscription ID azonosítók, nem jelszavak: önmagukban nem adnak hozzáférést. A biztonságot a 2. lépésben beállított federált bizalom adja, ami a te repository-dhoz és ágadhoz kötött. Ezeket az értékeket GitHub *Secrets* vagy *Variables* tárolásban is el lehetne helyezni, de ehhez a laborhoz erre nincs szükség.

## 5. Kipróbálás

1. Commitold és pushold a workflow fájlt a `megoldas` ágra. A GitHubon az *Actions* fülön elindul a *Deploy MovieCatalog.Web* futás. Várd meg, amíg zöld lesz.
1. Módosíts valami látható dolgot a Razor alkalmazásban, pl. a `Pages/Shared/_Layout.cshtml` láblécét:

    ``` HTML
    &copy; 2026 - <neptun kód> - CI/CD-vel telepítve
    ```

1. Commitold és pushold. Kézi telepítés nélkül, a workflow lefutása után az Azure-ban futó alkalmazáson megjelenik a változás.

Gyakori hibák:

- **`AADSTS700213: No matching federated identity record found`**: a token tárgya nem egyezik a federált hitelesítő adattal. A hibaüzenetben szerepel, milyen tárgyat (*subject*) küldött a GitHub: ezt vesd össze a *Subject identifier* értékével. Ellenőrizd az organization, a repository és az ág nevét, és hogy valóban a `megoldas` ágon fut-e a workflow.
- **`AuthorizationFailed` vagy 403-as hiba a telepítésnél**: hiányzik a *Website Contributor* szerepkör, vagy még nem érvényesült. Várj néhány percet, majd indítsd újra a futást (*Re-run jobs*).
- **A `dotnet publish` lépés hibára fut**: rossz a `PROJECT_PATH`, vagy a `DOTNET_VERSION` nem egyezik a projekt verziójával.
- **Nem indult el a workflow, és az *Actions* fülön meg sem jelenik**: ellenőrizd a GitHub webes felületén, a `megoldas` ágat kiválasztva, hogy a fájl pontosan a repository **gyökerében** lévő `.github/workflows/` mappában van-e (nem a projekt mappáján belül, és a mappanév `workflows`, többes számban), a kiterjesztése `.yml` vagy `.yaml`-e, és hogy valóban a `megoldas` ágra pusholtál-e.
- **Nem indult el a workflow egy későbbi pushnál**: a módosítás a `paths` szűrőn kívül esett. Módosíts egy fájlt a `MovieCatalog.Web` mappában, vagy magát a workflow fájlt, és pusholj újra.

## Beadandó

| Kép tartalma | Fájlnév (kiterjesztés nélkül) |
|--------------|-------------------------------|
| A felügyelt identitás *Federated credentials* oldala, látszik a *Subject identifier* | `f3_fedcred` |
| A Razor Web App *Access control (IAM)* » *Role assignments* oldala, látszik az identitás *Website Contributor* szerepköre | `f3_role` |
| A GitHub *Actions* fülön egy sikeres workflow-futás részletei (látszanak a lépések) | `f3_run` |
| Az Azure-ban futó Razor alkalmazás a módosított lábléccel | `f3_web` |

A workflow fájl a repository része, azt külön nem kell beadni.

## Következő feladat

Folytasd a [következő feladattal](Feladat-4.md).
