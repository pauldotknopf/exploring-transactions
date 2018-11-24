using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using Xunit;

namespace Trans.Tests
{
    public class TransactionTests : IDisposable
    {
        private string _database;
        private IDbConnectionFactory _dbConnectionFactory;
        
        public TransactionTests()
        {
            _database = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString().Replace("-", ""));
            _dbConnectionFactory = new OrmLiteConnectionFactory($"Data Source={_database};",  
                SqliteDialect.Provider);
        }
        
        [Fact]
        public void Can_create_tables()
        {
            using (var con = _dbConnectionFactory.CreateDbConnection())
            {
                con.Open();
                con.CreateTable<Person>();
                con.Insert(new Person {Name = "test3", Age = 3});
                var p = con.Select<Person>(x => x.Name == "test").SingleOrDefault();
                p.Name.Should().Be("test");
            }
        }

        public void Dispose()
        {
            File.Delete(_database);
        }
    }
}
