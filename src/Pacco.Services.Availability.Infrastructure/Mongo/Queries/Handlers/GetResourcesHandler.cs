using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Queries;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Queries.Handlers
{
    internal sealed class GetResourcesHandler : IQueryHandler<GetResources, IEnumerable<ResourceDto>>
    {
        private readonly IMongoDatabase _database;

        public GetResourcesHandler(IMongoDatabase database)
            => _database = database;

        public async Task<IEnumerable<ResourceDto>> HandleAsync(GetResources query)
        {
            var collection = _database.GetCollection<ResourceDocument>("resources");

            if (query.Tags is null || !query.Tags.Any())
            {
                var allDocuments = await collection.Find(_ => true).ToListAsync();
                return allDocuments?.Select(d => d.AsDto());
            }

            var queryable = collection.AsQueryable();

            queryable = query.MatchAllTags
                ? queryable.Where(r => r.Tags.All(t => query.Tags.Contains(t)))
                : queryable.Where(r => r.Tags.Any(t => query.Tags.Contains(t)));

            var documents = await queryable.ToListAsync();
            return documents?.Select(r => r.AsDto());
        }
    }
}