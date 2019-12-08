using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Convey.Types;
using MongoDB.Driver;
using Pacco.Services.Availability.Tests.Shared.Helpers;

namespace Pacco.Services.Availability.Tests.Shared.Fixtures
{
    public class MongoDbFixture<TEntity, TKey> : IDisposable where TEntity : class, IIdentifiable<TKey> 
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<TEntity> _collection;
        private readonly string _databaseName;
        private readonly string _collectionName;

        bool _disposed = false;

        public MongoDbFixture(string collectionName)
        {
            var options = OptionsHelper.GetOptions<MongoDbOptions>("mongo");
            _client = new MongoClient(options.ConnectionString);
            _databaseName = options.Database;
            _collectionName = collectionName;
            _database = _client.GetDatabase(_databaseName);
            //InitializeMongo();
            _collection = _database.GetCollection<TEntity>(_collectionName);
        }
        
        private void InitializeMongo()
            => new MongoDbFixtureInitializer(_database, null, new MongoDbOptions())
                .InitializeAsync().GetAwaiter().GetResult();

        public Task InsertAsync(TEntity entity)
            => _database.GetCollection<TEntity>(_collectionName).InsertOneAsync(entity);
        
        public Task<TEntity> GetAsync(TKey id)
            => _collection.Find(d => d.Id.Equals(id)).SingleOrDefaultAsync();
        
        public async Task GetAsync(TKey expectedId, TaskCompletionSource<TEntity> receivedTask)
        {
            if (expectedId is null)
            {
                throw new ArgumentNullException(nameof(expectedId));
            }

            var entity = await GetAsync(expectedId);
            
            if (entity is null)
            {
                receivedTask.TrySetCanceled();
                return;
            }
            
            receivedTask.TrySetResult(entity);
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