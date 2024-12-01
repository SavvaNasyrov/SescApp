using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using SescApp.Integration.Lycreg.Models;

namespace SescApp.Integration.Lycreg.Services.Implementations;

// TODO: добавить cancellationToken
public class LycregUtils(HttpClient httpClient, IConfiguration config) : ILycregUtils
{
    private record TeachListRowField
    {
        [JsonPropertyName("login")]
        public required string Login { get; init; }
        
        [JsonPropertyName("fio")]
        public required string Fio { get; init; }
    }
    
    private readonly string _lycregSource = config["Paths:Lycreg"] ?? throw new KeyNotFoundException("Paths:Lycreg");

    private Dictionary<string, string> _subjList = new Dictionary<string, string>();
    private List<string> _somethingElse = [];
    private DateTime _lastUpdateSubjectList = DateTime.MinValue;
    private DateTime _lastUpdateTeachList = DateTime.MinValue;

    private async Task<HttpResponseMessage> EnsureNewDataRow(string login, string token, string function)
    {
        var subjectListRequest = new
        {
            t = "pupil",
            l = login,
            p = token,
            f = function
        };
        var resp = await httpClient.PostAsJsonAsync(_lycregSource, subjectListRequest);

        if (!resp.IsSuccessStatusCode)
        {
            throw new Exception($"Lycreg API call failed with status code {resp.StatusCode}");
        }
        return resp;
    }

    private async Task<Dictionary<string, string>> EnsureNewDataTeachList(string login, string token)
    {
        var rowData = JsonSerializer.Deserialize<List<TeachListRowField>>(await EnsureNewDataRow(login, token, "teachList"));
        
    }
    
    private async Task<Dictionary<string, string>> EnsureNewDataSubjectList(string login, string token)
    {
        var rowData = await EnsureNewDataRow(login, token, "subjList");
        var result = await rowData.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        
        Debug.Assert(result != null, nameof(result) + " != null");

        return await Task.FromResult(result);
    }
    
    public async Task<Dictionary<string, string>> GetSubjListAsync(Authorization authData)
    {
        if (DateTime.Now - _lastUpdateSubjectList < TimeSpan.FromDays(1))
        {
            return _subjList;
        }
        
        _subjList = await EnsureNewDataSubjectList(authData.Login, authData.Token);
        _lastUpdateSubjectList = DateTime.Now;

        return await Task.FromResult(_subjList);
    }
}