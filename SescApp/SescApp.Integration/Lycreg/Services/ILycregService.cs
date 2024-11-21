using SescApp.Integration.Lycreg.DomainModels;
using SescApp.Integration.Schedule.DomainModels;

namespace SescApp.Integration.Lycreg.Services;


public interface ILycregService
{
    public Task<AuthResponse> AuthorizationAsync(string login, string password);
    public Task<Dictionary<string, Dictionary<string, string>>> GetTabelAsync();
    public Task<Dictionary<string, string>> GetSubjectListAsync();
    public Task<List<TeachListResponse>> GetTeachListAsync();
    public Task<Dictionary<string, Dictionary<string, List<>>>> GetJournalAsync();
    // TODO: дописать модельку для GetJournalAsync
}
