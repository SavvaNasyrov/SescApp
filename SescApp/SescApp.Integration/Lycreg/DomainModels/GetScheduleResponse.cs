using System.Text.Json.Serialization;

namespace SescApp.Integration.Schedule.DomainModels
{
    public record GetScheduleResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; init; }

        [JsonPropertyName("lessons")]
        public Lesson[] Lessons { get; init; }

        [JsonPropertyName("diffs")]
        public Lesson[] Diffs { get; init; }
    }
}
