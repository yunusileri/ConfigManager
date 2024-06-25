using System;
using System.Threading.Tasks;

namespace ConfigManager.Core.Cache
{
    public interface ICacheService
    {
        Task AddAsync(string key, string value, TimeSpan expire);
        T GetFromJson<T>(string key);
        Task DeleteAsync(string key);
    }
}