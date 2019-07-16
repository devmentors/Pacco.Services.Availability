using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Convey.Types;
using MongoDB.Driver;
using Pacco.Services.Availability.Core.Entities;

namespace Pacco.Services.Availability.IntegrationTests.Fixtures
{
    public class MongoDbFixture<TEntity, TKey> : IDisposable where TEntity : IIdentifiable<TKey> 
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<TEntity> _collection;
        private readonly string _databaseName;
        bool _disposed = false;
        
        public MongoDbFixture(string databaseName, string collectionName)
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _databaseName = databaseName;
            _database = _client.GetDatabase(databaseName);
            InitializeMongo();
            _collection = _database.GetCollection<TEntity>(collectionName);
        }
        
        public void InitializeMongo()
            => new MongoDbFixtureInitializer(_database, null, new MongoDbOptions())
                .InitializeAsync().GetAwaiter().GetResult();

        public Task InsertAsync(string collectionName, TEntity entity)
            => _database.GetCollection<TEntity>(collectionName).InsertOneAsync(entity);
        
        public async Task<TEntity> GetAsync(TKey expectedId)
        {
            if (expectedId == null)
            {
                throw new ArgumentNullException(nameof(expectedId));
            }

            var entity = await _collection.Find(d => d.Id.Equals(expectedId)).SingleOrDefaultAsync();
            
            if (entity is null)
            {
                throw new ArgumentNullException();
            }

            return entity;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            
            if (disposing)
            {
                _client.DropDatabase(_databaseName);
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}