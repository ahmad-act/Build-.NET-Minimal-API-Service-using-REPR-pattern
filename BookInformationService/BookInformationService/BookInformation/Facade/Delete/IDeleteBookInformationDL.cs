
namespace BookInformationService.BookInformation.Facade.Delete
{
    public interface IDeleteBookInformationDL
    {
        Task<Dictionary<string, object?>> DeleteBookInformation(BookInformationModel bookInformation);
        Task<Dictionary<string, object?>> GetBookInformation(int id);
    }
}