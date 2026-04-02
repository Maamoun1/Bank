namespace BusinessLayer.Service.IService
{
    public interface IDistributedLock
    {
        Task<string?> AcquireAsync(string key, TimeSpan expiry);
        Task ReleaseAsync(string key, string token);
    }
}