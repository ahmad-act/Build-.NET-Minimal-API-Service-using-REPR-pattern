using BookInformationService.BookInformation.Facade.List;
using Microsoft.AspNetCore.Mvc;

namespace BookInformationService.BookInformation.List;

public static class ListEndpoint
{
    public static void MapListBookInformationEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/BookInformations", handleAsync)
            .MapToApiVersion(1)
            .MapToApiVersion(2)
            .Produces<ListResponse>(StatusCodes.Status200OK, "application/json")
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
    }

    public static async Task<IResult> handleAsync(HttpContext httpContext, IListBookInformationBL listBookInformationBL)
    {
        var apiVersion = httpContext.GetRequestedApiVersion();

        ListResponse response = await listBookInformationBL.ListBookInformation(apiVersion!.ToString());

        if(response.ErrorResult != Results.Ok())
        {
            return response.ErrorResult;
        }

        return Results.Ok(response);
    }
}

