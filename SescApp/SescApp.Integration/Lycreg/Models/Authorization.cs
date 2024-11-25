namespace SescApp.Integration.Lycreg.Models
{
    public record Authorization
    {
        public required List<LycregRole> Roles { get; init; }

        public required string Token { get; init; }

        public required Dictionary<string, string> TeachLoad { get; init; }
    }
}
