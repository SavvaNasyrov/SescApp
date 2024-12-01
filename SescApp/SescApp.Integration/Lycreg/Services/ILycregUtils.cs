using SescApp.Integration.Lycreg.Models;

namespace SescApp.Integration.Lycreg.Services;

public interface ILycregUtils
{
    public Task<Dictionary<string, string>> GetSubjListAsync(Authorization authData);
}