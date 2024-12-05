using MediatR;
using Microsoft.Extensions.Configuration;
using SescApp.Integration.Lycreg.Models.MediatR;
using System.Net.Http.Json;

namespace SescApp.Integration.Lycreg.Services.MediatR
{
    public class CheckAuthRequestHandler(HttpClient httpClient, IConfiguration config) : IRequestHandler<CheckAuthRequest, bool>
    {
        private readonly string _lycregSource = config["Paths:Lycreg"] ?? throw new KeyNotFoundException("Paths:Lycreg");

        public async Task<bool> Handle(CheckAuthRequest request, CancellationToken cancellationToken)
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
                return false;

            return true;
        }
    }
}
