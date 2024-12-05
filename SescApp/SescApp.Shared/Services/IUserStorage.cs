
namespace SescApp.Shared.Services
{
    public interface IUserStorage
    {
        public Task SetAsync(StoredDataType key, object val);

        public Task<TValue?> GetAsync<TValue>(StoredDataType key);
    }
}
