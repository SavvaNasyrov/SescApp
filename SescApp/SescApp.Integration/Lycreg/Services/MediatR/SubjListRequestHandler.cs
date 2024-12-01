using MediatR;
using Microsoft.Extensions.Configuration;
using SescApp.Integration.Lycreg.Models.MediatR;
using System.Net.Http.Json;

namespace SescApp.Integration.Lycreg.Services.MediatR
{
    public class SubjListRequestHandler(HttpClient httpClient, IConfiguration config) : IRequestHandler<SubjListRequest, IReadOnlyDictionary<string, string>>
    {
        private readonly string _lycregSource = config["Paths:Lycreg"] ?? throw new KeyNotFoundException("Paths:Lycreg");

        public async Task<IReadOnlyDictionary<string, string>> Handle(SubjListRequest request, CancellationToken cancellationToken)
        {
            var subjListRequest = new
            {
                t = request.Auth.ShortRole,
                l = request.Auth.Login,
                p = request.Auth.Token,
                f = "subjList"
            };

            var resp = await httpClient.PostAsJsonAsync(_lycregSource, subjListRequest, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            resp.EnsureSuccessStatusCode();

            var result = await resp.Content.ReadFromJsonAsync<Dictionary<string, string>>(cancellationToken)
                ?? throw new InvalidOperationException("Response is not dict, may be auth problems");

            return result.AsReadOnly();
        }
    }
}
