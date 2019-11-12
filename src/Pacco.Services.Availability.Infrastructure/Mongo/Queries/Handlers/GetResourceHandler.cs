using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MongoDB.Driver;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Queries;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Queries.Handlers
{
    internal sealed class GetResourceHandler : IQueryHandler<GetResource, ResourceDto>
    {
        private readonly IMongoDatabase _database;

        public GetResourceHandler(IMongoDatabase database)
        {
            _database = database;
        }

        public Task<ResourceDto> HandleAsync(GetResource query)
            => _database.GetCollection<ResourceDto>("ResourcesDto")
                .Find(r => r.Id == query.ResourceId)
                .SingleOrDefaultAsync();
    }
}