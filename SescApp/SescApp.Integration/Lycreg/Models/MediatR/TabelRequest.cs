using MediatR;

namespace SescApp.Integration.Lycreg.Models.MediatR;

public record TabelRequest : IRequest<TabelResponse>
{
    public required Authorization Auth { get; init; }

}