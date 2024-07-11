using Microsoft.Extensions.Caching.Memory;

namespace BaseToolsUtils.Caching
{
    public class CacheHandler<K, V> : IDisposable
    {
        private MemoryCache _cache;

        public int AbsExpInSecDefault { get; private set; }
        public int SlidingExpInSecDefault { get; private set; }

        public CacheHandler()
        {
            _cache = new(new MemoryCacheOptions() { SizeLimit = 1024 });
            AbsExpInSecDefault = 1;
            SlidingExpInSecDefault = 1;
        }

        public CacheHandler(long pSizeLimit, int pAbsExpInSecDefault = 600, int pSlidingExpInSecDefault = 300)
        {
            _cache = new(new MemoryCacheOptions() { SizeLimit = pSizeLimit });
            AbsExpInSecDefault = pAbsExpInSecDefault;
            SlidingExpInSecDefault = pSlidingExpInSecDefault;
        }

        public V? Get(K pKey)
        {
            if (pKey == null) return default;

            return _cache.Get<V>(pKey);
        }

        public void Set(K pKey, V pValue, int pAbsExpInSec = 0, int pSlidingExpInSec = 0)
        {
            if (pKey == null) return;

            MemoryCacheEntryOptions memCacheOptions = new()
            {
                Size = 1,
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(pAbsExpInSec > 0 ? pAbsExpInSec : AbsExpInSecDefault),
                SlidingExpiration = TimeSpan.FromSeconds(pSlidingExpInSec > 0 ? pSlidingExpInSec : SlidingExpInSecDefault)
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
