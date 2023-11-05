# Világjáró Android alkalmazás fejlesztése 
### Android 1 labor

Készítsen Kotlin nyelven Világjáró Android alkalmazást! Az alkalmazás az adatait a https://restcountries.com/ oldalon részletezett API hívások segítségével töltse be.
Tanulmányozza és a böngészőben próbálja ki az oldalon lévő API végpont példákat.

* https://restcountries.com/v3.1/all
* https://restcountries.com/v3.1/name/Hungary
* stb.

Érdemes először végigolvasni az összes feladatot, hogy aki minden funkciót szeretne megvalósítani, már az elején úgy tervezhessen, és ne közben kelljen módosítani az adatmodellen pl.
Az alkalmazás elkészítése során törekedjen a strukturált felépítésre, package-k szervezésére. Készítsen letisztult, ergonomikus felhasználói felületeket, figyeljen a vissza gomb megfelelő működésére, továbbá a hibák kezelésére és a felhasználó számára történő releváns visszajelzésekre.

## Értékelés
Az Alapok rész hiánytalan megvalósítása esetén sikeres (elégséges) a labor. Az Alapok részen kívül megoldott minden további részfeladat hiánytalan megvalósítása plusz egy jegyet jelent. A feladatok részben egymásra épülnek, ezért az ebből következő hiányok – feladat kihagyás esetén – szükség szerint áthidalhatók pl. dummy adatok vagy nem perzisztált adatok használatával.

## Feltöltés
* A megoldást (teljes projekt) egy ZIP file formájában kell feltölteni a Moodle portálra a jelzett határidőig.
* A ZIP-ből az \app\build\intermediates mappából minden kerüljön törlésre, kivéve az \app\build\intermediates\apk\ mappa, az abban lévő APK fájl mindenképpen maradjon benne. 
* A feltöltött ZIP file-ba kerüljön egy egyszerű, név-neptun-kóddal ellátott PDF dokumentáció, melyben szerepeljenek az egyes részfeladatok nevei, mint alfejezetek, és ezekbe kerüljön 1-2 képernyőkép az elkészült funkcióról. Szükség szerint a dokumentáció tartalmazhat szöveges kiegészítéseket, rövid magyarázatokat.

## Tippek
* Törekedjen a rövid osztályokra és függvényekre, valamint az átlátható forráskódra 
* Ügyeljen a megfelelően hierarchikus package szervezésre, a kódolás során tartsa szem előtt a Clean Code elveket.
* A felhasználói felület a lehet egyszerűbb, nem elvárás látványos felületet készíteni.
* JSON to Kotlin konverter például: https://transform.tools/json-to-kotlin
* Kezelje megfelelően a készülék elforgatása során bekövetkező életciklus változásokat.
* Perzisztencia és hálózati hívásoknál figyeljen a megfelelő szálkezelésre!.
* Gondoljon az internetkapcsolat hiányára, a távoli kiszolgálók hibáira és ezek megfelelő lekezelésére, illetve a felhasználó megfelelő tájékoztatására.
* A Google Térkép használatához, valamint a szükséges API kulcs elkészítéséhez az Android Studio új Google Maps Activity opciója jó példát mutat.
* A tesztelés Nexus 5X API 31, Android 12.0 (Google APIs) x86_64 emulátoron fog történni.
* Érdemes az előző féléves előadáson és a laborokon tanult ismereteket és projekteket alapul venni a feladatok megoldásához.
    * https://www.aut.bme.hu/Course/VIAUBB03 
    * https://github.com/bmeaut/VIAUBB03/tree/master/Mobil/El%C5%91ad%C3%A1s%20p%C3%A9ld%C3%A1k 
    * https://github.com/bmeaut/VIAUBB03/tree/master/Mobil/Labor 
* A feladat megvalósítása és beadása során önálló, egyedi munkákat várunk.

## 1. Alapok

* Állítson be az alkalmazásnak egyedi vagy saját készítésű ikont.
    * https://icon.kitchen
    * New Image/Vector Asset -> Asset Studio
