using MediatR;

namespace SescApp.Integration.Lycreg.Models.MediatR;

public record FullJournalRequest : IRequest<FullJournalResponse>
{
    public required Authorization Auth { get; init; }
}