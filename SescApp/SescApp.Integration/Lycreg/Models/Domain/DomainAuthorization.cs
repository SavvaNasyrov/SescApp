using System.Text.Json.Serialization;

namespace SescApp.Integration.Lycreg.Models.Domain
{
    public record DomainAuthorization
    {
        [JsonPropertyName("roles")]
        public required List<string> Roles { get; init; }

        [JsonPropertyName("token")]
        public required string Token { get; init; }

        [JsonPropertyName("teachLoad")]
        public Dictionary<string, string>? TeachLoad { get; init; }
    }
}