* Valósítson meg navigációt egy tetszőleges megközelítéssel (NavigationDrawer, ViewPager, BottomNavigationView, Főmenü activity három gombbal stb.), amivel összesen három felület (Activity vagy Fragment) között lehet váltani az alkalmazásban. 
* Készítsen a Toolbar-on egy menüt, ami egy tetszőlegesen ideillő ikonként látható, kiválasztás esetén pedig egy Snackbar üzenetben kiírja az Ön nevét és Neptun-kódját. 
* Az első felületen készítsen RecyclerView alapú görgethető listát, amihez egy beviteli mező (pl. rögzített EditText és gomb a felület telején vagy AlertDialog FloatingActionButton-ra kattintva) segítségével lehet dinamikusan hozzáadni országokat a Retrofit osztálykönyvtár és a RestCountries API felhasználásával. Az egyes lista elemek tartalmazzák az ország angol nevét (name -> common), hárombetűs országkódját (cca3) és zászlóját. 
* A zászló képét a Glide osztálykönyvtár segítségével töltse be az API által visszaadott URL (flag -> png) felhasználásával. 
* Törekedjen a hálózati adatforgalom minimalizálására! Használja az API által biztosított szűrési lehetőséget (filter response) a szükséges adatmezőkre (előretekintve a további feladatokra is). 

```gradle
//Retrofit dependencies
val retrofit_version = "2.9.0"
implementation("com.squareup.retrofit2:retrofit:$retrofit_version")
implementation("com.squareup.retrofit2:converter-gson:$retrofit_version")

//Glide dependencies
```

```kotlin
data class CountryData (
    //...
)
```

```kotlin
interface CountryApi {

    @GET("v3.1/name/{name}")
    fun getCountryByName(@Path("name") name: String): Call<List<CountryData?>?>

}
```

NetWorkManager.kt
```kotlin
object NetworkManager {
    private val retrofit: Retrofit
    private val countryApi: CountryApi;

    private const val SERVICE_URL = "https://restcountries.com/"

    init {
        retrofit = Retrofit.Builder()
            .baseUrl(SERVICE_URL)
            .client(OkHttpClient.Builder().build())
            .addConverterFactory(GsonConverterFactory.create())
            .build()
        countryApi = retrofit.create(CountryApi::class.java)
    }

    fun getCountryByName(name: String): Call<List<CountryData?>?> {
        return countryApi.getCountryByName(name)
    }
}
```


item_country.xml
```xml
<?xml version="1.0" encoding="utf-8"?>
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:tools="http://schemas.android.com/tools"
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:orientation="horizontal"
    android:paddingBottom="8dp"
    android:paddingLeft="16dp"
    android:paddingRight="16dp"
    android:paddingTop="8dp">

    <ImageView
        android:id="@+id/ivFlag"
        android:layout_width="36dp"
        android:layout_height="36dp"
        android:layout_gravity="center_vertical" />

    <LinearLayout
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:orientation="vertical">

        <TextView
            android:id="@+id/tvCountryName"
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:paddingLeft="8dp"
            android:gravity="center_vertical"
            tools:text="Country" />

        <TextView
            android:id="@+id/tvCountryAlpha3"
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:paddingLeft="8dp"
            android:gravity="center_vertical"
            tools:text="Code" />
    </LinearLayout>

</LinearLayout>
```

```kotlin
class CountryAdapter : RecyclerView.Adapter<CountryAdapter.CountryViewHolder>() {

    private var countries: MutableList<CountryData> = ArrayList()

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int) = CountryViewHolder(
        ItemCountryBinding.inflate(
            LayoutInflater.from(parent.context), parent, false
        )
    )

    override fun onBindViewHolder(holder: CountryViewHolder, position: Int) {
        val country = countries[position]

        holder.binding.tvCountryName.text = country.name.common
        holder.binding.tvCountryAlpha3.text = country.cca3
        Glide.with(holder.binding.root.context).load(country.flags.png).into(holder.binding.ivFlag)
    }

    override fun getItemCount(): Int = countries.size

    fun addCountry(newCountry: CountryData?) {
        countries.add(newCountry!!)
        notifyItemInserted(countries.size)
    }

    inner class CountryViewHolder(val binding:  ItemCountryBinding) :
        RecyclerView.ViewHolder(binding.root) {
    }
}
```

dialog_new_country.xml
```xml
<?xml version="1.0" encoding="utf-8"?>
<!--...->
```

AddCountryDialogFragment.kt
```kotlin
class AddCountryDialogFragment : AppCompatDialogFragment() {

    private var _binding: DialogNewCountryBinding? = null
    private val binding get() = _binding!!

    private lateinit var listener: AddCountryDialogListener

    override fun onAttach(context: Context) {
        super.onAttach(context)
        try{
            listener = if (targetFragment != null){
                targetFragment as AddCountryDialogListener
            } else {
                activity as AddCountryDialogListener
            }
        } catch ( e: ClassCastException){
            throw RuntimeException(e)
        }
    }
    override fun onCreateDialog(savedInstanceState: Bundle?): Dialog {
        _binding = DialogNewCountryBinding.inflate(LayoutInflater.from(context))
        return AlertDialog.Builder(requireContext())
            .setTitle("New country")
            .setView(binding.root)
            .setPositiveButton("Add") { _, _ ->
                listener.onCountryAdded(binding.etNewCountry!!.text.toString())
            }
            .setNegativeButton("Cancel", null)
            .create()
    }

    interface AddCountryDialogListener {
        fun onCountryAdded(name: String)
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}
```



