namespace MovieCatalogApi.Entities;

public enum TitleType
{
    Unknown, Short, Movie, TvMovie, TvSeries, TvEpisode, TvShort, TvMiniSeries, TvSpecial, Video, VideoGame, TvPilot
}

public class Title
{
    public int Id { get; set; }
    public string PrimaryTitle { get; set; }
    public string OriginalTitle { get; set; }

    public int? StartYear { get; set; }

    public int? EndYear { get; set; }

    public int? RuntimeMinutes { get; set; }

    public TitleType TitleType { get; set; }

    public ICollection<TitleGenre> TitleGenres { get; set; }
        = new List<TitleGenre>();
    public override string ToString() => $"Title {Id}: {TitleType} - {PrimaryTitle} ({OriginalTitle}, [{StartYear?.ToString() ?? "?"}{(EndYear != null ? $"-{EndYear}" : "")}]{($"<{RuntimeMinutes} min>" )}{(TitleGenres.Any() ? $" - {string.Join(", ", TitleGenres.Select(g => $"{g.Genre.Name}"))}" : string.Empty)}";
    public Title(string primaryTitle, string originalTitle)
    {
        PrimaryTitle = primaryTitle;
        OriginalTitle = originalTitle;
    }

}