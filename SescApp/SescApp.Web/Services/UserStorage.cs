using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SescApp.Shared.Services;

namespace SescApp.Web.Services
{
    public class UserStorage(ProtectedLocalStorage localStorage) : IUserStorage
    {
        private readonly ProtectedLocalStorage _localStorage = localStorage;

        public async Task SetAsync(StoredDataType key, object val)
        {
            await _localStorage.SetAsync(key.ToString(), val);
        }

        public async Task<TValue?> GetAsync<TValue>(StoredDataType key)
        {
            var res = await _localStorage.GetAsync<TValue>(key.ToString());

            if (!res.Success)
                return default;

            return res.Value;
        }
    }
}
