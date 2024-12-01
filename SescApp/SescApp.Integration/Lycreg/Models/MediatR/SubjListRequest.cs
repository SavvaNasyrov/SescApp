using MediatR;

namespace SescApp.Integration.Lycreg.Models.MediatR
{
    public record SubjListRequest : IRequest<IReadOnlyDictionary<string, string>>
    {
        public required Authorization Auth { get; init; }
    }
}
