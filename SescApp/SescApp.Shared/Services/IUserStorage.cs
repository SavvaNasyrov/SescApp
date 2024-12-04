using SescApp.Integration.Lycreg.Models;
using SescApp.Shared.Models;

namespace SescApp.Shared.Services
{
    public interface IUserStorage
    {
        public UserStorageModel Model { get; }

        public Task SaveChangesAsync();

        public Task InitAsync();
    }
}
