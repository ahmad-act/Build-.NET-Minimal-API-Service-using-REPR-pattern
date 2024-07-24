using BookInformationService.DatabaseContext;
using BookInformationService.Util;
using Microsoft.EntityFrameworkCore;

namespace BookInformationService.BookInformation.Facade.List;

public class ListBookInformationDL : IListBookInformationDL
{
    private readonly ILogger<object> _logger;
    private readonly SystemDbContext _dbContext;

    public ListBookInformationDL(ILogger<object> logger, SystemDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Dictionary<string, object?>> ListBookInformation()
    {
        try
        {
            List<BookInformationModel>? bookInformations = await _dbContext.DbSetBookInformation.ToListAsync();

            return new Dictionary<string, object?>
            {
                { "Message", string.Empty },
                { "BookInformations", bookInformations }
            };
        }
        catch (Exception ex)
        {
            ex.ErrorLog(_logger);

            return new Dictionary<string, object?>
            {
                { "Message", ex.Message
},
                { "BookInformations", null }
            };
        }
    }
}

