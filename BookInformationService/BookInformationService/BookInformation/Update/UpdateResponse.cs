using BookInformationService.BookInformation.Facade;

namespace BookInformationService.BookInformation.Update;

public class UpdateResponse
{
    public IResult ErrorResult { get; set; } = Results.Empty;
    public int ID { get; set; } = 1;
}

