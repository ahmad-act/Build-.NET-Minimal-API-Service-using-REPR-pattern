using BookInformationService.BookInformation.Delete;
using BookInformationService.Util;

namespace BookInformationService.BookInformation.Facade.Delete;

public class DeleteBookInformationBL : IDeleteBookInformationBL
{
    private readonly ILogger<object> _logger;
    private readonly IDeleteBookInformationDL _deleteBookInformationDL;

    public DeleteBookInformationBL(ILogger<object> logger, IDeleteBookInformationDL deleteBookInformationDL)
    {
        _logger = logger;
        _deleteBookInformationDL = deleteBookInformationDL;
    }

    public async Task<DeleteResponse> DeleteBookInformation(string apiVersion, int id)
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

    private DeleteResponse InternalServerErrorResponse(string apiVersion, string exceptionMessage)
    {
        return new DeleteResponse
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

    private DeleteResponse InvalidApiVersionResponse(string apiVersion)
    {
        return new DeleteResponse
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

    private DeleteResponse DbErrorResponse(string apiVersion, string detail)
    {
        return new DeleteResponse
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

    private DeleteResponse NotFoundResponse(string apiVersion)
    {
        return new DeleteResponse
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

    private async Task<DeleteResponse> HandleApiVersion1(string apiVersion, int id)
    {
        Dictionary<string, object?> dbReturn = await _deleteBookInformationDL.GetBookInformation(id);

        string? dbErr = Convert.ToString(dbReturn["Message"]);

        if (!string.IsNullOrWhiteSpace(dbErr))
        {
            return DbErrorResponse(apiVersion, dbErr);
        }

        BookInformationModel? existingBookInformation = dbReturn["BookInformation"] as BookInformationModel;

        if (existingBookInformation == null)
        {
            return NotFoundResponse(apiVersion);
        }

        await _deleteBookInformationDL.DeleteBookInformation(existingBookInformation);

        return new DeleteResponse
        {
            ErrorResult = Results.Ok(),
            ID = id
        };
    }

    private async Task<DeleteResponse> HandleApiVersion2(string apiVersion, int id)
    {
        Dictionary<string, object?> dbReturn = await _deleteBookInformationDL.GetBookInformation(id);

        string? dbErr = Convert.ToString(dbReturn["Message"]);

        if (!string.IsNullOrWhiteSpace(dbErr))
        {
            return DbErrorResponse(apiVersion, dbErr);
        }

        BookInformationModel? existingBookInformation = dbReturn["BookInformation"] as BookInformationModel;

        if (existingBookInformation == null)
        {
            return NotFoundResponse(apiVersion);
        }

        await _deleteBookInformationDL.DeleteBookInformation(existingBookInformation);

        return new DeleteResponse
        {
            ErrorResult = Results.Ok(),
            ID = id
        };
    }

    #endregion
}

