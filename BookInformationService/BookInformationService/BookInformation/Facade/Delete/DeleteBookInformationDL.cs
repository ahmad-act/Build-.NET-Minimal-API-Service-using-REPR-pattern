using BookInformationService.DatabaseContext;
using BookInformationService.Util;

namespace BookInformationService.BookInformation.Facade.Delete;

public class DeleteBookInformationDL : IDeleteBookInformationDL
{
    private readonly ILogger<object> _logger;
    private readonly SystemDbContext _dbContext;

    public DeleteBookInformationDL(ILogger<object> logger, SystemDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Dictionary<string, object?>> GetBookInformation(int id)
    {
        try
        {
            BookInformationModel? bookInformation = await _dbContext.DbSetBookInformation.FindAsync(id);

            return new Dictionary<string, object?>
            {
                { "Message", string.Empty },
                { "BookInformation", bookInformation }
            };
        }
        catch (Exception ex)
        {
            ex.ErrorLog(_logger);

            return new Dictionary<string, object?>
            {
                { "Message", ex.Message },
                { "BookInformation", null }
            };
        }
    }

    public async Task<Dictionary<string, object?>> DeleteBookInformation(BookInformationModel bookInformation)
    {
        try
        {
            _dbContext.DbSetBookInformation.Remove(bookInformation);
            int result = await _dbContext.SaveChangesAsync();

            return new Dictionary<string, object?>
            {
                { "Message", string.Empty },
                { "ID", bookInformation.Id }
            };
        }
        catch (Exception ex)
        {
            ex.ErrorLog(_logger);

            return new Dictionary<string, object?>
            {
                { "Message", ex.Message},
                { "ID", 0 }
            };
        }
    }
}

