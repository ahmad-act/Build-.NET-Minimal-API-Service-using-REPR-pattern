using BookInformationService.BookInformation.Facade.Get;

namespace BookInformationService.BookInformation.Get;

public class GetResponse
{
    public IResult ErrorResult { get; set; } = Results.Empty;
    public GetBookInformationDto? BookInformation { get; set; } = null;
}

