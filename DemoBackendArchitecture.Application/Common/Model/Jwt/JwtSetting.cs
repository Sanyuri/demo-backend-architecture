using System.Text.Json.Serialization;

namespace DemoBackendArchitecture.Application.Common.Model.Jwt;

public class JwtSetting
{
    [JsonPropertyName("key")]public string Key { get; set; } = string.Empty;
    [JsonPropertyName("issuer")]public string Issuer { get; set; } = string.Empty;
    [JsonPropertyName("audience")]public string Audience { get; set; } = string.Empty;
    [JsonPropertyName("accessTokenExpiration")]public int AccessTokenExpiration { get; set; }
    [JsonPropertyName("refreshTokenExpiration")]public int RefreshTokenExpiration { get; set; }
}