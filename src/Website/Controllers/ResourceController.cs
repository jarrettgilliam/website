namespace Website.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Website.Interfaces;
using Website.Models;

[Route ("api/[controller]/[action]")]
public sealed class ResourceController : Controller
{
    public ResourceController(
        ILogger<ResourceController> logger,
        IReCaptchaService reCaptchaService,
        IOptions<AppSecrets> appSettings)
    {
        this.Logger = logger;
        this.ReCaptchaService = reCaptchaService;
        this.AppSecrets = appSettings.Value;
    }

    private ILogger<ResourceController> Logger { get; }
    private IReCaptchaService ReCaptchaService { get; }
    private AppSecrets AppSecrets { get; }

    [HttpGet]
    public Task<IActionResult> GetEmail(string token) => this.GetResource(token, this.AppSecrets.EmailLink);

    [HttpGet]
    public Task<IActionResult> GetResume(string token) => this.GetResource(token, this.AppSecrets.ResumeLink);

    [NonAction]
    private async Task<IActionResult> GetResource(
        string captchaToken,
        string resourceLink)
    {
        try
        {
            SiteVerifyResponse response = await this.ReCaptchaService.SiteVerifyAsync(captchaToken);

            if (response.Success && response.Score >= 0.5)
            {
                return this.Ok(resourceLink);
            }

            return this.Forbid();
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Error in method {Method}", nameof(this.GetResource));
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}