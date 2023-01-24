namespace Website.Services;

using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Website.Interfaces;
using Website.Models;

internal sealed class ReCaptchaService : IReCaptchaService
{
    public ReCaptchaService(
        IHttpClientFactory httpClientFactory,
        IOptions<AppSecrets> appSettings)
    {
        this.HttpClient = httpClientFactory.CreateClient();
        this.AppSecrets = appSettings.Value;
    }

    private HttpClient HttpClient { get; }
    private AppSecrets AppSecrets { get; }

    public async Task<SiteVerifyResponse> SiteVerifyAsync(string captchaToken)
    {
        var query = new Dictionary<string, string>
        {
            ["secret"] = this.AppSecrets.ReCaptchaSecret,
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