using BookInformationService.BookInformation.Facade.Update;
using Microsoft.AspNetCore.Mvc;

namespace BookInformationService.BookInformation.Update;

public static class UpdateEndpoint
{
    public static void MapUpdateBookInformationEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPut("/BookInformations/{id:int}", handleAsync)
            .MapToApiVersion(1)
            .MapToApiVersion(2)
            .Produces<UpdateResponse>(StatusCodes.Status200OK, "application/json")
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound, "application/problem+json")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
    }

    public static async Task<IResult> handleAsync(HttpContext httpContext, int id, UpdateRequest request, IUpdateBookInformationBL updateBookInformationBL)
    {
        var apiVersion = httpContext.GetRequestedApiVersion();

        UpdateResponse response = await updateBookInformationBL.UpdateBookInformation(apiVersion!.ToString(), id, request);

        if (response.ErrorResult != Results.Ok())
        {
            return response.ErrorResult;
        }

        return Results.Ok(response);

    }
}

