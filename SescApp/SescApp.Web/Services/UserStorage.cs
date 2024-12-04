using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SescApp.Shared.Models;
using SescApp.Shared.Services;

namespace SescApp.Web.Services
{
    public class UserStorage : IUserStorage
    {
        public UserStorageModel Model { get; private set; } = UserStorageModel.Default;

        private readonly ProtectedLocalStorage _localStorage;

        private readonly string _key;

        public UserStorage(IConfiguration config, ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
            _key = config["Paths:UserStorage"] ?? "appdata";
        }

        public async Task SaveChangesAsync()
        {
            await _localStorage.SetAsync(_key, Model);
        }

        public async Task InitAsync()
        {
            var result = await _localStorage.GetAsync<UserStorageModel>(_key).AsTask();

            if (!result.Success || result.Value is null)
            {
                Model = UserStorageModel.Default;
                await _localStorage.SetAsync(_key, Model).AsTask();
            }
            else
            {
                Model = result.Value;
            }
        }
    }
}
