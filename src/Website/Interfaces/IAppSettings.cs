namespace Website.Interfaces;

public interface IAppSettings
{
    public string ReCaptchaSecret { get; }
    public string EmailLink { get; }
    public string ResumeLink { get; }
}