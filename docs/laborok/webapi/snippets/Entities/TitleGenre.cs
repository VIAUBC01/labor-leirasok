namespace MovieCatalogApi.Entities;

public class TitleGenre
{
    public int Id { get; set; }

    public int TitleId { get; set; }

    public Title Title { get; set; } = null!;

    public int GenreId { get; set; }

    public Genre Genre { get; set; } = null!;

}