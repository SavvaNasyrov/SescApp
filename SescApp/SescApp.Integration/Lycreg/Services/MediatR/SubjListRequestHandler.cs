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
            var basedSubjects = new Dictionary<string, string>(){
                { "s110", "Русский язык" },
                { "s120", "Литература" },
                { "s210", "Английский язык" },
                { "s220", "Немецкий язык" },
                { "s230", "Французский язык" },
                { "s310", "Искусство" },
                { "s320", "МХК" },
                { "s330", "Музыка" },
                { "s410", "Математика" },
                { "s420", "Алгебра" },
                { "s430", "Алгебра и начала анализа" },
                { "s440", "Геометрия" },
                { "s450", "Вероятность и статистика" },
                { "s460", "Информатика" },
                { "s510", "История" },
                { "s520", "История России" },
                { "s530", "Всеобщая история" },
                { "s540", "Обществознание" },
                { "s550", "Экономика" },
                { "s560", "Право" },
                { "s570", "География" },
                { "s610", "Физика" },
                { "s620", "Астрономия" },
                { "s630", "Химия" },
                { "s640", "Биология" },
                { "s710", "Технология" },
                { "s810", "Физическая культура" },
                { "s820", "ОБЗР" }
            };
            var combinedResult = result.Concat(basedSubjects).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return combinedResult.AsReadOnly();
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
