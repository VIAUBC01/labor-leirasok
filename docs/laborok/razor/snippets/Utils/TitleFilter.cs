using MovieCatalogApi.Entities;

namespace MovieCatalog.Web.Utils;

/// <summary>
/// Szűrési feltételeket tároló objektum. Minden érték opcionális.
/// </summary>
public class TitleFilter
{
    /// <summary>
    /// A mű címére való tartalmazást vizsgáló szűrő.
    /// </summary>
    public string? TitleContains { get; set; }

    /// <summary>
    /// A lehetséges <see cref="TitleType"/> típusokat tartalmazó lista.
    /// </summary>
    public IReadOnlyCollection<TitleType>? TitleTypes { get; set; }

    /// <summary>
    /// A kiadási, vagy sorozat esetén nyitó évad évszámára vizsgáló szűrő, minimum érték.
    /// </summary>
    public int? StartYearMin { get; set; }

    /// <summary>
    /// A kiadási, vagy sorozat esetén nyitó évad évszámára vizsgáló szűrő, maximum érték.
    /// </summary>
    public int? StartYearMax { get; set; }

    /// <summary>
    /// Sorozatok esetén a záró évad évszámára vizsgáló szűrő, minimum érték.
    /// </summary>
    public int? EndYearMin { get; set; }

    /// <summary>
    /// Sorozatok esetén a záró évad évszámára vizsgáló szűrő, maximum érték.
    /// </summary>
    public int? EndYearMax { get; set; }

    /// <summary>
    /// A futási időre vizsgáló szűrő, minimum érték.
    /// </summary>
    public int? RuntimeMinutesMin { get; set; }

    /// <summary>
    /// A futási időre vizsgáló szűrő, maximum érték.
    /// </summary>
    public int? RuntimeMinutesMax { get; set; }

    /// <summary>
    /// A műfajokra vizsgáló szűrő. Inkluzív, tehát bármely műfaji egyezésre vizsgál.
    /// </summary>
    public List<string>? Genres { get; set; }

    public static TitleFilter Empty { get; } = new();
}
