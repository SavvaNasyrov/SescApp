using MediatR;
using Microsoft.Extensions.Configuration;
using SescApp.Integration.Lycreg.Models;
using SescApp.Integration.Lycreg.Models.MediatR;
using System.Net.Http.Json;
using System.Text.Json;

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
            
            // добавляем в json поле Login (его требует моделька Authorization)
            var jsonResponse = await resp.Content.ReadFromJsonAsync<Dictionary<string, object>>(cancellationToken: cancellationToken);
            jsonResponse["login"] = request.Login;
            var jsonResponseSerialised = JsonSerializer.Serialize(jsonResponse);
            
            // Преобразуем jsonResponse обратно в объект Authorization
            var authData = JsonSerializer.Deserialize<Authorization>(jsonResponseSerialised);
            cancellationToken.ThrowIfCancellationRequested();

            if (authData == null)
            {
                return new AuthResponse { Result = AuthorizationResult.Error };
            }

            return new AuthResponse { Result = AuthorizationResult.Success, Auth = authData };
        }
    }
}
