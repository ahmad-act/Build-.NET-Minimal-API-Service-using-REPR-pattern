
namespace BookInformationService.BookInformation.Facade.Create
{
    public interface ICreateBookInformationDL
    {
        Task<Dictionary<string, object?>> CreateBookInformation(BookInformationModel bookInformation);
    }
}