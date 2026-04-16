namespace Website.Endpoints;

using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Website.Models;

public static class ResourceEndpoints
{
    public static void MapResourceEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/resource").RequireRateLimiting("api");

        group.MapGet("/email", GetEmail).WithSummary("Get email address");
        group.MapGet("/resume", GetResume).WithSummary("Get resume link");
    }

    private static async Task<IResult> GetEmail(
        [FromServices] IMediator mediator,
        [FromServices] IOptions<AppSecrets> appSecrets,
        [FromQuery] string token) =>
        await mediator.Send(new GetResourceRequest(token, appSecrets.Value.EmailLink));

    private static async Task<IResult> GetResume(
        [FromServices] IMediator mediator,
        [FromServices] IOptions<AppSecrets> appSecrets,
        [FromQuery] string token) =>
        await mediator.Send(new GetResourceRequest(token, appSecrets.Value.ResumeLink));
}
