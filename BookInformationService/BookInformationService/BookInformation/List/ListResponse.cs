using BookInformationService.BookInformation.Facade.List;

namespace BookInformationService.BookInformation.List;

public class ListResponse
{
    public IResult ErrorResult { get; set; } = Results.Empty;
    public List<ListBookInformationDto> BookInformations { get; set; } = [];
}

