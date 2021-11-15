using Microsoft.Extensions.Caching.Memory;

namespace Felix.Data.Core.Configuration
{
    public static class CacheExtensions
    {
        public static MemoryCache ConnectionString { get; private set; } = new MemoryCache(new MemoryCacheOptions());
    }
}
