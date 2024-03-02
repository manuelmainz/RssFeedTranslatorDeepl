using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssFeedTranslator.Utils
{
    public class CacheTranslator : ITranslator
    {
        private readonly AsyncDictionaryCache<string, string> cache;
        private readonly ITranslator translator;
        private readonly IPersister persister;

        public CacheTranslator(ITranslator translator, IPersister persister)
        {
            this.translator = translator;
            this.persister = persister;

            var dictionary = persister.Load<Dictionary<string, string>>();
            cache = new(dictionary ?? []);
        }

        public async Task<string> Translate(string summary)
        {
            string result = await cache.GetOrAddAsync(summary, translator.Translate);
            
            // persist value
            persister.Store(cache.ToDictionary());

            return result;
        }
    }
}
