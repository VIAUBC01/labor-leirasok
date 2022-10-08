using MovieCatalogApi.Entities;

namespace MovieCatalogApi.Dtos;

public class TitleQueryModel
{
    public TitleQueryModel(Title title, int[] genreIDs)
    {
        Id = title.Id;
        PrimaryTitle = title.PrimaryTitle;
        OriginalTitle = title.OriginalTitle;
        TitleType = title.TitleType;
        StartYear = title.StartYear;
        EndYear = title.EndYear;
        RuntimeMinutes = title.RuntimeMinutes;
        //AverageRating = title.AverageRating;
        //NumberOfVotes = title.NumberOfVotes;
        Genres = genreIDs;
    }

    public int Id { get; set; }
    public string PrimaryTitle { get; set; }
    public string? OriginalTitle { get; set; }
    public TitleType TitleType { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    public int? RuntimeMinutes { get; set; }
    //public float? AverageRating { get; set; }
    //public int NumberOfVotes { get; set; }
    public IEnumerable<int>? Genres { get; set; }
}