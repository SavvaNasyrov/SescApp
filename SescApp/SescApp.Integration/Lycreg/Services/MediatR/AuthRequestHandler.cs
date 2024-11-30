using MediatR;
using Microsoft.Extensions.Configuration;
using SescApp.Integration.Lycreg.Models;
using SescApp.Integration.Lycreg.Models.MediatR;
using System.Net.Http.Json;

namespace SescApp.Integration.Lycreg.Services.MediatR
{
    public class AuthRequestHandler(ICaptchaSolver captchaSolver, IConfiguration config, HttpClient httpClient) : IRequestHandler<AuthRequest, AuthResponse>
    {
        private readonly ICaptchaSolver _captchaSolver = captchaSolver;

        private readonly string _lycregSource = config["Paths:Lycreg"] ?? throw new KeyNotFoundException("Paths:Lycreg");

        private readonly HttpClient _httpClient = httpClient;

        public async Task<AuthResponse> Handle(AuthRequest request, CancellationToken cancellationToken)
        {
            var (captchaId, captchaSolution) = await _captchaSolver.GetSolvedCaptcha(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var authRequest = new
            {
                t = request.Role,
                l = request.Login,
                p = request.Password,
                f = "login",
                ci = captchaId,
                c = captchaSolution
            };

            var resp = await _httpClient.PostAsJsonAsync(_lycregSource, authRequest, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            if (!resp.IsSuccessStatusCode)
            {
                return new AuthResponse { Result = AuthorizationResult.Error };
            }

            var answer = await resp.Content.ReadAsStringAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            if (answer == "none")
            {
                return new AuthResponse { Result = AuthorizationResult.InvalidData };
            }

            var authData = await resp.Content.ReadFromJsonAsync<Authorization>(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            if (authData == null)
            {
                return new AuthResponse { Result = AuthorizationResult.Error };
            }

            return new AuthResponse { Result = AuthorizationResult.Success, Auth = authData };
        }
    }
}
