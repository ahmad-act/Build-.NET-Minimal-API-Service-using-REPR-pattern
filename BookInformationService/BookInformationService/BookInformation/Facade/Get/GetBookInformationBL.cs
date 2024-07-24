using BookInformationService.BookInformation.Get;
using BookInformationService.Util;

namespace BookInformationService.BookInformation.Facade.Get;

public class GetBookInformationBL : IGetBookInformationBL
{
    private readonly ILogger<object> _logger;
    private readonly IGetBookInformationDL _getBookInformationDL;

    public GetBookInformationBL(ILogger<object> logger, IGetBookInformationDL getBookInformationDL)
    {
        _logger = logger;
        _getBookInformationDL = getBookInformationDL;
    }

    public async Task<GetResponse> GetBookInformation(string apiVersion, int id)
    {
        try
        {
            return apiVersion switch
            {
                "1" => await HandleApiVersion1(apiVersion, id),
                "2" => await HandleApiVersion2(apiVersion, id),
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

    private GetResponse InternalServerErrorResponse(string apiVersion, string exceptionMessage)
    {
        return new GetResponse
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
            BookInformation = null
        };
    }

    private GetResponse InvalidApiVersionResponse(string apiVersion)
    {
        return new GetResponse
        {
            ErrorResult = Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid API Version",
                detail: $"The API version '{apiVersion}' is not supported.",
                extensions: new Dictionary<string, object?>
                {
                    { "apiVersion", apiVersion }
                }),
            BookInformation = null
        };
    }

    private GetResponse DbErrorResponse(string apiVersion, string detail)
    {
        return new GetResponse
        {
            ErrorResult = Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Error during retriving",
                detail: detail,
                extensions: new Dictionary<string, object?>
                {
                    { "apiVersion", apiVersion }
                }),
            BookInformation = null
        };
    }

    private GetResponse NotFoundResponse(string apiVersion)
    {
        return new GetResponse
        {
            ErrorResult = Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Book Not Found",
                detail: "The books information was not found.",
                extensions: new Dictionary<string, object?>
                {
                { "apiVersion", apiVersion }
                }),
            BookInformation = null
        };
    }

    #endregion

    #region Version based methods

    private async Task<GetResponse> HandleApiVersion1(string apiVersion, int id)
    {
        Dictionary<string, object?> dbGetReturn = await _getBookInformationDL.GetBookInformation(id);

        string? dbGetErr = Convert.ToString(dbGetReturn["Message"]);

        if (!string.IsNullOrWhiteSpace(dbGetErr))
        {
            return DbErrorResponse(apiVersion, dbGetErr);
        }

        BookInformationModel? bookInformation = dbGetReturn["BookInformation"] as BookInformationModel;

        if (bookInformation == null)
        {
            return NotFoundResponse(apiVersion);
        }

        return new GetResponse
        {
            ErrorResult = Results.Ok(),
            BookInformation = bookInformation
        };
    }

    private async Task<GetResponse> HandleApiVersion2(string apiVersion, int id)
    {
        Dictionary<string, object?> dbReturn = await _getBookInformationDL.GetBookInformation(id);

        string? dbErr = Convert.ToString(dbReturn["Message"]);

        if (!string.IsNullOrWhiteSpace(dbErr))
        {
            return NotFoundResponse(apiVersion);
        }

        BookInformationModel? bookInformation = dbReturn["BookInformation"] as BookInformationModel;

        if (bookInformation == null)
        {
            return NotFoundResponse(apiVersion);
        }

        return new GetResponse
        {
            ErrorResult = Results.Ok(),
            BookInformation = bookInformation
        };
    }

    #endregion
}

