namespace MovieCatalogApi.Entities;

public class Genre
{

    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<TitleGenre> TitleGenres { get; set; }
        = new List<TitleGenre>();

    public override string ToString() => $"Genre {Name} ({Id})";

    public Genre(string name)
    {
        Name = name;
    }

}