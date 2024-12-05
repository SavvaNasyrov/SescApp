using System.Net.Http.Json;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SescApp.Integration.Lycreg.Models.MediatR;

namespace SescApp.Integration.Lycreg.Services.MediatR
{
    public class SubjListRequestHandler(HttpClient httpClient, IConfiguration config, IMemoryCache cache) : IRequestHandler<SubjListRequest, IReadOnlyDictionary<string, string>>
    {
        private readonly string _lycregSource = config["Paths:Lycreg"] ?? throw new KeyNotFoundException("Paths:Lycreg");

        private async Task<IReadOnlyDictionary<string, string>> FetchSubjListAsync(SubjListRequest request,
            CancellationToken cancellationToken)
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

            var strResp = await resp.Content.ReadAsStringAsync(cancellationToken);

            if (strResp == "none")
                throw new ArgumentException("Unauthorized");

            var result = await resp.Content.ReadFromJsonAsync<Dictionary<string, string>>(cancellationToken)
                         ?? throw new InvalidOperationException("Response is not dict, may be auth problems");

            return result.AsReadOnly();
        }

        public async Task<IReadOnlyDictionary<string, string>> Handle(SubjListRequest request, CancellationToken cancellationToken)
        {
            if (!cache.TryGetValue("subjList", out IReadOnlyDictionary<string, string>? subjList))
            {
                subjList = await FetchSubjListAsync(request, cancellationToken);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
                cache.Set("subjList", subjList, cacheEntryOptions);
            }

            return subjList!;
        }
    }
}