activity_list.xml
```xml
<?xml version="1.0" encoding="utf-8"?>
<androidx.coordinatorlayout.widget.CoordinatorLayout xmlns:android="http://schemas.android.com/apk/res/android"
    android:id="@+id/coordinatorContent"
    xmlns:tools="http://schemas.android.com/tools"
    android:layout_width="match_parent"
    android:layout_height="match_parent"
    tools:context=".ListActivity">

    <androidx.recyclerview.widget.RecyclerView
        xmlns:android="http://schemas.android.com/apk/res/android"
        xmlns:app="http://schemas.android.com/apk/res-auto"
        android:id="@+id/country_list"
        android:layout_width="match_parent"
        android:layout_height="match_parent"
        app:layout_behavior="@string/appbar_scrolling_view_behavior"/>

    <com.google.android.material.floatingactionbutton.FloatingActionButton
        android:id="@+id/fab"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:layout_gravity="bottom|end"
        android:layout_margin="16dp"
        android:src="@android:drawable/ic_input_add"/>

</androidx.coordinatorlayout.widget.CoordinatorLayout>
```

```kotlin
class ListActivity : AppCompatActivity(),
    AddCountryDialogFragment.AddCountryDialogListener {

    private lateinit var binding: ActivityListBinding
    private lateinit var adapter: CountryAdapter

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityListBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.fab.setOnClickListener{
            AddCountryDialogFragment()
                .show(supportFragmentManager, AddCountryDialogFragment::class.java.simpleName)
        }
        
        binding.countryList.layoutManager = LinearLayoutManager(this)
        adapter = CountryAdapter()
        onCountryAdded("Hungary")
        binding.countryList.adapter = adapter
    }

    override fun onCountryAdded(name: String) {
        NetworkManager.getCountryByName(name).enqueue(object : Callback<List<CountryData?>?> {

            override fun onResponse(
                call: Call<List<CountryData?>?>,
                response: Response<List<CountryData?>?>
            ) {
                if (response.isSuccessful) {
                    adapter.addCountry(response.body()!![0])
                } else {
                    Toast.makeText(
                        this@ListActivity,
                        "Error: " + response.message(),
                        Toast.LENGTH_SHORT
                    ).show()
                }
            }

            override fun onFailure(
                call: Call<List<CountryData?>?>,
                throwable: Throwable
            ) {
                throwable.printStackTrace()
                Toast.makeText(
                    this@ListActivity,
                    "Network request error occurred, check LOG",
                    Toast.LENGTH_SHORT
                ).show()
            }
        })
    }

}
```



 
## 2. Részletes nézet és Wikipedia

* Adott listaelem kiválasztása esetén a kiválasztott ország részletes nézete kerüljön betöltésre a második felületre, szépen elrendezve, címkékkel, mértékegységekkel. A részletes nézet tartalmazza legalább az ország nevét, fővárosát, népességét, területét.
* Adott listaelem hosszú kiválasztása esetén pedig kiválasztott ország angol Wikipedia oldala töltsön be az alapértelmezett böngésző alkalmazásban. (a szóközt a Wikipedia automatikusan redirekteli alulvonásra) Pl. https://en.wikipedia.org/wiki/Hungary, https://en.wikipedia.org/wiki/Costa_Rica 

## 3. Perzisztencia

* Az alkalmazás tegye lehetővé a listához hozzáadott országok Room alapú perzisztens elmentését, onnan való kitörlését. Ehhez bővítse ki az egyes országokhoz tartozó listaelemet egy gombbal, amivel ki lehet törölni az aktuális országot. 
* Az alkalmazás ismételt elindítása esetén ezeknek az elmentett országoknak vissza kell töltődniük. 

## 4. Térkép

Ebben a feladatban már ne használjunk Retrofit hívást. Minden a perzisztens adatbázisból kerüljön felhasználásra (kivéve az előző feladat kihagyása esetén). 

* A harmadik felületen egy térkép (Google, OpenStreetMap stb.) jelenjen meg. 
* A térképen egy választott színű jelölővel szerepeljenek a listában szereplő országok az elmentett földrajzi koordinátákkal jelölt helyükön (latlng). 
* A jelölőre kattintva írja ki az ország nevét és fővárosát (capital). 
