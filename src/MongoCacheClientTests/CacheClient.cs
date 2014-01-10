using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MongoCacheClient;

namespace MongoCacheClientTests
{
    [TestFixture]
    public class CacheClientTests
    {
        protected CacheClient _client;

        [TestFixtureSetUp]
        public void Setup()
        {
            var connStr = "mongodb://rob:testtest@ds063158.mongolab.com:63158/robertbird";
            var dbName = "robertbird";
            _client = new CacheClient(connStr, dbName);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            if(_client != null)
                _client.Dispose();
        }

        [Test]
        public void SetNewCacheItemShouldReturnTrue()
        {
            string key = "urn:iauthsession:dpssDdsJZtD/wGEbaVFM";
            string value = "testValue";

            var result = _client.Set<string>(key, value);
            Assert.True(result);
        }

        [Test]
        public void GetAllItemShouldANewlyAddedItem()
        {
            string key = "urn:iauthsession:dpssDdsJZtD/wGEbaVFM";
            string value = "testValue";

            _client.Set<string>(key, value);
            var allItems = _client.GetAll<string>(new List<string>() { key });
            Assert.True(allItems.ContainsKey(key));
            Assert.NotNull(allItems[key]);
            _client.Remove(key);
        }

        [Test]
        public void AddNewCacheItemShouldReturnTrue()
        {
            string key = "urn:iauthsession:dpssDdsJZtD/wGEbaVFM";
            string value = "testValue";

            _client.Add<string>(key, value);
        }

        [Test]
        public void GetNonExistantKeyShouldReturnNull()
        {
            string key = "doesnotexist";

            var item = _client.Get<string>(key);
            Assert.IsNullOrEmpty(item);
        }


    }
}
