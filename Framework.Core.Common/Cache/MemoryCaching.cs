using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Framework.Core.Common
{
    public class MemoryCaching : ICache
    {
        private readonly IMemoryCache _cache;
        public MemoryCaching(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Clear()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _cache.GetType().GetField("_entries", flags).GetValue(_cache);
            var cacheItems = entries as IDictionary;
            if (cacheItems == null) return;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                _cache.Remove(cacheItem.Key);
            }
        }

        public TEntity Get<TEntity>(string key)
        {
            return (TEntity)_cache.Get(key);
        }

        public bool Get(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            return _cache.TryGetValue(key, out _);
        }

        public string GetValue(string key)
        {
            var value = _cache.Get(key);
            if (value != null)
            {
                return value.ToJson();
            }
            return null;
        }

        public void Remove(string key)
        {
             _cache.Remove(key);
        }

        public void Set(string key, object value, TimeSpan cacheTime)
        {
            _cache.Set(key, value, cacheTime);
        }
    }
}
