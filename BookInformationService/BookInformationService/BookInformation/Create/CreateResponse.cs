
namespace BookInformationService.BookInformation.Create;

public class CreateResponse
{
    public IResult ErrorResult { get; set; } = Results.Empty;
    public int ID { get; set; } = -1;
}

