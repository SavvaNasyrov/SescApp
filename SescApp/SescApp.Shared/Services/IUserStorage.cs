
namespace SescApp.Shared.Services
{
    public interface IUserStorage
    {
        public Task SetAsync(string key, object val);

        public Task<TValue?> GetAsync<TValue>(string key);
    }
}
