using BookInformationService.BookInformation.Update;

namespace BookInformationService.BookInformation.Facade.Update
{
    public interface IUpdateBookInformationBL
    {
        Task<UpdateResponse> UpdateBookInformation(string apiVersion, int id, UpdateRequest request);
    }
}