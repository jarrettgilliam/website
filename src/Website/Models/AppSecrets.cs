namespace Website.Models;

using System.ComponentModel.DataAnnotations;

public sealed class AppSecrets
{
    [Required]
    required public string ReCaptchaSecret { get; init; }

    [Required]
    required public string EmailLink { get; init; }

    [Required, Url]
    required public string ResumeLink { get; init; }
}