namespace Website.Services;

using System.Net.Http;
using System.Text.Json;
using Website.Interfaces;
using Website.Models;

internal sealed class ReCaptchaService : IReCaptchaService
{
    public ReCaptchaService(IAppSettings appSettings)
    {
        this.AppSettings = appSettings;
    }

    private HttpClient HttpClient { get; } = new();
    private IAppSettings AppSettings { get; }

    public async Task<SiteVerifyResponse> SiteVerifyAsync(string captchaToken)
    {
        var query = new Dictionary<string, string>
        {
            ["secret"] = this.AppSettings.ReCaptchaSecret,
            ["response"] = captchaToken
        };

        HttpResponseMessage response = await this.HttpClient.PostAsync(
            "https://www.google.com/recaptcha/api/siteverify",
            new FormUrlEncodedContent(query));

        return JsonSerializer.Deserialize<SiteVerifyResponse>(
            await response.Content.ReadAsStreamAsync()) ??
               throw new Exception($"Unable to deserialize {nameof(SiteVerifyResponse)}");
    }
}