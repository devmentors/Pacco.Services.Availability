using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MongoDB.Driver;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Queries;
using Pacco.Services.Availability.Core.Repositories;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Queries.Handlers
{
    public sealed class GetResourceHandler : IQueryHandler<GetResource, ResourceDto>
    {
        private readonly IMongoDatabase _mongoDatabase;

        public GetResourceHandler(IMongoDatabase mongoDatabase)
            => _mongoDatabase = mongoDatabase;
        
        public async Task<ResourceDto> HandleAsync(GetResource query)
        {
            var resource = await _mongoDatabase
                .GetCollection<ResourceDocument>("resources")
                .Find(r => r.Id == query.ResourceId)
                .SingleOrDefaultAsync();

            return resource?.AsDto();
        }
    }
}