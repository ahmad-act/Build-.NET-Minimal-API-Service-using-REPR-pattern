using BookInformationService.BookInformation.List;

namespace BookInformationService.BookInformation.Facade.List
{
    public interface IListBookInformationBL
    {
        Task<ListResponse> ListBookInformation(string apiVersion);
    }
}