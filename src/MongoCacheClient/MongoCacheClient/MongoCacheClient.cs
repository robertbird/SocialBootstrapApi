using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.CacheAccess;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoCacheClient
{
    public class CacheClient : ICacheClient
    {
        private static bool databaseInitialised = false;
        private bool disposed = false;
        
        private MongoServer mongoServer = null;
        private string _connectionString;
        private string _dbName = "robertbird";
        private string _collectionName = "MongoCacheItems";

        public CacheClient(string connectionString, string dbName, string collectionName = "MongoCacheItems")
        {
            _connectionString = connectionString;
            _dbName = dbName;
            _collectionName = collectionName;
        }

        #region Helper Methods

        private void InitialiseDatabaseCacheCollection()
        {

        }

        private MongoCollection<MongoCacheItem<T>> GetCacheCollection<T>()
        {
            MongoServer server = MongoServer.Create(_connectionString);
            MongoDatabase database = server[_dbName];
            MongoCollection<MongoCacheItem<T>> collection = database.GetCollection<MongoCacheItem<T>>(_collectionName);

            if (!databaseInitialised)
            {
                collection.EnsureIndex(new IndexKeysBuilder<MongoCacheItem<T>>().Ascending(x => x.key), IndexOptions.SetUnique(true));
                databaseInitialised = true;
            }

            return collection;
        }

        #endregion

        #region ICacheClient

        private class MongoCacheItem<T>
        {
            [BsonId]
            public MongoDB.Bson.ObjectId _id { get; set; }
            public string key { get; set; }
            public T value { get; set; }
        }


        public bool Add<T>(string key, T value, TimeSpan expiresIn)
        {
            return Add(key, value);
        }

        public bool Add<T>(string key, T value, DateTime expiresAt)
        {
            return Add(key, value);
        }

        public bool Add<T>(string key, T value)
        {
            var cacheItem = new MongoCacheItem<T>() { key = key, value = value };
            var result = GetCacheCollection<T>().Insert(cacheItem);
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
            var query = new QueryBuilder<MongoCacheItem<T>>().EQ(x => x.key, key);
            var result = GetCacheCollection<T>().FindOneAs<MongoCacheItem<T>>(query);
            if(result == null)
            {
                return default(T);
            }
            return result.value;
        }

        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            var allItems = GetCacheCollection<T>().FindAllAs<MongoCacheItem<T>>(); // .ToArray(); 
            Dictionary<string, T> collection = new Dictionary<string,T>();

            foreach (var item in allItems.ToArray<MongoCacheItem<T>>())
            {
                collection.Add(item.key, item.value);
            }
            return collection;
        }

        public long Increment(string key, uint amount)
        {
            return amount++;
        }

        public bool Remove(string key)
        {
            var query = new QueryBuilder<MongoCacheItem<object>>().EQ(x => x.key, key);
            var result = GetCacheCollection<MongoCacheItem<object>>().Remove(query);
            return true;
        }

        public void RemoveAll(IEnumerable<string> keys)
        {
            var query = new QueryBuilder<MongoCacheItem<object>>().In(x => x.key, keys);
            GetCacheCollection<MongoCacheItem<object>>().Remove(query);
        }

        public bool Replace<T>(string key, T value, TimeSpan expiresIn)
        {
            return Replace(key, value);
        }

        public bool Replace<T>(string key, T value, DateTime expiresAt)
        {
            return Replace(key, value);
        }

        public bool Replace<T>(string key, T value)
        {
            return Add(key, value);
        }

        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            return Set(key, value);
        }

        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            return Set(key, value);
        }

        public bool Set<T>(string key, T value)
        {
            return Add(key, value);
        }

        public void SetAll<T>(IDictionary<string, T> values)
        {
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (mongoServer != null)
                    {
                        this.mongoServer.Disconnect();
                    }
                }
            }


            this.disposed = true;
        }

        #endregion

    }
}
