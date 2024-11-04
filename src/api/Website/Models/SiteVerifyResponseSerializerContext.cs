namespace Website.Models;

using System.Text.Json.Serialization;

[JsonSerializable(typeof(SiteVerifyResponse))]
[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
internal partial class SiteVerifyResponseSerializerContext : JsonSerializerContext;