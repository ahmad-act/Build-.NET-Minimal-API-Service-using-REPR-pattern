using BookInformationService.BookInformation.Create;

namespace BookInformationService.BookInformation.Facade.Create;

public interface ICreateBookInformationBL
{
    Task<CreateResponse> CreateBookInformation(string apiVersion, BookInformation.Create.CreateRequest request);
}
