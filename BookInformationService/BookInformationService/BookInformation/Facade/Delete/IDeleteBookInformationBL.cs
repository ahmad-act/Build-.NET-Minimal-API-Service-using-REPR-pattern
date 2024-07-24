using BookInformationService.BookInformation.Delete;

namespace BookInformationService.BookInformation.Facade.Delete
{
    public interface IDeleteBookInformationBL
    {
        Task<DeleteResponse> DeleteBookInformation(string apiVersion, int id);
    }
}