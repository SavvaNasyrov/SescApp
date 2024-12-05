
using System.Text.Json;
using SescApp.Shared.Services;

namespace SescApp.Services
{
    public class UserStorage : IUserStorage
    {
        public async Task<TValue?> GetAsync<TValue>(StoredDataType key)
        {
            var json = await SecureStorage.GetAsync(key.ToString());

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

        public async Task SetAsync(StoredDataType key, object val)
        {
            var json = JsonSerializer.Serialize(val);

            await SecureStorage.SetAsync(key.ToString(), json);
        }
    }
}
