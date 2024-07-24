using BookInformationService.BookInformation.Facade.Delete;
using Microsoft.AspNetCore.Mvc;

namespace BookInformationService.BookInformation.Delete;

public static class DeleteEndpoint
{
    public static void MapDeleteBookInformationEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/BookInformations/{id:int}", handleAsync)
            .MapToApiVersion(1)
            .MapToApiVersion(2)
            .Produces<DeleteResponse>(StatusCodes.Status200OK, "application/json")
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
    }

    public static async Task<IResult> handleAsync(HttpContext httpContext, int id, IDeleteBookInformationBL deleteBookInformationBL)
    {
        var apiVersion = httpContext.GetRequestedApiVersion();

        DeleteResponse response = await deleteBookInformationBL.DeleteBookInformation(apiVersion!.ToString(), id);

        if (response.ErrorResult != Results.Ok())
        {
            return response.ErrorResult;
        }

        return Results.Ok(response);
    }
}

