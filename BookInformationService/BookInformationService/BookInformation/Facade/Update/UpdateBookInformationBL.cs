using BookInformationService.BookInformation.Update;
using BookInformationService.Util;

namespace BookInformationService.BookInformation.Facade.Update;

public class UpdateBookInformationBL : IUpdateBookInformationBL
{
    private readonly ILogger<object> _logger;
    private readonly IUpdateBookInformationDL _updateBookInformationDL;

    public UpdateBookInformationBL(ILogger<object> logger, IUpdateBookInformationDL getBookInformationDL)
    {
        _logger = logger;
        _updateBookInformationDL = getBookInformationDL;
    }

    public async Task<UpdateResponse> UpdateBookInformation(string apiVersion, int id, UpdateRequest request)
    {
        try
        {
            return apiVersion switch
            {
                "1" => await HandleApiVersion1(apiVersion, id, request),
                "2" => await HandleApiVersion2(apiVersion, id, request),
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

    private UpdateResponse InternalServerErrorResponse(string apiVersion, string exceptionMessage)
    {
        return new UpdateResponse
        {
            ErrorResult = Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error",
                detail: $"The API version '{apiVersion}' is not supported.",
                extensions: new Dictionary<string, object?>
                {
                    { "apiVersion", apiVersion },
                    { "exceptionMessage", exceptionMessage }
                })
        };
    }

    private UpdateResponse InvalidApiVersionResponse(string apiVersion)
    {
        return new UpdateResponse
        {
            ErrorResult = Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid API Version",
                detail: $"The API version '{apiVersion}' is not supported.",
                extensions: new Dictionary<string, object?>
                {
                    { "apiVersion", apiVersion }
                })
        };
    }

    private UpdateResponse DbErrorResponse(string apiVersion, string detail)
    {
        return new UpdateResponse
        {
            ErrorResult = Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Error during retriving",
                detail: detail,
                extensions: new Dictionary<string, object?>
                {
                    { "apiVersion", apiVersion }
                })
        };
    }

    private UpdateResponse NotFoundResponse(string apiVersion)
    {
        return new UpdateResponse
        {
            ErrorResult = Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Book Not Found",
                detail: "The books information was not found.",
                extensions: new Dictionary<string, object?>
                {
                { "apiVersion", apiVersion }
                })
        };
    }

    #endregion

    #region Version based methods

    private async Task<UpdateResponse> HandleApiVersion1(string apiVersion, int id, UpdateRequest request)
    {
        Dictionary<string, object?> dbGetReturn = await _updateBookInformationDL.GetBookInformation(id);

        string? dbGetErr = Convert.ToString(dbGetReturn["Message"]);

        if (!string.IsNullOrWhiteSpace(dbGetErr))
        {
            return DbErrorResponse(apiVersion, dbGetErr);
        }

        BookInformationModel? existingBookInformation = dbGetReturn["BookInformation"] as BookInformationModel;

        if (existingBookInformation == null)
        {
            return NotFoundResponse(apiVersion);
        }

        existingBookInformation.Title = request.Title;
        existingBookInformation.Available += request.Stock - existingBookInformation.Stock;
        existingBookInformation.Stock = request.Stock;

        Dictionary<string, object?> dbUpdateReturn = await _updateBookInformationDL.UpdateBookInformation(existingBookInformation);

        string? dbUpdateErr = Convert.ToString(dbUpdateReturn["Message"]);

        if (!string.IsNullOrWhiteSpace(dbUpdateErr))
        {
            return DbErrorResponse(apiVersion, dbUpdateErr);
        }

        int updatedId = Convert.ToInt32(dbUpdateReturn["ID"]);

        return new UpdateResponse
        {
            ErrorResult = Results.Ok(),
            ID = updatedId
        };
    }

    private async Task<UpdateResponse> HandleApiVersion2(string apiVersion, int id, UpdateRequest request)
    {
        Dictionary<string, object?> dbGetReturn = await _updateBookInformationDL.GetBookInformation(id);

        string? dbGetErr = Convert.ToString(dbGetReturn["Message"]);

        if (!string.IsNullOrWhiteSpace(dbGetErr))
        {
            return DbErrorResponse(apiVersion, dbGetErr);
        }

        BookInformationModel? existingBookInformation = dbGetReturn["BookInformation"] as BookInformationModel;

        if (existingBookInformation == null)
        {
            return NotFoundResponse(apiVersion);
        }

        existingBookInformation.Title = request.Title;
        existingBookInformation.Available += request.Stock - existingBookInformation.Stock;
        existingBookInformation.Stock = request.Stock;

        Dictionary<string, object?> dbUpdateReturn = await _updateBookInformationDL.UpdateBookInformation(existingBookInformation);

        string? dbUpdateErr = Convert.ToString(dbUpdateReturn["Message"]);

        if (!string.IsNullOrWhiteSpace(dbUpdateErr))
        {
            return DbErrorResponse(apiVersion, dbUpdateErr);
        }

        int updatedId = Convert.ToInt32(dbGetReturn["ID"]);

        return new UpdateResponse
        {
            ErrorResult = Results.Ok(),
            ID = updatedId
        };
    }

    #endregion
}

