namespace Website.Services;

using Website.Interfaces;

internal sealed class AppSettings : IAppSettings
{
    public AppSettings()
    {
        this.ReCaptchaSecret = this.GetEnvironmentVariableOrThrow("RECAPTCHA_SECRET");
        this.EmailLink = this.GetEnvironmentVariableOrThrow("EMAIL_LINK");
        this.ResumeLink = this.GetEnvironmentVariableOrThrow("RESUME_LINK");
    }

    public string ReCaptchaSecret { get; }
    public string EmailLink { get; }
    public string ResumeLink { get; }

    private string GetEnvironmentVariableOrThrow(string variable) =>
        Environment.GetEnvironmentVariable(variable) ??
            throw new Exception($"{variable} environment variable not set");
}