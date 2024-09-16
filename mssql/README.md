# Microsoft SQL Server programozása

A labor során a Microsoft SQL Server programozási lehetőségeit gyakoroljuk komplexebb feladatokon keresztül.

## Előfeltételek, felkészülés

A labor elvégzéséhez szükséges eszközök:

- Windows, Linux vagy macOS
- Microsoft SQL Server
  - Az Express változat ingyenesen használható, de a Visual Studio mellett feltelepülő _localdb_ is megfelelő.
  - Van [Linuxos változata](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-setup) is.
  - MacOS-en [Dockerrel futtatható](https://devblogs.microsoft.com/azure-sql/development-with-sql-in-containers-on-macos/). Apple siliconon csak akkor működik, ha a Rosetta is telepítve van.
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms), vagy kipróbálható a platformfüggetlen [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download) is.
  - Azure Data Studióban 3.12-es Python verzió felett [nem lehet deployolni](https://github.com/microsoft/azuredatastudio/issues/25327) SQL Server container image-t.
- Az adatbázist létrehozó script: [mssql.sql](https://raw.githubusercontent.com/BMEVIAUBB04/gyakorlat-mssql/master/mssql.sql)

A labor elvégzéséhez használható segédanyagok és felkészülési anyagok:

- Microsoft SQL Server használata: [leírás](https://github.com/BMEVIAUBB04/gyakorlat-mssql/blob/master/mssql-hasznalat.md) és [videó](https://web.microsoftstream.com/video/e3a83d16-b5c4-4fe9-b027-703347951621)
- Az adatbázis [sémájának leírása](https://github.com/BMEVIAUBB04/gyakorlat-mssql/blob/master/sema.md)
- Microsoft SQL Server programozási lehetőségei és az SQL nyelv: lásd a _Háttéralkalmazások_ c. tárgy anyagait

### Adatbázis létrehozása

1. Kapcsolódj Microsoft SQL Serverhez SQL Server Management Studio Segítségével. Indítsd el az alkalmazást, és az alábbi adatokkal kapcsolódj.

   - Server name: `(localdb)\mssqllocaldb` vagy `.\sqlexpress` (ezzel egyenértékű: `localhost\sqlexpress`)
   - Authentication: localdb esetén `Windows authentication`, sqlexpress esetén `SQL Server authentication`

1. Hozz létre egy új adatbázist (ha még nem létezik). Az adatbázis neve legyen a Neptun-kódod: _Object Explorer_-ben Databases-en jobb kattintás, és _New Database..._.

1. Hozd létre a mintaadatbázist a [generálóscript](https://raw.githubusercontent.com/BMEVIAUBB04/gyakorlat-mssql/master/mssql.sql) lefuttatásával. Nyiss egy új _Query_ ablakot, másold be a script tartalmát, és futtasd le. Ügyelj az eszköztáron levő legördülő menüben a megfelelő adatbázis kiválasztására.

   ![Adatbázis kiválasztása](images/sql-management-database-dropdown.png)

1. Ellenőrizd, hogy létrejöttek-e a táblák. Ha a _Tables_ mappa ki volt már nyitva, akkor frissíteni kell.

   ![Adatbázis kiválasztása](images/sql-managment-tablak.png).

## Beadandó

A labor elvégzése után az alábbi tartalmat kérjük beadni a laborvezető által meghatározott módon:

- A megírt SQL-scriptek (szöveges fájlként),
- Minden feladatról egy képernyőkép, ami mutatja a feladatban megvalósított funkció eredményét. Pl. egy tárolt eljárás esetén a lefuttatása során kiírt üzenetek, egy trigger esetén a trigger tesztelésének eredménye mutatva a szükség szerint változott sorokat stb.

## Értékelés

A laborban négy feladatrész van (az A és B feladatrészek kettőnek számítanak). Jeles osztályzat az összes feladatrész elvégzésével kapható. Minden hiányzó vagy hiányos feladatrész mínusz egy jegy.

## Feladatok

Összesen 3 feladat van. [Itt kezdd](Feladat-1.md) az első feladattal.
