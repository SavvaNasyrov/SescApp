using System.Net.Http.Json;
using MediatR;
using Microsoft.Extensions.Configuration;
using SescApp.Integration.Lycreg.Models.MediatR;

namespace SescApp.Integration.Lycreg.Services.MediatR;

// TODO: наверное добавить кеширование (1 запрос выполняется 3 секунды)
public class TabelRequestHandler(HttpClient httpClient, IConfiguration config, IMediator mediator) : IRequestHandler<TabelRequest, TabelResponse>
{
    private readonly string _lycregSource = config["Paths:Lycreg"] ?? throw new KeyNotFoundException("Paths:Lycreg");

    public async Task<TabelResponse> Handle(TabelRequest request, CancellationToken cancellationToken)
    {
        var tabelRequest = new
        {
            t = request.Auth.ShortRole,
            l = request.Auth.Login,
            p = request.Auth.Token,
            f = "tabelGet",
            z = new List<string>
            {
                request.Auth.Login
            }
        };
        
        var resp = await httpClient.PostAsJsonAsync(_lycregSource, tabelRequest, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        
        if (!resp.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http status code {resp.StatusCode}: {resp.ReasonPhrase}");
        }
        
        var tabelData = await resp.Content.ReadFromJsonAsync<Dictionary<string, Dictionary<string, string>>>(cancellationToken);
        
        var subjList = await mediator.Send(new SubjListRequest
        {
            Auth = request.Auth,
        }, cancellationToken);
        
        var formattedData = tabelData!.Select(kvp => (subjList[kvp.Key].ToString(), kvp.Value.Select(vals => (DtsitData.Dtsit[vals.Key][0], vals.Value)).ToDictionary())).ToDictionary(); // в этой строчке можно поменять то, как выглядит результат
        
        return new TabelResponse
        {
            Tabel = formattedData
        };
    }
}