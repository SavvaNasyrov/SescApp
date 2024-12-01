using MediatR;

namespace SescApp.Integration.Lycreg.Models.MediatR
{
    public class TeachListRequest : IRequest<IReadOnlyDictionary<string, string>>
    {
        public required Authorization Auth { get; init; }
    }
}
