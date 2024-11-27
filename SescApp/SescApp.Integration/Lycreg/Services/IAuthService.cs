using SescApp.Integration.Lycreg.DomainModels;
using SescApp.Integration.Lycreg.Models;

namespace SescApp.Integration.Lycreg.Services;


public interface IAuthService
{
    public Task<(AuthorizationResult, Authorization? Auth)> AuthorizationAsync(string login, string password);

    public Task<Dictionary<string, Dictionary<string, string>>?> GetTabelAsync(string token, string login);

    public Task<Dictionary<string, string>?> GetSubjectListAsync(string token, string login);

    public Task<List<TeachListResponse>?> GetTeachListAsync(string token, string login);

    public Task<Dictionary<string, Dictionary<string, List<string>>>?> GetJournalAsync(string token, string login);
    // TODO: дописать модельку для GetJournalAsync
}
