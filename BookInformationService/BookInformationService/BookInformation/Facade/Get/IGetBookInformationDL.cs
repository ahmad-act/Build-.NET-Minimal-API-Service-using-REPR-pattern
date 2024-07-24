
namespace BookInformationService.BookInformation.Facade.Get
{
    public interface IGetBookInformationDL
    {
        Task<Dictionary<string, object?>> GetBookInformation(int id);
    }
}