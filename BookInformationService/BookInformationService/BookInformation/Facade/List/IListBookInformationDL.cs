
namespace BookInformationService.BookInformation.Facade.List
{
    public interface IListBookInformationDL
    {
        Task<Dictionary<string, object?>> ListBookInformation();
    }
}