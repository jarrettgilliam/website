namespace Website.Models;

using System.ComponentModel.DataAnnotations;

public sealed class AppSecrets
{
    [Required]
    public string ReCaptchaSecret { get; init; } = string.Empty;

    [Required]
    public string EmailLink { get; init; } = string.Empty;

    [Required, Url]
    public string ResumeLink { get; init; } = string.Empty;
}