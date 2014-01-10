using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.CacheAccess;

namespace SocialBootstrapApi.Logic
{
    public class NullCacheClient : ICacheClient
    {
        public bool Add<T>(string key, T value, TimeSpan expiresIn)
        {
            return true;
        }

        public bool Add<T>(string key, T value, DateTime expiresAt)
        {
            return true;
        }

        public bool Add<T>(string key, T value)
        {
            return true;
        }

        public long Decrement(string key, uint amount)
        {
            return amount--;
        }

        public void FlushAll()
        {
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            return new Dictionary<string, T>();
        }

        public long Increment(string key, uint amount)
        {
            return amount++;
        }

        public bool Remove(string key)
        {
            return true;
        }

        public void RemoveAll(IEnumerable<string> keys)
        {
            
        }

        public bool Replace<T>(string key, T value, TimeSpan expiresIn)
        {
            return true;
        }

        public bool Replace<T>(string key, T value, DateTime expiresAt)
        {
            return true;
        }

        public bool Replace<T>(string key, T value)
        {
            return true;
        }

        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            return true;
        }

        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            return true;
        }

        public bool Set<T>(string key, T value)
        {
            return true;
        }

        public void SetAll<T>(IDictionary<string, T> values)
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}