using BookInformationService.BookInformation.Create;
using BookInformationService.BookInformation.Get;
using BookInformationService.Util;

namespace BookInformationService.BookInformation.Facade.Create;

public class CreateBookInformationBL : ICreateBookInformationBL
{
    private readonly ILogger<object> _logger;
    private readonly ICreateBookInformationDL _createBookInformationDL;

    public CreateBookInformationBL(ILogger<object> logger, ICreateBookInformationDL createBookInformationDL)
    {
        _logger = logger;
        _createBookInformationDL = createBookInformationDL;
    }

    public async Task<CreateResponse> CreateBookInformation(string apiVersion, CreateRequest request)
    {
        try
        {
            return apiVersion switch
            {
                "1" => await HandleApiVersion1(apiVersion, request),
                "2" => await HandleApiVersion2(apiVersion, request),
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

    private CreateResponse InternalServerErrorResponse(string apiVersion, string exceptionMessage)
    {
        return new CreateResponse
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
            ID = -1
        };
    }

    private CreateResponse InvalidApiVersionResponse(string apiVersion)
    {
        return new CreateResponse
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

    private CreateResponse DbErrorResponse(string apiVersion, string detail)
    {
        return new CreateResponse
        {
            ErrorResult = Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Error during retriving",
                detail: detail,
                extensions: new Dictionary<string, object?>
                {
                    { "apiVersion", apiVersion }
                }),
            ID = 0
        };
    }

    #endregion

    #region Version based methods

    private async Task<CreateResponse> HandleApiVersion1(string apiVersion, CreateRequest request)
    {
        Dictionary<string, object?> dbReturn = await _createBookInformationDL.CreateBookInformation(request.ToBookInformationModel());

        string? dbErr = Convert.ToString(dbReturn["Message"]);

        if (!string.IsNullOrWhiteSpace(dbErr))
        {
            return DbErrorResponse(apiVersion, dbErr);
        }

        int id = Convert.ToInt32(dbReturn["ID"]);

        return new CreateResponse
        {
            ErrorResult = Results.Ok(),
            ID = id
        };
    }

    private async Task<CreateResponse> HandleApiVersion2(string apiVersion, CreateRequest request)
    {
        Dictionary<string, object?> dbReturn = await _createBookInformationDL.CreateBookInformation(request.ToBookInformationModel());

        string? dbErr = Convert.ToString(dbReturn["Message"]);

        if (!string.IsNullOrWhiteSpace(dbErr))
        {
            return DbErrorResponse(apiVersion, dbErr);
        }

        int id = Convert.ToInt32(dbReturn["ID"]);

        return new CreateResponse
        {
            ErrorResult = Results.Ok(),
            ID = id
        };
    }

    #endregion
}

