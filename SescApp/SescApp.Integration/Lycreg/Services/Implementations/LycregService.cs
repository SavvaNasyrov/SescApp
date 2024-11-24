using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SescApp.Integration.Lycreg.DomainModels;

namespace SescApp.Integration.Lycreg.Services.Implementations;

public class LycregService(HttpClient httpClient, IConfiguration config) : ILycregService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _lycregSource = config["Paths:Lycreg"] ?? throw new KeyNotFoundException("Paths:Lycreg");
    
    public async Task<AuthResponse> AuthorizationAsync(string login, string password)
    {
        try
        {
            // Подготавливаем данные для отправки
            
            var resp = await _httpClient.PostAsync($"{_lycregSource}");
            var authResponse = JsonSerializer.Deserialize<AuthResponse>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Error in auth");
        }
    }

    public Task<Dictionary<string, Dictionary<string, string>>?> GetTabelAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, string>?> GetSubjectListAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<TeachListResponse>?> GetTeachListAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, Dictionary<string, List<string>>>?> GetJournalAsync()
    {
        throw new NotImplementedException();
    }
}