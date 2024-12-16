namespace SescApp.Integration.Lycreg.Models.MediatR;

public record TabelResponse
{
    public required Dictionary<string, Dictionary<string, string>> Tabel { get; init; }
}