using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Queries;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Queries.Handlers
{
    internal sealed class GetResourceHandler : IQueryHandler<GetResource, ResourceDto>
    {
        private readonly IMongoRepository<ResourceDocument, Guid> _repository;

        public GetResourceHandler(IMongoRepository<ResourceDocument, Guid> repository)
            => _repository = repository;

        public async Task<ResourceDto> HandleAsync(GetResource query)
        {
            var document = await _repository.GetAsync(r => r.Id == query.ResourceId);
            return document?.AsDto();
        }
    }
}