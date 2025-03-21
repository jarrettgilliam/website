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
        this.AppSecrets = appSettings;
    }

    private HttpClient HttpClient { get; }
    private IOptions<AppSecrets> AppSecrets { get; }

    public async Task<SiteVerifyResponse> SiteVerifyAsync(string captchaToken)
    {
        var query = new Dictionary<string, string>
        {
            ["secret"] = this.AppSecrets.Value.ReCaptchaSecret,
            ["response"] = captchaToken
        };

        HttpResponseMessage response = await this.HttpClient.PostAsync(
            "https://www.google.com/recaptcha/api/siteverify",
            new FormUrlEncodedContent(query));

        return JsonSerializer.Deserialize(
                   await response.Content.ReadAsStreamAsync(),
                   SiteVerifyResponseSerializerContext.Default.SiteVerifyResponse) ??
               throw new Exception($"Unable to deserialize {nameof(SiteVerifyResponse)}");
    }
}