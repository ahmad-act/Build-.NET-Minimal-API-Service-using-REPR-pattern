using BookInformationService.DatabaseContext;
using BookInformationService.Util;
using Microsoft.EntityFrameworkCore;

namespace BookInformationService.BookInformation.Facade.Get;

public class GetBookInformationDL : IGetBookInformationDL
{
    private readonly ILogger<object> _logger;
    private readonly SystemDbContext _dbContext;

    public GetBookInformationDL(ILogger<object> logger, SystemDbContext dbContext)
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
}

