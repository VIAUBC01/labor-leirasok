# Aknakereső Android alkalmazás fejlesztése 
### Android 2 labor

A labor során Android platformra kell elkészíteni egy Aknakereső játékot Kotlin nyelven:

[http://en.wikipedia.org/wiki/Minesweeper_(video_game)](http://en.wikipedia.org/wiki/Minesweeper_(video_game))

![MineSweeper](/assets/minesweeper.jpg)

A labor során az idő rovidsége miatt elegendő, ha az alkalmazás az alábbi megkötésekkel üzemel:
* 5x5 játéktér elég
* 3 akna elég
* nem kell teljes feltöltő logikát írni, elég ha be van égetve egy fix
* a felületen elhelyezhető egy ToggleButton vagy checkbox az akció kiválasztásához:
    * zászló lehelyezése
    * mező kipróbálása
* Ha a kipróbált mező:
    * akna, a játék véget ér
    * nem akna, akkor megjelenik egy szám, ami a szomszédos aknák számát jelzi

Érdemes először végigolvasni az összes feladatot, hogy aki minden funkciót szeretne megvalósítani, már az elején úgy tervezhessen, és ne közben kelljen módosítani az adatmodellen pl.
Az alkalmazás elkészítése során törekedjen a strukturált felépítésre, package-k szervezésére. Készítsen letisztult, ergonomikus felhasználói felületeket, figyeljen a vissza gomb megfelelő működésére, továbbá a hibák kezelésére és a felhasználó számára történő releváns visszajelzésekre.

## Értékelés
Az Alapok rész hiánytalan megvalósítása esetén sikeres (elégséges) a labor. Az Alapok részen kívül megoldott minden további részfeladat hiánytalan megvalósítása plusz egy jegyet jelent. A feladatok részben egymásra épülnek, ezért az ebből következő hiányok – feladat kihagyás esetén – szükség szerint áthidalhatók pl. dummy adatok használatával.

## Feltöltés
* A megoldást (teljes projekt) egy ZIP file formájában kell feltölteni a https://www.aut.bme.hu/Members/MyResults.aspx oldalon.
* A ZIP-ből az \app\build\intermediates mappa kerüljön törlésre, ugyanakkor az \app\build\outputs\apk\debug\ mappában lévő APK fájl mindenképpen legyen benne. 
* A feltöltött ZIP file-ba kerüljön egy egyszerű, név-neptun-kóddal ellátott PDF dokumentáció, melyben szerepeljenek az egyes részfeladatok nevei, mint alfejezetek, és ezekbe kerüljön 1-2 képernyőkép, valamint némi szöveges leírás az elkészült funkcióról.

## Tippek
* Törekedjen a rövid osztályokra és függvényekre, valamint az átlátható forráskódra 
* Ügyeljen a megfelelően hierarchikus package szervezésre, a kódolás során tartsa szem előtt a Clean Code elveket.
* Kezelje megfelelően a készülék elforgatása során bekövetkező életciklus változásokat.
* Perzisztenciánál figyeljen a megfelelő szálkezelésre!.
* A feladat megvalósítása és beadása során önálló, egyedi munkákat várunk.

## 1. Alapok

* Állítson be az alkalmazásnak egyedi vagy saját készítésű ikont.
* Az alkalmazás indulása után egy főmenü jelenjen meg két gombbal: "Új játék" és "Eredmények"
* Az "Új játék" gomb megnyomására egy új Activity-n jelenjen meg a játéktér, amin érintés esemény hatására változnak az egyes mezők. Mivel itt több elem együttes működésére van szükség, a lépéseket és mintakódokat biztosítunk:
        * A játék állapotának nyilvántartásához szükségünk lesz egy mező osztályra és egy singleton model osztályra:
```kotlin
data class Field(
    var type: Int,
    var minesAround: Int,
    var isFlagged: Boolean,
    var wasClicked: Boolean
)
```

```kotlin
object MineSweeperModel {

    var fieldMatrix: Array<Array<Field>> = arrayOf(
        arrayOf(
            Field(1, 5, true, true),
            Field(2, 6, false, false)
        ),
        arrayOf(
            Field(3, 7, true, true), Field(
                4, 8, false, false)))
		...

    fun initGameArea() {
	...
    }

    fun getFieldContent(x: Int, y: Int): Field {
        return fieldMatrix[x][y]
    }
```

    * A képernyőn való megjelenítéshez szükségünk lesz egy saját View-ra, minek felül kell írni az alábbi függvényeit:
        * onDraw: itt történik a view kirajzolása. Ezt jelenleg két részre bontottuk: a játéktér és a játékállapot kirajzolása.
        * onMeasure: ahhoz szükséges, hogy megállapítsuk a megfelelő dimenziókat egy maximális négyzet rajzolásához.
        * onTouchEvent: az érintés események lekezelése. Itt kell az felhasználói akció és a meglévő modell alapján változtatni a modellt. A végén az *invalidate()* hatására rajzolódik újra a view.
```kotlin
class MineSweeperView : View {

    private val paintBg = Paint()
    private val paintLine = Paint()

    constructor(context: Context?) : super(context)
    constructor(context: Context?, attrs: AttributeSet?) : super(context, attrs)

    init {
        paintBg.color = Color.BLACK
        paintBg.style = Paint.Style.FILL

        paintLine.color = Color.WHITE
        paintLine.style = Paint.Style.STROKE
        paintLine.strokeWidth = 5F
    }

    override fun onDraw(canvas: Canvas) {
        canvas.drawRect(0F, 0F, width.toFloat(), height.toFloat(), paintBg)

        drawPlayer(canvas)
        drawGameState(canvas)

    }

    private fun drawGameArea(canvas: Canvas) {
        val widthFloat: Float = width.toFloat()
        val heightFloat: Float = height.toFloat()

        // border
        canvas.drawRect(0F, 0F, widthFloat, heightFloat, paintLine)

        // four horizontal lines
        canvas.drawLine(0F, heightFloat / 5, widthFloat, heightFloat / 5, paintLine)
        canvas.drawLine(0F, 2 * heightFloat / 5, widthFloat, 2 * heightFloat / 5, paintLine)
        ...

        // four vertical lines
        canvas.drawLine(widthFloat / 5, 0F, widthFloat / 5, heightFloat, paintLine)
        canvas.drawLine(2 * widthFloat / 5, 0F, 2 * widthFloat / 5, heightFloat, paintLine)
        ...
    }

    private fun drawGameState(canvas: Canvas) {
        for (i in 0 until 5) {
            for (j in 0 until 5) {
                if (MineSweeperModel.getFieldContent(i, j).isFlagged) {
                    canvas.drawLine(
                        (i * width / 5).toFloat(),
                        (j * height / 5).toFloat(),
                        ((i + 1) * width / 5).toFloat(),
                        ((j + 1) * height / 5).toFloat(),
                        paintLine
                    )
                    canvas.drawLine(
                        ((i + 1) * width / 5).toFloat(),
                        (j * height / 5).toFloat(),
                        (i * width / 5).toFloat(),
                        ((j + 1) * height / 5).toFloat(),
                        paintLine
                    )
                }
				...
            }
        }
    }

    override fun onMeasure(widthMeasureSpec: Int, heightMeasureSpec: Int) {
        val w = View.MeasureSpec.getSize(widthMeasureSpec)
        val h = View.MeasureSpec.getSize(heightMeasureSpec)
        val d: Int

        when {
            w == 0 -> {
                d = h
            }
            h == 0 -> {
                d = w
            }
            else -> {
                d = min(w, h)
            }
        }

        setMeasuredDimension(d, d)
    }

    override fun onTouchEvent(event: MotionEvent?): Boolean {
        when (event?.action) {
            MotionEvent.ACTION_DOWN -> {
                val tX: Int = (event.x / (width / 5)).toInt()
                val tY: Int = (event.y / (height / 5)).toInt()
                if (tX < 5 && tY < 5 && !MineSweeperModel.getFieldContent(tX, tY).wasClicked) {
                    ...
                    invalidate()
                }
                return true
            }
            else -> return super.onTouchEvent(event)
        }
    }
```
    * A saját view ezek után teljes package hivatkozással már használható is a felületen: 
```xml
<hu.bme.aut.android.minesweeper.view.MineSweeperView
    android:id="@+id/mineSweeperView"
    android:layout_width="wrap_content"
    android:layout_height="wrap_content"
    .../>
```

## 2. Bombák, zászlók, számok megjelenítése

* Ha már a játéktér megjelenik a felületen és össze van kötve a modellel, készítse el és tegye használhatóvá a játék különbözö elemeit (grafikusan mindegyik máshogy jelenjen meg):
    * A megérintett megző színe változzon meg, hogy egyértelmű legyen, hogy az már fel van fedve.
    * Ha a megérintett mező alatt bomba van, az legyen egyértelműen jelezve.
    * Ha a megérintett mező alatt szám van, az jelenjen meg.
    * Legyen lehetőség zászlók letételére és felvételére még nem megnézett mezőn. (Ez bármilyen módszerrel történhet, egy lehetséges mód egy ToggleButton elhelyezése a felületen, amivel a következő akció állítható)

## 3. Játék vége és időmérés

* A felületre helyezzen el egy futó órát, ami a megoldás idejét számolja.
* Készítse el a játék befejezésének logikáját, és a befejezést jelezze dialógusablakban:
    * Ha egy bomba felfedésre kerül, a játék vereséggel véget ér.
    * Ha a bombák kivételével az összes mező felfedésre került, a játék győzelemmel ér véget. Ekkor a dialógusablakban jelenjen meg a felhasznált idő is.

## 4. Perzisztencia

* Ha a játék győzelemmel ér véget, kérjen be egy felhasználónevet, és ezt az idővel együtt mentse el perzisztensen.
* Valósítsa meg az "Eredmények" gomb funkcionalitását! Nyíljon meg egy új felület, amin egy listában láthatóak a nevek és az elért időeredmények.