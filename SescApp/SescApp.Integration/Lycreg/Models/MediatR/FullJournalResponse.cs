using SescApp.Integration.Lycreg.Models.JournalModels;

namespace SescApp.Integration.Lycreg.Models.MediatR;

public record FullJournalResponse
{
    public required List<JournalSubject> Subjects { get; init; }
}