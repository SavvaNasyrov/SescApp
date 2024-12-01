using MediatR;

namespace SescApp.Integration.Lycreg.Models.MediatR;

public record AuthRequest : IRequest<AuthResponse>
{
    public required string Login { get; init; }

    public required string Password { get; init; }

    public string Role { get; init; } = "pupil";

    public required string CaptchaId { get; init; }

    public required string CaptchaSolution { get; init; }
}