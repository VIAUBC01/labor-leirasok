using MovieCatalogApi.Entities;

namespace MovieCatalogApi.Dtos;

public class TitleInsertUpdateModel
{
    public string? PrimaryTitle { get; set; }
    public string? OriginalTitle { get; set; }
    public TitleType? TitleType { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    public int? RuntimeMinutes { get; set; }

    public Title ToTitleEntity()
    {
        if (OriginalTitle == null || TitleType == null || PrimaryTitle ==null)
            throw new InvalidOperationException();

        return new Title(PrimaryTitle, OriginalTitle)
        {
            TitleType = TitleType.Value,
            StartYear = StartYear,
            EndYear = EndYear,
            RuntimeMinutes = RuntimeMinutes
        };
    }

}