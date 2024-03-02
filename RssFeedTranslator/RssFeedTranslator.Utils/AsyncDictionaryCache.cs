using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace RssFeedTranslator.Utils
{
    public class AsyncDictionaryCache<TKey, TValue>
        where TKey : notnull
    {
        private readonly ConcurrentDictionary<TKey, Task<TValue>> taskDic = new();
        private readonly ConcurrentDictionary<TKey, TValue> valueDic = new();


        public event EventHandler? CacheChanged;

        public AsyncDictionaryCache()
        {
        }

        public AsyncDictionaryCache(IDictionary<TKey, TValue> initialValues)
        {
            if (initialValues is not null)
            {
                foreach(var pair in initialValues)
                {
                    valueDic.TryAdd(pair.Key, pair.Value);
                }
            }
        }

        public async Task<TValue> GetOrAddAsync(TKey key, Func<TKey, Task<TValue>> func)
        {
            if (valueDic.TryGetValue(key, out TValue? v))
            {
                return v;
            }

            TValue value = await taskDic.GetOrAdd(key, k => CreateValue(k, func));
            valueDic.AddOrUpdate(key, value, (k, o) => value);
            taskDic.TryRemove(key, out _);

            return value;
        }

        public Dictionary<TKey, TValue> ToDictionary()
            => valueDic.ToDictionary();

        private async Task<TValue> CreateValue(TKey key, Func<TKey, Task<TValue>> func)
        {
            if (valueDic.TryGetValue(key, out TValue? v))
            {
                return v;
            }

            TValue value = await func(key);
            valueDic.AddOrUpdate(key, value, (k,o) => value);
            return value;
        }
    }
}
