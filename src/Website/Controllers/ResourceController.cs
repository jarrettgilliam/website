namespace Website.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Website.Interfaces;
using Website.Models;

[Route ("api/[controller]/[action]")]
public sealed class ResourceController : Controller
{   
    public ResourceController(
        ILogger<ResourceController> logger,
        IReCaptchaService reCaptchaService,
        IAppSettings appSettings)
    {
        this.Logger = logger;
        this.ReCaptchaService = reCaptchaService;
        this.AppSettings = appSettings;
    }

    private ILogger<ResourceController> Logger { get; }
    private IReCaptchaService ReCaptchaService { get; }
    private IAppSettings AppSettings { get; }

    [HttpGet]
    public Task<IActionResult> GetEmail(string token) => this.GetResource(token, this.AppSettings.EmailLink);

    [HttpGet]
    public Task<IActionResult> GetResume(string token) => this.GetResource(token, this.AppSettings.ResumeLink);

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
            return this.InternalServerError();
        }
    }

    private IActionResult InternalServerError() =>
        new StatusCodeResult(StatusCodes.Status500InternalServerError);
}