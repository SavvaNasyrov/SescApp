using System.Text.Json.Serialization;

namespace SescApp.Integration.Schedule.DomainModels;

public record TeachListResponse
{
    [JsonPropertyName("login")]
    public required string Login { get; init; }

    [JsonPropertyName("fio")]
    public required string Fio { get; init; }

}
