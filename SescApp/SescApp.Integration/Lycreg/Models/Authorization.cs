using System.Text.Json.Serialization;

namespace SescApp.Integration.Lycreg.Models
{
    public record Authorization
    {
        [JsonPropertyName("roles")]
        public required List<string> Roles { get; init; }
        
        [JsonPropertyName("token")]
        public required string Token { get; init; }

        [JsonPropertyName("teachLoad")]
        public required Dictionary<string, string> TeachLoad { get; init; }

        [JsonIgnore]
        public string Login { get; set; } = null!;
    }
}
