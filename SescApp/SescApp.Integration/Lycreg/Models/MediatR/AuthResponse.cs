using SescApp.Integration.Lycreg.Models.Domain;

namespace SescApp.Integration.Lycreg.Models.MediatR
{
    public record AuthResponse
    {
        public required AuthorizationResult Result { get; init; }

        public Authorization? Auth { get; init; }
    }
}
