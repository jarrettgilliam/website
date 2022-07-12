namespace Website.Interfaces;

using Website.Models;

public interface IReCaptchaService
{
    Task<SiteVerifyResponse> SiteVerifyAsync(string captchaToken);
}