using BookInformationService.BookInformation.Facade;

namespace BookInformationService.BookInformation.Delete;

public class DeleteResponse
{
    public IResult ErrorResult { get; set; } = Results.Empty;
    public int ID { get; set; } = 1;
}

