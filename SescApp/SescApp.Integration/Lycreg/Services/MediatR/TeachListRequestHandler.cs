using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Configuration;
using SescApp.Integration.Lycreg.Models.MediatR;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;

namespace SescApp.Integration.Lycreg.Services.MediatR
{
    public class TeachListRequestHandler(HttpClient httpClient, IConfiguration config, IMemoryCache cache) : IRequestHandler<TeachListRequest, IReadOnlyDictionary<string, string>>
    {
        private record TeachListRowField
        {
            [JsonPropertyName("login")]
            public required string Login { get; init; }
        
            [JsonPropertyName("fio")]
            public required string Fio { get; init; }
        }
        
        private readonly string _lycregSource = config["Paths:Lycreg"] ?? throw new KeyNotFoundException("Paths:Lycreg");

        private async Task<IReadOnlyDictionary<string, string>> FetchTeachListAsync(TeachListRequest request,
            CancellationToken cancellationToken)
        {
            var teachListRequest = new
            {
                t = request.Auth.ShortRole,
                l = request.Auth.Login,
                p = request.Auth.Token,
                f = "teachList"
            };

            var resp = await httpClient.PostAsJsonAsync(_lycregSource, teachListRequest, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            resp.EnsureSuccessStatusCode();

            var result = await resp.Content.ReadFromJsonAsync<List<TeachListRowField>>(cancellationToken)
                         ?? throw new InvalidOperationException("Response is not array of tuples, may be auth problems");
            
            Console.WriteLine(result);
            Debug.Assert(result is not null);
            return result.ToDictionary(x => x.Login, y => y.Fio).AsReadOnly();
        }
        
        public async Task<IReadOnlyDictionary<string, string>> Handle(TeachListRequest request, CancellationToken cancellationToken)
        {
            if (!cache.TryGetValue("teachList", out IReadOnlyDictionary<string, string>? teachList))
            {
                teachList = await FetchTeachListAsync(request, cancellationToken);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
                cache.Set("teachList", teachList, cacheEntryOptions);
            }

            return teachList!;
        }
    }
}
