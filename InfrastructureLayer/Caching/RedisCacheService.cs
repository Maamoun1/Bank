using BusinessLayer.Service.IService;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
namespace InfrastructureLayer.Caching
{
    public class RedisCacheService : ICacheService
    {

        private readonly IDistributedCache _cahche;

        public RedisCacheService(IDistributedCache cahche)
        {
            _cahche = cahche;
        }


        public async Task<bool> ExistAsync(string key)
        {
            
            var data =  await _cahche.GetStringAsync(key);

            return !string.IsNullOrEmpty(data);

        }

        public  async Task<T?> GetAsync<T>(string key)
        {

            try
            { 

              var data = await _cahche.GetStringAsync(key);
                if (string.IsNullOrEmpty(data))
                    return default;


                return JsonSerializer.Deserialize<T>(data);
            
            }

            catch
            {
                return default;
            }
        }

        public async Task RemoveAsync(string key)
        {

            try
            {
                await _cahche.RemoveAsync(key);
            }

            catch
            {
                // don not crash the application if cache remove fails
            }

        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {

            try
            {

                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(5)
                };

                var serializedData = JsonSerializer.Serialize(value);
                await _cahche.SetStringAsync(key, serializedData, options);

            }

            catch
            {
                //fail silently
                //redis is optimziation layer only
            }


        }
    }
}
