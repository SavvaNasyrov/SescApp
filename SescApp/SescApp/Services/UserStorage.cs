using Microsoft.Extensions.Configuration;
using SescApp.Shared.Models;
using SescApp.Shared.Services;
using System.Text.Json;

namespace SescApp.Services
{
    public class UserStorage(IConfiguration config) : IUserStorage
    {
        private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, config["Paths:UserStorage"] ?? "appdata.json");

        public UserStorageModel Model { get; private set; } = UserStorageModel.Default;

        public async Task SaveChangesAsync()
        {
            var data = JsonSerializer.Serialize(Model);

            await File.WriteAllTextAsync(_filePath, data);
        }

        public async Task InitAsync()
        {
            if (!File.Exists(_filePath))
            {
                Model = UserStorageModel.Default;
                await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(Model));
            }
            else
            {
                Model = await JsonSerializer.DeserializeAsync<UserStorageModel>(File.OpenRead(_filePath))
                    ?? throw new InvalidDataException($"File: {_filePath} has incorrect format for UserStorageModel");
            }
        }
    }
}
