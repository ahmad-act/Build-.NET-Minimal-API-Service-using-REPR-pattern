using BookInformationService.BookInformation.Get;
using BookInformationService.BookInformation.List;
using BookInformationService.Util;

namespace BookInformationService.BookInformation.Facade.List;

public class ListBookInformationBL : IListBookInformationBL
{
    private readonly ILogger<object> _logger;
    private readonly IListBookInformationDL _listBookInformationDL;

    public ListBookInformationBL(ILogger<object> logger, IListBookInformationDL listBookInformationDL)
    {
        _logger = logger;
        _listBookInformationDL = listBookInformationDL;
    }

    public async Task<ListResponse> ListBookInformation(string apiVersion)
    {
        try
        {
            return apiVersion switch
            {
                "1" => await HandleApiVersion1(apiVersion),
                "2" => await HandleApiVersion2(apiVersion),
                _ => InvalidApiVersionResponse(apiVersion)
            };
        }
        catch (Exception ex)
        {
            ex.ErrorLog(_logger);
            return InternalServerErrorResponse(apiVersion, ex.Message);
        }
    }


    #region Response methods

    private ListResponse InternalServerErrorResponse(string apiVersion, string exceptionMessage)
    {
        return new ListResponse
        {
            ErrorResult = Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error",
                detail: $"The API version '{apiVersion}' is not supported.",
                extensions: new Dictionary<string, object?>
                {
                    { "apiVersion", apiVersion },
                    { "exceptionMessage", exceptionMessage }
                }),
            BookInformations = [] // Return an empty list
        };
    }

    private ListResponse InvalidApiVersionResponse(string apiVersion)
    {
        return new ListResponse
        {
            ErrorResult = Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid API Version",
                detail: $"The API version '{apiVersion}' is not supported.",
                extensions: new Dictionary<string, object?>
                {
                    { "apiVersion", apiVersion }
                }),
            BookInformations = [] // Return an empty list
        };
    }

    private ListResponse DbErrorResponse(string apiVersion, string detail)
    {
        return new ListResponse
        {
            ErrorResult = Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Error during retriving",
                detail: detail,
                extensions: new Dictionary<string, object?>
                {
                    { "apiVersion", apiVersion }
                }),
            BookInformations = []
        };
    }

    private ListResponse NotFoundResponse(string apiVersion)
    {
        return new ListResponse
        {
            ErrorResult = Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Book Not Found",
                detail: "The books information was not found.",
                extensions: new Dictionary<string, object?>
                {
                { "apiVersion", apiVersion }
                }),
            BookInformations = [] // Return an empty list
        };
    }

    #endregion

    #region Version based methods

    private async Task<ListResponse> HandleApiVersion1(string apiVersion)
    {
        Dictionary<string, object?> dbReturn = await _listBookInformationDL.ListBookInformation();

        string? dbErr = Convert.ToString(dbReturn["Message"]);

        if (!string.IsNullOrWhiteSpace(dbErr))
        {
            return DbErrorResponse(apiVersion, dbErr);
        }

        List<BookInformationModel>? bookInformations = dbReturn["BookInformations"] as List<BookInformationModel>;

        if (bookInformations == null || bookInformations.Count == 0)
        {
            return NotFoundResponse(apiVersion);
        }

        return new ListResponse
        {
            ErrorResult = Results.Ok(),
            BookInformations = await bookInformations.ToListBookInformationDtoListAsync()
        };
    }

    private async Task<ListResponse> HandleApiVersion2(string apiVersion)
    {
        Dictionary<string, object?> dbReturn = await _listBookInformationDL.ListBookInformation();

        string? dbErr = Convert.ToString(dbReturn["Message"]);

        if (!string.IsNullOrWhiteSpace(dbErr))
        {
            return DbErrorResponse(apiVersion, dbErr);
        }

        List<BookInformationModel>? bookInformations = dbReturn["BookInformations"] as List<BookInformationModel>;

        if (bookInformations == null || bookInformations.Count == 0)
        {
            return NotFoundResponse(apiVersion);
        }

        return new ListResponse
        {
            ErrorResult = Results.Ok(),
            BookInformations = await bookInformations.ToListBookInformationDtoListAsync()
        };
    }

    #endregion
}

