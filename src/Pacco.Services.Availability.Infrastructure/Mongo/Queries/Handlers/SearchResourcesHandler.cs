using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MongoDB.Driver;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Queries;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Queries.Handlers
{
    public class SearchResourcesHandler : IQueryHandler<SearchResources, IEnumerable<ResourceDto>>
    {
        public SearchResourcesHandler(IMongoDatabase dataI)
        {
            
        }
        
        public Task<IEnumerable<ResourceDto>> HandleAsync(SearchResources query)
        {
            throw new System.NotImplementedException();
        }
    }
}