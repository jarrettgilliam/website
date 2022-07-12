namespace Website.Models;

using System.Text.Json.Serialization;

public sealed class SiteVerifyResponse
{
    /// <summary>
    /// Whether this request was a valid reCAPTCHA token for your site
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    /// <summary>
    /// The score for this request (0.0 - 1.0)
    /// </summary>
    [JsonPropertyName("score")]
    public double Score { get; set; }

    /// <summary>
    /// The action name for this request (important to verify)
    /// </summary>
    [JsonPropertyName("action")]
    public string? Action { get; set; }
    
    /// <summary>
    /// Timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
    /// </summary>
    [JsonPropertyName("challenge_ts")]
    public DateTimeOffset ChallengeTs { get; set; }
    
    /// <summary>
    /// The hostname of the site where the reCAPTCHA was solved
    /// </summary>
    [JsonPropertyName("hostname")]
    public string? Hostname { get; set; }
    
    /// <summary>
    /// Optional
    /// </summary>
    [JsonPropertyName("error-codes")]
    public IEnumerable<string>? ErrorCodes { get; set; }
}