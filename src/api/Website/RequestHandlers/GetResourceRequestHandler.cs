namespace Website.RequestHandlers;

using System.Text.Json;
using System.Threading;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Website.Interfaces;
using Website.Models;

internal sealed class GetResourceRequestHandler(
    IConfiguration Configuration,
    IReCaptchaService ReCaptchaService,
    ILogger<GetResourceRequestHandler> Logger)
    : IRequestHandler<GetResourceRequest, IResult>
{
    public async ValueTask<IResult> Handle(GetResourceRequest request, CancellationToken cancellationToken)
    {
        SiteVerifyResponse response = await ReCaptchaService.SiteVerifyAsync(request.CaptchaToken);

        if (response is { Success: true, Score: >= 0.5, Action: "submit" } && response.Hostname == Configuration["ReCaptchaHostname"])
        {
            return TypedResults.Content(request.ResourceLink);
        }

        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("reCAPTCHA token rejected: {Response}",
                JsonSerializer.Serialize(response, SiteVerifyResponseSerializerContext.Default.SiteVerifyResponse));
        }

        return TypedResults.Unauthorized();
    }
}