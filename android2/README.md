# Aknakereső Android-alkalmazás fejlesztése (Android 2. labor)

A labor során Android platformra kell elkészíteni
egy [aknakereső](http://en.wikipedia.org/wiki/Minesweeper_(video_game)) játékot Kotlin nyelven:

<img src="./assets/minesweeper.png" width="400" style="vertical-align: middle" alt="An image of a smartphone displaying a MineSweeper game app. The grid has several squares marked with numbers, indicating the count of adjacent mines. Some squares are marked with red crosses, while one square is highlighted in red. At the top, the text reads 'MineSweeper' with '11 mp' displayed underneath. A 'TRY' button is visible below the grid.">

A labor során az idő rövidsége miatt elegendő, ha az alkalmazás az alábbi megkötésekkel üzemel:

* 5x5 játéktér
* 3 akna
* nem kell teljes feltöltő logikát írni – elég, ha be van égetve egy fix elrendezés
* a felületen elhelyezhető egy `ToggleButton` vagy checkbox az akció kiválasztásához:
    * zászló lehelyezése
    * mező kipróbálása
* ha a kipróbált mező...
    * ...akna, a játék véget ér
    * ...nem akna, akkor megjelenik egy szám, ami a szomszédos aknák számát jelzi

Érdemes először végigolvasni az összes feladatot, hogy aki minden funkciót szeretne megvalósítani, már az elején úgy
tervezhessen, és ne közben kelljen módosítani az adatmodellen.
Az alkalmazás elkészítése során törekedjen a strukturált felépítésre, package-ek szervezésére! Készítsen letisztult,
ergonomikus felhasználói felületeket, figyeljen a vissza gomb megfelelő működésére, továbbá a hibák kezelésére és a
felhasználó számára történő releváns visszajelzésekre!

## Értékelés

Az _Alapok_ rész hiánytalan megvalósítása esetén sikeres (elégséges) a labor. Az _Alapok_ részen kívül megoldott minden
további részfeladat hiánytalan megvalósítása plusz egy jegyet jelent. A feladatok részben egymásra épülnek, ezért az
ebből következő hiányok – feladatkihagyás esetén – szükség szerint áthidalhatóak pl. dummy adatok használatával.

## Feltöltés

* A megoldást (teljes projekt) egy ZIP fájl formájában kell feltölteni a Moodle portálra a jelzett határidőig.
* A ZIP-ből törölje az `app\build\intermediates` mappát. Az `app\build\outputs\apk\debug` mappában lévő `.apk` fájl
  viszont mindenképpen legyen benne.
* A ZIP-be kerüljön egy egyszerű, névvvel és Neptun-kóddal ellátott PDF-dokumentáció, melyben szerepeljenek az egyes
  részfeladatok nevei, mint alfejezetek, és ezekbe kerüljön 1-2 képernyőkép, valamint némi szöveges leírás az elkészült
  funkcióról.

## Tippek

* Törekedjen a rövid osztályokra és függvényekre, valamint az átlátható forráskódra!
* Ügyeljen a megfelelően hierarchikus packageszervezésre, a kódolás során tartsa szem előtt a Clean Code-elveket!
* Kezelje megfelelően a készülék elforgatása során bekövetkező életciklusváltozásokat!
* Perzisztenciánál figyeljen a megfelelő szálkezelésre!
* Önálló, egyedi munkákat várunk.

## 1. – Alapok

* Állítson be az alkalmazásnak egyedi vagy saját készítésű ikont.
* Az alkalmazás indulása után egy főmenü jelenjen meg két gombbal: _New game_ és _Results_.
* A _New game_ gomb megnyomására egy új `Activity`-n jelenjen meg a játéktér, amin érintés esemény hatására változnak az
  egyes mezők. Mivel itt több elem együttes működésére van szükség, a lépéseket és mintakódokat biztosítunk.
    * A játék állapotának nyilvántartásához szükségünk lesz egy `Tile` osztályra és egy singleton `Model` osztályra:

```kotlin
data class Tile(
    var type: TileType = TileType.FIELD_TYPE_NORMAL,
    var minesAround: Byte = 0,  // -1 means mine
) {
    enum class TileType {
        FIELD_TYPE_NORMAL,
        FIELD_TYPE_REVEALED,
        FIELD_TYPE_FLAGGED
    }
}
```

```kotlin
object MineSweeperModel {

    var fieldMatrix: Array<Array<Tile>> = arrayOf(
        arrayOf(
            Tile(Tile.TileType.FIELD_TYPE_NORMAL, 1),
            Tile(Tile.TileType.FIELD_TYPE_NORMAL, 1),
            // TODO
        ),
        arrayOf(
            Tile(Tile.TileType.FIELD_TYPE_NORMAL, 2),
            Tile(Tile.TileType.FIELD_TYPE_FLAGGED, -1),
            // TODO
        ),
        // TODO
    )

    fun initGameArea() {
        // TODO
    }

    fun getFieldContent(x: Int, y: Int): Tile {
        return fieldMatrix[x][y]
    }
}
```

* A képernyőn való megjelenítéshez szükségünk lesz egy saját `View`-ra, aminek felül kell írni az alábbi függvényeit:
    * `onDraw`: itt történik a view kirajzolása. Ezt jelenleg két részre bontottuk: a játéktér és a játékállapot
      kirajzolása.
    * `onMeasure`: ahhoz szükséges, hogy megállapítsuk a megfelelő dimenziókat egy maximális négyzet rajzolásához.
    * `onTouchEvent`: az érintésesemények lekezelése. Itt kell az felhasználói akció és a meglévő modell alapján
      változtatni a modellt. A végén az `invalidate()` hatására rajzolódik újra a view.

```kotlin
import android.content.Context
import android.graphics.Canvas
import android.graphics.Color
import android.graphics.Paint
import android.util.AttributeSet
import android.view.MotionEvent
import android.view.View
import hu.bme.aut.android.minesweeper.data.MineSweeperModel
import hu.bme.aut.android.minesweeper.data.Tile
import kotlin.math.min

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

        drawGameArea(canvas)
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
        // TODO

        // four vertical lines
        canvas.drawLine(widthFloat / 5, 0F, widthFloat / 5, heightFloat, paintLine)
        canvas.drawLine(2 * widthFloat / 5, 0F, 2 * widthFloat / 5, heightFloat, paintLine)
        // TODO
    }

    private fun drawGameState(canvas: Canvas) {
        for (i in 0 until 5) {
            for (j in 0 until 5) {
                if (MineSweeperModel.getFieldContent(i, j).type == Tile.TileType.FIELD_TYPE_FLAGGED) {
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
                // TODO
            }
        }
    }

    override fun onMeasure(widthMeasureSpec: Int, heightMeasureSpec: Int) {
        val w = MeasureSpec.getSize(widthMeasureSpec)
        val h = MeasureSpec.getSize(heightMeasureSpec)

        val d: Int = when {
            w == 0 -> {
                h
            }

            h == 0 -> {
                w
            }

            else -> {
                min(w, h)
            }
        }

        setMeasuredDimension(d, d)
    }

    override fun onTouchEvent(event: MotionEvent?): Boolean {
        when (event?.action) {
            MotionEvent.ACTION_DOWN -> {
                val tX: Int = (event.x / (width / 5)).toInt()
                val tY: Int = (event.y / (height / 5)).toInt()
                if (tX < 5 && tY < 5 && MineSweeperModel.getFieldContent(
                        tX,
                        tY
                    ).type == Tile.TileType.FIELD_TYPE_NORMAL
                ) {
                    // TODO
                    invalidate()
                }
                return true
            }
            else -> return super.onTouchEvent(event)
        }
    }
}
```

* A saját view ezek után teljes package-hivatkozással már használható is a felületen:

```xml

<hu.bme.aut.android.minesweeper.view.MineSweeperView
        android:id="@+id/mineSweeperView"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        .../>
```

## 2. – Bombák, zászlók, számok megjelenítése

* Ha már a játéktér megjelenik a felületen és össze van kötve a modellel, készítse el és tegye használhatóvá a játék
  különbözö elemeit (grafikusan mindegyik máshogy jelenjen meg):
    * A megérintett mező színe változzon meg, hogy egyértelmű legyen: már fel van fedve.
    * Legyen egyértelműen jelezve, ha a megérintett mező alatt bomba van.
    * Jelenjen meg, ha a megérintett mező alatt szám van.
    * Legyen lehetőség zászlók letételére és felvételére a még meg nem nézett mezőkön. (Ez bármilyen módszerrel
      történhet. Egy lehetséges mód egy `ToggleButton` elhelyezése a felületen, amivel a következő akció állítható.)

## 3. – Játék vége és időmérés

* A felületre helyezzen el egy futó órát, ami a megoldás idejét számolja.
* Készítse el a játék befejezésének logikáját, és a befejezést jelezze dialógusablakban:
    * Ha egy bomba felfedésre kerül, a játék vereséggel véget ér.
    * Ha a bombák kivételével az összes mező felfedésre került, a játék győzelemmel ér véget. Ekkor a dialógusablakban
      jelenjen meg a felhasznált idő is.

## 4. – Perzisztencia

* Ha a játék győzelemmel ér véget, kérjen be egy felhasználónevet, és ezt az idővel együtt mentse el perzisztensen.
* Valósítsa meg a _Results_ gomb funkcionalitását! Nyíljon meg egy új felület, amin egy listában láthatóak a nevek és az
  elért időeredmények.