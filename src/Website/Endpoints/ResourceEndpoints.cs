namespace Website.Endpoints;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Website.Interfaces;
using Website.Models;

public static class ResourceEndpoints
{
    public static void MapResourceEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/resource").WithOpenApi();

        group.MapGet("/email", GetEmail).WithSummary("Get email address");
        group.MapGet("/resume", GetResume).WithSummary("Get resume link");
    }

    private static Task<Results<ContentHttpResult, ForbidHttpResult>> GetEmail(
        [FromServices] IOptions<AppSecrets> appSecrets,
        [FromServices] IReCaptchaService reCaptchaService,
        [FromQuery] string token) =>
        GetResource(reCaptchaService, token, appSecrets.Value.EmailLink);

    private static Task<Results<ContentHttpResult, ForbidHttpResult>> GetResume(
        [FromServices] IOptions<AppSecrets> appSecrets,
        [FromServices] IReCaptchaService reCaptchaService,
        [FromQuery] string token) =>
        GetResource(reCaptchaService, token, appSecrets.Value.ResumeLink);

    private static async Task<Results<ContentHttpResult, ForbidHttpResult>> GetResource(
        IReCaptchaService reCaptchaService, string captchaToken, string resourceLink)
    {
        SiteVerifyResponse response = await reCaptchaService.SiteVerifyAsync(captchaToken);

        return response is { Success: true, Score: >= 0.5 }
            ? TypedResults.Content(resourceLink)
            : TypedResults.Forbid();
    }
}