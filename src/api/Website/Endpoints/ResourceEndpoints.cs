namespace Website.Endpoints;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Website.Interfaces;
using Website.Models;

public static class ResourceEndpoints
{
    public static void MapResourceEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/resource").RequireRateLimiting("api");

        group.MapGet("/email", GetEmail).WithSummary("Get email address");
        group.MapGet("/resume", GetResume).WithSummary("Get resume link");
    }

    private static Task<Results<ContentHttpResult, UnauthorizedHttpResult>> GetEmail(
        [FromServices] IOptions<AppSecrets> appSecrets,
        [FromServices] IConfiguration configuration,
        [FromServices] IReCaptchaService reCaptchaService,
        [FromQuery] string token) =>
        GetResource(reCaptchaService, token, appSecrets.Value.EmailLink, configuration["Hostname"] ?? "");

    private static Task<Results<ContentHttpResult, UnauthorizedHttpResult>> GetResume(
        [FromServices] IOptions<AppSecrets> appSecrets,
        [FromServices] IConfiguration configuration,
        [FromServices] IReCaptchaService reCaptchaService,
        [FromQuery] string token) =>
        GetResource(reCaptchaService, token, appSecrets.Value.ResumeLink, configuration["Hostname"] ?? "");

    private static async Task<Results<ContentHttpResult, UnauthorizedHttpResult>> GetResource(
        IReCaptchaService reCaptchaService, string captchaToken, string resourceLink, string hostname)
    {
        SiteVerifyResponse response = await reCaptchaService.SiteVerifyAsync(captchaToken);

        return response is { Success: true, Score: >= 0.5, Action: "submit" }
            && response.Hostname == hostname
            ? TypedResults.Content(resourceLink)
            : TypedResults.Unauthorized();
    }
}
