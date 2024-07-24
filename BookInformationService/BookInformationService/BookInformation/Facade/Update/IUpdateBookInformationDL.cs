
namespace BookInformationService.BookInformation.Facade.Update
{
    public interface IUpdateBookInformationDL
    {
        Task<Dictionary<string, object?>> GetBookInformation(int id);
        Task<Dictionary<string, object?>> UpdateBookInformation(BookInformationModel bookInformation);
    }
}