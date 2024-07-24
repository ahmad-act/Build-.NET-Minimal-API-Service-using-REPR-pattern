using BookInformationService.BookInformation.Facade;
using BookInformationService.BookInformation.Facade.Create;
using Microsoft.AspNetCore.Mvc;

namespace BookInformationService.BookInformation.Create;

public static class CreateEndpoint
{
    public static void MapCreateBookInformationEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/BookInformations", handleAsync)
            .MapToApiVersion(1)
            .MapToApiVersion(2)
            .Produces<CreateResponse>(StatusCodes.Status201Created, "application/json")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, "application/problem+json");
    }

    public static async Task<IResult> handleAsync(HttpContext httpContext, CreateRequest request, ICreateBookInformationBL createBookInformationBL)
    {
        var apiVersion = httpContext.GetRequestedApiVersion();

        CreateResponse response = await createBookInformationBL.CreateBookInformation(apiVersion!.ToString(), request);

        if (response.ErrorResult != Results.Ok())
        {
            return response.ErrorResult;
        }

        return Results.Created($"/bookinformations/{response.ID}", response);
    }
}

