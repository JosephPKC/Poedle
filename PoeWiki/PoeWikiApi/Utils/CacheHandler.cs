using Microsoft.Extensions.Caching.Memory;

namespace PoeWikiApi.Utils
{
    internal class CacheHandler<K, V> : IDisposable
    {
        private MemoryCache _cache;
        private int absExpInSecDefault;
        private int slidingExpInSecDefault;

        public CacheHandler()
        {
            _cache = new(new MemoryCacheOptions() { SizeLimit = 1024 });
            absExpInSecDefault = 1;
            slidingExpInSecDefault = 1;
        }

        public CacheHandler(long pSizeLimit, int pAbsExpInSecDefault = 600, int pSlidingExpInSecDefault = 300)
        {
            _cache = new(new MemoryCacheOptions() { SizeLimit = pSizeLimit });
            absExpInSecDefault = pAbsExpInSecDefault;
            slidingExpInSecDefault = pSlidingExpInSecDefault;
        }

        public V? Get(K pKey)
        {
            if (pKey == null)
            {
                return default;
            }

            return _cache.Get<V>(pKey);
        }

        public void Set(K pKey, V pValue, int pAbsExpInSec = 0, int pSlidingExpInSec = 0)
        {
            if (pKey == null)
            {
                return;
            }

            var memCacheOptions = new MemoryCacheEntryOptions()
            {
                Size = 1,
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(pAbsExpInSec > 0 ? pAbsExpInSec : absExpInSecDefault),
                SlidingExpiration = TimeSpan.FromSeconds(pSlidingExpInSec > 0 ? pSlidingExpInSec : slidingExpInSecDefault)
            };

            _cache.Set(pKey, pValue, memCacheOptions);
        }

        public void Flush(long pSizeLimit)
        {
            _cache.Dispose();
            _cache = new(new MemoryCacheOptions() { SizeLimit = pSizeLimit });
        }

        public void Dispose()
        {
            _cache.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
