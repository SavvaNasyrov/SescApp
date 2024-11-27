using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SescApp.Integration.Lycreg.DomainModels;
using SescApp.Integration.Lycreg.Models;
using System.Data;
using System.Net.Http.Json;

namespace SescApp.Integration.Lycreg.Services.Implementations;

public class AuthService(HttpClient httpClient, IConfiguration config, ICaptchaSolver captchaSolver) : IAuthService
{
    private readonly string _lycregSource = config["Paths:Lycreg"] ?? throw new KeyNotFoundException("Paths:Lycreg");

    private readonly HttpClient _httpClient = httpClient;

    public async Task<(AuthorizationResult, Authorization? Auth)> AuthorizationAsync(string login, string password)
    {
        var (captchaId, captchaSolution) = await captchaSolver.GetSolvedCaptcha();

        var authRequest = new
        {
            t = "pupil",
            l = login,
            p = password,
            f = "login",
            ci = captchaId,
            c = captchaSolution
        };

        var resp = await _httpClient.PostAsJsonAsync(_lycregSource, authRequest);

        if (!resp.IsSuccessStatusCode)
        {
            return (AuthorizationResult.Error, null);
        }

        var answer = await resp.Content.ReadAsStringAsync();

        if (answer == "none")
        {
            return (AuthorizationResult.InvalidData, null);
        }

        var authData = await resp.Content.ReadFromJsonAsync<AuthResponse>();

        if (authData == null)
        {
            return (AuthorizationResult.Error, null);
        }

        var roles = authData.Roles.Select(role => Enum.Parse<LycregRole>(role, ignoreCase: true)).ToList();

        var auth = new Authorization
        {
            Roles = roles,
            Token = authData.Token,
            TeachLoad = authData.TeachLoad,
        };

        return (AuthorizationResult.Success, auth);
    }

    public Task<Dictionary<string, Dictionary<string, List<string>>>?> GetJournalAsync(string token, string login)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, string>?> GetSubjectListAsync(string token, string login)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, Dictionary<string, string>>?> GetTabelAsync(string token, string login)
    {
        throw new NotImplementedException();
    }

    public Task<List<TeachListResponse>?> GetTeachListAsync(string token, string login)
    {
        throw new NotImplementedException();
    }
}
