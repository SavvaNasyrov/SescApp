
using System.Text.Json;
using SescApp.Shared.Services;

namespace SescApp.Services
{
    public class UserStorage : IUserStorage
    {
        public async Task<TValue?> GetAsync<TValue>(string key)
        {
            var json = await SecureStorage.GetAsync(key);

            if (json == null)
                return default;

            try
            {
                return JsonSerializer.Deserialize<TValue>(json);
            }
            catch
            {
                return default;
            }
        }

        public async Task SetAsync(string key, object val)
        {
            var json = JsonSerializer.Serialize(val);

            await SecureStorage.SetAsync(key, json);
        }
    }
}
