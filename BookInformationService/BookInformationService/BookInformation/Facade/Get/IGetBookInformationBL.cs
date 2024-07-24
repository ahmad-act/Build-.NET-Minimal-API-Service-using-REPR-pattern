using BookInformationService.BookInformation.Get;

namespace BookInformationService.BookInformation.Facade.Get
{
    public interface IGetBookInformationBL
    {
        Task<GetResponse> GetBookInformation(string apiVersion, int id);
    }
}