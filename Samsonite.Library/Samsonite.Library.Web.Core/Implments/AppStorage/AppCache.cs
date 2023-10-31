using Microsoft.Extensions.Caching.Memory;
using System;

namespace Samsonite.Library.Web.Core
{
    public class AppCache : IAppCache
    {
        private IMemoryCache _memoryCache;
        private string _cacheKey = string.Empty;
        public AppCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _cacheKey = AppGlobalConst.GetGlobalStaticConfig.CacheKey;
        }

        /// <summary>
        /// 存储Cache值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="effectTime"></param>
        public void Set(string key, object value, int effectTime)
        {
            _memoryCache.Set($"{_cacheKey}_{key}", value, new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(effectTime)));
        }

        /// <summary>
        /// 读取Cache值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            object _object;
            if (!_memoryCache.TryGetValue($"{_cacheKey}_{key}", out _object))
            {
                _object = null;
            }
            return _object;
        }

        /// <summary>
        /// 清空Cache
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            _memoryCache.Remove($"{_cacheKey}_{key}");
        }
    }
}
