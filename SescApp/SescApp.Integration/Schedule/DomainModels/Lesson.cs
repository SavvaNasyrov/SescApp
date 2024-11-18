using System.Text.Json.Serialization;

namespace SescApp.Integration.Schedule.DomainModels
{
    public record Lesson
    {
        [JsonPropertyName("uid")]
        public int Uid { get; init; }

        [JsonPropertyName("subject")]
        public string Subject { get; init; }

        [JsonPropertyName("auditory")]
        public string Auditory { get; init; }

        [JsonPropertyName("group")]
        public string Group { get; init; }

        [JsonPropertyName("teacher")]
        public string Teacher { get; init; }

        [JsonPropertyName("subgroup")]
        public int Subgroup { get; init; }

        [JsonPropertyName("number")]
        public int Number { get; init; }

        [JsonPropertyName("weekday")]
        public int Weekday { get; init; }
    }
}
