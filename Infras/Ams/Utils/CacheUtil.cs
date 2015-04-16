using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Ams.Utils
{
    /// <summary>
    /// 缓存类
    /// </summary>
    public class CacheUtil
    {
        private static readonly Cache _cache = HttpRuntime.Cache;

        public static IDictionary<string, object> Get(IEnumerable<string> keys)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (string str in keys)
            {
                if (Get(str) != null)
                {
                    dictionary.Add(str, Get(str));
                }
            }
            return dictionary;
        }

        public static object Get(string key)
        {
            return _cache.Get(key);
        }

        public static void Insert(string key, object obj, TimeSpan validFor)
        {
            _cache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, validFor);
        }

        public static void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
