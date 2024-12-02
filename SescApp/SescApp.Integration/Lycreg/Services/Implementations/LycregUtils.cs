using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using SescApp.Integration.Lycreg.Models;

namespace SescApp.Integration.Lycreg.Services.Implementations;

// TODO: добавить cancellationToken
// TODO: сделать нормально кеширование
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
    private Dictionary<string, string> _teachList = new Dictionary<string, string>();
    private DateTime _lastUpdate = DateTime.MinValue;

    private async Task<HttpResponseMessage> GetNewDataRow(Authorization authData, string function)
    {
        var subjectListRequest = new
        {
            t = authData.ShortRole,
            l = authData.Login,
            p = authData.Token,
            f = function
        };
        var resp = await httpClient.PostAsJsonAsync(_lycregSource, subjectListRequest);
        resp.EnsureSuccessStatusCode();
        
        return resp;
    }

    private async Task<Dictionary<string, string>> GetNewDataTeachList(Authorization authData)
    {
        var rowData = await GetNewDataRow(authData, "teachList");
        var result = await rowData.Content.ReadFromJsonAsync<List<TeachListRowField>>();
        Debug.Assert(result != null, nameof(result) + " != null");
        
        return result.ToDictionary(x => x.Login, x => x.Fio);
    }
    
    private async Task<Dictionary<string, string>> GetNewDataSubjectList(Authorization authData)
    {
        var rowData = await GetNewDataRow(authData, "subjList");
        var result = await rowData.Content.ReadFromJsonAsync<Dictionary<string, string>>();

        Debug.Assert(result != null, nameof(result) + " != null");

        return await Task.FromResult(result);
    }

    private async Task EnsureNewData(Authorization authData)
    {
        if (DateTime.Now - _lastUpdate <= TimeSpan.FromDays(1)) return;
        
        _subjList = await GetNewDataSubjectList(authData);
        _teachList = await GetNewDataTeachList(authData);
        _lastUpdate = DateTime.Now;
    }
    
    public async Task<Dictionary<string, string>> GetSubjListAsync(Authorization authData)
    {
        await EnsureNewData(authData);
        return _subjList;
    }
    public async Task<Dictionary<string, string>> GetTeachListAsync(Authorization authData)
    {
        await EnsureNewData(authData);
        return _teachList;
    }
}