namespace SescApp.Integration.Lycreg.Models
{
    public record Authorization
    {
        public List<LycregRole>? Roles { get; init; }

        public string? Token { get; init; }

        public Dictionary<string, string>? TeachLoad { get; init; }
    }
}
