
using System;
using System.Collections.Generic;
using MongoDB.Driver;
using NUnit.Framework;

namespace GMATechProject.Domain.Tests
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public abstract class TestBase
	{
		protected MongoServer _server;
        private MongoDatabase _database;

        [TestFixtureSetUp]
        public virtual void SetupFixture()
        {
            _server = MongoServer.Create("mongodb://localhost");
            _database = _server.GetDatabase("test");
        }

        [SetUp]
        public virtual void SetupTest()
        { }

        [TearDown]
        public virtual void TearDownTest()
        { }

        [TestFixtureTearDown]
        public virtual void TearDownFixture()
        {
            //_server.DropDatabase("test");
        }

        protected MongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public void Log(Dictionary<string, string> info)
        {
            foreach (var item in info)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0} : {1}", item.Key, item.Value));
            }
        }

        public void Log(string info)
        {
            System.Diagnostics.Trace.WriteLine(info);
        }
	}
}
