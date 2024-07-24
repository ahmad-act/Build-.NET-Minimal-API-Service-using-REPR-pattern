using Microsoft.AspNetCore.Mvc;

using BookInformationService.BookInformation.Facade.Get;

namespace BookInformationService.BookInformation.Get;

public static class GetEndpoint
{
    public static void MapGetBookInformationEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/BookInformations/{id:int}", handleAsync)
            .MapToApiVersion(1)
            .MapToApiVersion(2)
            .Produces<GetResponse>(StatusCodes.Status200OK, "application/json")
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");

    }

    public static async Task<IResult> handleAsync(HttpContext httpContext, int id, IGetBookInformationBL getBookInformationBL)
    {
        var apiVersion = httpContext.GetRequestedApiVersion();

        GetResponse response = await getBookInformationBL.GetBookInformation(apiVersion!.ToString(), id);

        if (response.ErrorResult != Results.Ok())
        {
            return response.ErrorResult;
        }

        return Results.Ok(response);

    }
}

