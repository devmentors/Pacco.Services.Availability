using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Queries;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Queries.Handlers
{
    internal sealed class GetResourcesHandler : IQueryHandler<GetResources, IEnumerable<ResourceDto>>
    {
        private readonly IMongoRepository<ResourceDocument, Guid> _repository;

        public GetResourcesHandler(IMongoRepository<ResourceDocument, Guid> repository)
            => _repository = repository;

        public async Task<IEnumerable<ResourceDto>> HandleAsync(GetResources query)
        {
            var documents = await _repository.FindAsync(_ => true);
            return documents.Select(d => d.AsDto());
        }
    }
}